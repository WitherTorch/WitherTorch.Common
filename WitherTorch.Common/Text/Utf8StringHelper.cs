using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    // .NET 預設的 UTF8 編碼實現會在內部複製字串，增加GC壓力和降低編碼效率，故實作此 UTF8 編解碼實現來減少開銷
    internal static class Utf8StringHelper
    {
        // UTF-16 代理對範圍
        private const char Utf16LeadSurrogateStart = unchecked((char)0b_1101_1000_0000_0000);
        private const char Utf16LeadSurrogateEnd = unchecked((char)0b_1101_1011_1111_1111);
        private const char Utf16TrailSurrogateStart = unchecked((char)0b_1101_1100_0000_0000);
        private const char Utf16TrailSurrogateEnd = unchecked((char)0b_1101_1111_1111_1111);
        private const char Utf16SurrogateMask = unchecked((char)0b_0000_0011_1111_1111);

        // Utf16 編碼限制
        private const uint Utf16EncodingLimit = 0x10FFFFu;

        // UTF-8 碼點邊界
        private const uint Utf8Section1Limit = 0x007Fu;
        private const uint Utf8Section2Limit = 0x07FFu;
        private const uint Utf8Section3Limit = 0xFFFFu;
        // RFC 3629: 禁用 5 bytes 和 6 bytes 的 UTF-8 編組，且 UTF-8 最多支援至 U+10FFFF 碼點
        private const uint Utf8EncodingLimit = 0x10FFFFu;
        //private const uint Utf8Section4Limit = 0x1FFFFFu;
        // private const uint Utf8Section5Limit = 0x3FFFFFFu;
        // private const uint Utf8Section6Limit = 0x7FFFFFFFu;

        // UTF-8 頭部位元組
        private const byte Utf8Section2Head = 0b110_00000;
        private const byte Utf8Section2Mask = 0b000_11111;
        private const byte Utf8Section3Head = 0b1110_0000;
        private const byte Utf8Section3Mask = 0b0000_1111;
        private const byte Utf8Section4Head = 0b11110_000;
        private const byte Utf8Section4Mask = 0b00000_111;

        // UTF-8 尾部位元組
        private const byte Utf8TrailHeader = 0b10_000000;
        private const byte Utf8TrailMask = 0b00_111111;

        // UTF-8 解碼錯誤替代字元 (U+FFFD)
        private const uint Utf8DecodeErrorCodePoint = 0xFFFD;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_Utf16(byte* a, char* b, nuint length)
        {
            byte* aEnd = (byte*)UnsafeHelper.PointerMaxValue;
            char* bEnd = b + length;
            while ((b = TryReadUtf16Character(b, bEnd, out uint unicodeValueB)) != null)
            {
                if ((a = TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null)
                    return 1;
                int comparison = unicodeValueA.CompareTo(unicodeValueB);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_Latin1(byte* a, byte* b, nuint length)
        {
            byte* aEnd = (byte*)UnsafeHelper.PointerMaxValue;
            byte* bEnd = b + length;
            for (nuint i = 0; i < length; i++)
            {
                uint unicodeValueB = b[i];
                if ((a = TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null)
                    return 1;
                int comparison = unicodeValueA.CompareTo(unicodeValueB);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals_Latin1(byte* a, byte* b, nuint length)
        {
            byte* aEnd = (byte*)UnsafeHelper.PointerMaxValue;
            for (nuint i = 0; i < length; i++)
            {
                if ((a = TryReadUtf8Character(a, aEnd, out uint unicodeValue)) == null ||
                    unicodeValue != b[i])
                    return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals_Utf16(byte* a, char* b, nuint length)
        {
            byte* aEnd = (byte*)UnsafeHelper.PointerMaxValue;
            char* bEnd = b + length;
            while ((b = TryReadUtf16Character(b, bEnd, out uint unicodeValueB)) != null)
            {
                if ((a = TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null ||
                    unicodeValueA != unicodeValueB)
                    return false;
            }
            return true;
        }

        public static unsafe char* PointerIndexOf(char* ptr, nuint count, byte* value, nuint valueLength)
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, count, unchecked((char)*value));

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* ptrBuffer = buffer)
                {
                    WriteToUtf16Buffer(value, (byte*)UnsafeHelper.PointerMaxValue, ptrBuffer, ptrBuffer + valueLength);
                    return InternalSequenceHelper.PointerIndexOf(ptr, count, ptrBuffer, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        public static unsafe byte* PointerIndexOf(byte* ptr, nuint count, char value)
        {
            if (value <= '\u007f')
                return PointerIndexOfVeryFast(ptr, count, value);
            if (IsLeadSurrogate(value))
                return PointerIndexOfSlow_LeadSurrogate(ptr, count, value);
            if (IsTrailSurrogate(value))
                return PointerIndexOfSlow_TrailSurrogate(ptr, count, value);
            return PointerIndexOfFast(ptr, count, value);
        }

        private static unsafe byte* PointerIndexOfVeryFast(byte* ptr, nuint count, char value)
            => SequenceHelper.PointerIndexOf(ptr, count, unchecked((byte)value));

        private static unsafe byte* PointerIndexOfFast(byte* ptr, nuint count, char value)
        {
            const int SingleCharMaxSpaceLength = 3;

            byte* buffer = stackalloc byte[SingleCharMaxSpaceLength]; // 最大編碼空間
            byte* bufferEnd = TryWriteUtf8Character(buffer, buffer + SingleCharMaxSpaceLength, value);
            if (bufferEnd == null)
            {
                // 不可能發生的情況
                DebugHelper.Throw("Impossible route!");
            }

            return InternalSequenceHelper.PointerIndexOf(ptr, count, buffer, unchecked((nuint)(bufferEnd - buffer)));
        }

        private static unsafe byte* PointerIndexOfSlow_LeadSurrogate(byte* ptr, nuint count, char value)
        {
            byte* ptrNext, ptrEnd = ptr + count;
            while ((ptrNext = TryReadUtf8Character(ptr, ptrEnd, out uint unicodeValue)) == null)
            {
                if (unicodeValue == value ||
                    ToUtf16Characters(unicodeValue, out char compareChar, out _) && compareChar == value)
                    return ptr;
                ptr = ptrNext;
            }
            return null;
        }

        private static unsafe byte* PointerIndexOfSlow_TrailSurrogate(byte* ptr, nuint count, char value)
        {
            byte* ptrNext, ptrEnd = ptr + count;
            while ((ptrNext = TryReadUtf8Character(ptr, ptrEnd, out uint unicodeValue)) == null)
            {
                if (unicodeValue == value ||
                    ToUtf16Characters(unicodeValue, out _, out char compareChar) && compareChar == value)
                    return ptr;
                ptr = ptrNext;
            }
            return null;
        }

        private static unsafe byte* PointerIndexOfFast(byte* ptr, nuint count, char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* ptrBuffer = buffer)
                {
                    Latin1StringHelper.NarrowAndCopyTo(value, valueLength, ptrBuffer);
                    return InternalSequenceHelper.PointerIndexOf(ptr, count, ptrBuffer, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* TryReadUtf16Character(char* ptr, char* ptrEnd, out uint unicodeValue)
        {
            if (ptr >= ptrEnd)
            {
                unicodeValue = 0;
                return null;
            }

            char leadSurrogate = ptr[0];
            if (!IsLeadSurrogate(leadSurrogate) || ptr + 1 < ptrEnd)
                goto Single;

            char trailSurrogate = ptr[1];
            if (!IsTrailSurrogate(trailSurrogate) || trailSurrogate > Utf16TrailSurrogateEnd)
                goto Single;

            unicodeValue = unchecked((uint)(((leadSurrogate - Utf16LeadSurrogateStart) << 10) + (trailSurrogate - Utf16TrailSurrogateEnd)));
            return ptr + 2;

        Single:
            unicodeValue = leadSurrogate;
            return ptr + 1;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool IsLeadSurrogate(char c) => c >= Utf16LeadSurrogateStart && c <= Utf16LeadSurrogateEnd;

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool IsTrailSurrogate(char c) => c >= Utf16TrailSurrogateStart && c <= Utf16TrailSurrogateStart;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* TryWriteUtf16Character(char* ptr, char* ptrEnd, uint unicodeValue)
        {
            if (ptr >= ptrEnd)
                return null;

            if (ToUtf16Characters(unicodeValue, out char leadSurrogate, out char trailSurrogate))
            {
                *ptr++ = leadSurrogate;
                if (ptr >= ptrEnd)
                    return ptr;
                *ptr++ = trailSurrogate;
            }
            else
            {
                *ptr++ = leadSurrogate;
            }

            return ptr;
        }

        /// <summary>
        /// 將 Unicode 碼轉換成 UTF-16 字元組
        /// </summary>
        /// <param name="unicodeValue">要轉換的 Unicode 碼</param>
        /// <param name="leadSurrogate">如果回傳值為 <see langword="true"/>，則為 UTF-16 代理對中的前導字元；反之則為普通的 UTF-16 字元</param>
        /// <param name="trailSurrogate">如果回傳值為 <see langword="true"/>，則為 UTF-16 代理對中的後尾字元；反之則為 0</param>
        /// <returns>轉換結果是否有代理對 (Surrogate Pair)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ToUtf16Characters(uint unicodeValue, out char leadSurrogate, out char trailSurrogate)
        {
            if (unicodeValue <= 0xFFFF)
            {
                leadSurrogate = unchecked((char)unicodeValue);
                trailSurrogate = default;
                return false;
            }
            if (unicodeValue > Utf16EncodingLimit)
            {
                leadSurrogate = '\uFFFD';
                trailSurrogate = default;
                return false;
            }
            leadSurrogate = unchecked((char)(((unicodeValue >> 10) & Utf16SurrogateMask) | Utf16LeadSurrogateStart));
            trailSurrogate = unchecked((char)((unicodeValue & Utf16SurrogateMask) | Utf16TrailSurrogateStart));
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        private static bool IsTrailByte(byte value) => (value & ~Utf8TrailMask) == Utf8TrailHeader;

        [Inline(InlineBehavior.Remove)]
        private static uint DecodeTrailByte(byte value) => unchecked((uint)(value & Utf8TrailMask));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ReadFromUtf16Buffer(char* source, char* sourceEnd, byte* destination, byte* destinationEnd)
        {
            while ((source = TryReadUtf16Character(source, sourceEnd, out uint unicodeValue)) != null &&
                (destination = TryWriteUtf8Character(destination, destinationEnd, unicodeValue)) != null) ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteToUtf16Buffer(byte* source, byte* sourceEnd, char* destination, char* destinationEnd)
        {
            while ((source = TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) != null &&
                (destination = TryWriteUtf16Character(destination, destinationEnd, unicodeValue)) != null) ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* TrySkipUtf8Characters(byte* ptr, byte* ptrEnd, nuint skipCount)
        {
            for (nuint i = 0; i < skipCount; i++)
            {
                if ((ptr = TrySkipUtf8Character(ptr, ptrEnd)) == null)
                    return null;
            }
            return ptr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* TrySkipUtf8Character(byte* ptr, byte* ptrEnd)
        {
            if (ptr >= ptrEnd)
                return null;

            byte* ptrNext;
            byte leadingByte = *ptr;
            if (leadingByte <= Utf8Section1Limit) // Section 1
                ptrNext = ptr + 1;
            else if ((leadingByte & ~Utf8Section2Mask) == Utf8Section2Head) // Section 2
            {
                ptrNext = ptr + 2;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                if (!IsTrailByte(trailingByte))
                    goto Failed;
            }
            else if ((leadingByte & ~Utf8Section3Mask) == Utf8Section3Head) // Section 3
            {
                ptrNext = ptr + 3;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                byte trailingByte2 = ptr[2];
                if (!IsTrailByte(trailingByte) || !IsTrailByte(trailingByte2))
                    goto Failed;
            }
            else if ((leadingByte & ~Utf8Section4Mask) == Utf8Section4Head) // Section 4
            {
                ptrNext = ptr + 4;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                byte trailingByte2 = ptr[2];
                byte trailingByte3 = ptr[3];
                if (!IsTrailByte(trailingByte) || !IsTrailByte(trailingByte2) || !IsTrailByte(trailingByte3))
                    goto Failed;
            }
            else
                goto Failed;

            return ptrNext;

        OutRange:
            return null;

        Failed:
            return ptr + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* TryReadUtf8Character(byte* ptr, byte* ptrEnd, out uint unicodeValue)
        {
            if (ptr >= ptrEnd)
            {
                unicodeValue = 0;
                return null;
            }

            byte* ptrNext;
            byte leadingByte = *ptr;
            if (leadingByte <= Utf8Section1Limit) // Section 1
            {
                ptrNext = ptr + 1;
                unicodeValue = leadingByte;
            }
            else if ((leadingByte & ~Utf8Section2Mask) == Utf8Section2Head) // Section 2
            {
                ptrNext = ptr + 2;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                if (!IsTrailByte(trailingByte))
                    goto Failed;
                unicodeValue = unchecked(((uint)leadingByte & Utf8Section2Mask) << 6 | DecodeTrailByte(trailingByte));
            }
            else if ((leadingByte & ~Utf8Section3Mask) == Utf8Section3Head) // Section 3
            {
                ptrNext = ptr + 3;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                byte trailingByte2 = ptr[2];
                if (!IsTrailByte(trailingByte) || !IsTrailByte(trailingByte2))
                    goto Failed;
                unicodeValue = unchecked(((uint)leadingByte & Utf8Section3Mask) << 12 | DecodeTrailByte(trailingByte) << 6 | DecodeTrailByte(trailingByte2));
            }
            else if ((leadingByte & ~Utf8Section4Mask) == Utf8Section4Head) // Section 4
            {
                ptrNext = ptr + 4;
                if (ptrNext > ptrEnd)
                    goto OutRange;

                byte trailingByte = ptr[1];
                byte trailingByte2 = ptr[2];
                byte trailingByte3 = ptr[3];
                if (!IsTrailByte(trailingByte) || !IsTrailByte(trailingByte2) || !IsTrailByte(trailingByte3))
                    goto Failed;
                unicodeValue = unchecked(((uint)leadingByte & Utf8Section3Mask) << 18 | DecodeTrailByte(trailingByte) << 12 |
                    DecodeTrailByte(trailingByte2) << 6 | DecodeTrailByte(trailingByte3));
            }
            else
                goto Failed;

            return ptrNext;

        OutRange:
            unicodeValue = 0;
            return null;

        Failed:
            unicodeValue = Utf8DecodeErrorCodePoint;
            return ptr + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* TryWriteUtf8Character(byte* ptr, byte* ptrEnd, uint unicodeValue)
        {
            byte* ptrNext;
            if (unicodeValue <= Utf8Section1Limit)
            {
                ptrNext = ptr + 1;
                if (ptrNext > ptrEnd)
                    return null;

                ptr[0] = unchecked((byte)unicodeValue);
            }
            else if (unicodeValue <= Utf8Section2Limit)
            {
                ptrNext = ptr + 2;
                if (ptrNext > ptrEnd)
                    return null;

                ptr[0] = unchecked((byte)(((unicodeValue >> 6) & Utf8Section2Mask) | Utf8Section2Head));
                ptr[1] = unchecked((byte)((unicodeValue & Utf8TrailMask) | Utf8TrailHeader));
            }
            else if (unicodeValue <= Utf8Section3Limit)
            {
                ptrNext = ptr + 3;
                if (ptrNext > ptrEnd)
                    return null;

                ptr[0] = unchecked((byte)(((unicodeValue >> 12) & Utf8Section3Mask) | Utf8Section3Head));
                ptr[1] = unchecked((byte)(((unicodeValue >> 6) & Utf8TrailMask) | Utf8TrailHeader));
                ptr[2] = unchecked((byte)((unicodeValue & Utf8TrailMask) | Utf8TrailHeader));
            }
            else if (unicodeValue <= Utf8EncodingLimit)
            {
                ptrNext = ptr + 4;
                if (ptrNext > ptrEnd)
                    return null;

                ptr[0] = unchecked((byte)(((unicodeValue >> 18) & Utf8Section4Mask) | Utf8Section4Head));
                ptr[1] = unchecked((byte)(((unicodeValue >> 12) & Utf8TrailMask) | Utf8TrailHeader));
                ptr[2] = unchecked((byte)(((unicodeValue >> 6) & Utf8TrailMask) | Utf8TrailHeader));
                ptr[3] = unchecked((byte)((unicodeValue & Utf8TrailMask) | Utf8TrailHeader));
            }
            else
                goto Failed;

            return ptrNext;
        Failed:
            return null;
        }
    }
}
