using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    // .NET 預設的 UTF8 編碼實現會在內部複製字串，增加GC壓力和降低編碼效率，故實作此 UTF8 編解碼實現來減少開銷
    public static class Utf8EncodingHelper
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
        // private const uint Utf8Section4Limit = 0x1FFFFFu;
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

        // ASCII 編碼邊界
        private const char AsciiEncodingLimit = '\u007f';

        // UTF-8 解碼錯誤替代字元 (U+FFFD)
        public const uint Utf8DecodeErrorCodePoint = 0xFFFD;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe int GetWorstCaseForEncodeLength(int length) => length * 3;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe uint GetWorstCaseForEncodeLength(uint length) => length * 3;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe nuint GetWorstCaseForEncodeLength(nuint length) => length * 3;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe int GetWorstCaseForDecodeLength(int length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe uint GetWorstCaseForDecodeLength(uint length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe nuint GetWorstCaseForDecodeLength(nuint length) => length;

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

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool IsLeadSurrogate(char c) => c >= Utf16LeadSurrogateStart && c <= Utf16LeadSurrogateEnd;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool IsTrailSurrogate(char c) => c >= Utf16TrailSurrogateStart && c <= Utf16TrailSurrogateStart;

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
        public static unsafe byte* ReadFromUtf16Buffer(char* source, nuint sourceLength, byte* destination, nuint destinationLength)
        {
            if (sourceLength == 0 || destinationLength == 0)
                return destination;
            nuint length = MathHelper.Min(sourceLength, destinationLength);
            if (!SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingLimit))
            {
                Latin1StringHelper.NarrowAndCopyTo(source, length, destination);
                return destination + length;
            }
            byte* destinationEnd = destination + destinationLength;
            destination = TryReadFromUtf16BufferCore(source, source + sourceLength, destination, destinationEnd);
            return destination == null ? destinationEnd : destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ReadFromUtf16Buffer(char* source, char* sourceEnd, byte* destination, byte* destinationEnd)
        {
            if (sourceEnd <= source || destinationEnd <= destination)
                return destination;
            nuint length = MathHelper.Min(unchecked((nuint)(sourceEnd - source)), unchecked((nuint)(destinationEnd - destination)));
            if (!SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingLimit))
            {
                Latin1StringHelper.NarrowAndCopyTo(source, length, destination);
                return destination + length;
            }
            destination = TryReadFromUtf16BufferCore(source, sourceEnd, destination, destinationEnd);
            return destination == null ? destinationEnd : destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* TryReadFromUtf16BufferCore(char* source, char* sourceEnd, byte* destination, byte* destinationEnd)
        {
            while ((source = TryReadUtf16Character(source, sourceEnd, out uint unicodeValue)) != null &&
                (destination = TryWriteUtf8Character(destination, destinationEnd, unicodeValue)) != null) ;
            return destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* WriteToUtf16Buffer(byte* source, nuint sourceLength, char* destination, nuint destinationLength)
        {
            if (sourceLength == 0 || destinationLength == 0)
                return destination;
            nuint length = MathHelper.Min(sourceLength, destinationLength);
            if (!SequenceHelper.ContainsGreaterThan(source, length, (byte)AsciiEncodingLimit))
            {
                Latin1StringHelper.WidenAndCopyTo(source, length, destination);
                return destination + length;
            }
            char* destinationEnd = destination + destinationLength;
            destination = TryWriteToUtf16BufferCore(source, source + sourceLength, destination, destinationEnd);
            return destination == null ? destinationEnd : destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* WriteToUtf16Buffer(byte* source, byte* sourceEnd, char* destination, char* destinationEnd)
        {
            if (sourceEnd <= source || destinationEnd <= destination)
                return destination;
            nuint length = MathHelper.Min(unchecked((nuint)(sourceEnd - source)), unchecked((nuint)(destinationEnd - destination)));
            if (!SequenceHelper.ContainsGreaterThan(source, length, (byte)AsciiEncodingLimit))
            {
                Latin1StringHelper.WidenAndCopyTo(source, length, destination);
                return destination + length;
            }
            destination = TryWriteToUtf16BufferCore(source, sourceEnd, destination, destinationEnd);
            return destination == null ? destinationEnd : destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe char* TryWriteToUtf16BufferCore(byte* source, byte* sourceEnd, char* destination, char* destinationEnd)
        {
            while ((source = TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) != null &&
                (destination = TryWriteUtf16Character(destination, destinationEnd, unicodeValue)) != null) ;
            return destination;
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
