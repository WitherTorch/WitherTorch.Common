using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8EncodingHelper
    {
        private static unsafe byte* TryReadFromUtf16BufferCoreFast(char* source, nuint count, byte* destination, byte* destinationEnd) // only for no zero-included data
        {
            ArrayPool<uint> pool = ArrayPool<uint>.Shared;
            uint[] buffer = pool.Rent(count);
            try
            {
                fixed (uint* ptr = buffer)
                {
                    uint* ptrEnd;
                    if (HasSurrogateCharacters(source, count))
                        ptrEnd = TryWriteUtf16ToUtf32BufferCore_HasSurrogate(source, ptr, count);
                    else
                        ptrEnd = TryWriteUtf16ToUtf32BufferCore(source, ptr, count);
                    EncodeUtf32ToUtf8(ptr, count);
                    for (byte* iterator = (byte*)ptr; iterator < ptrEnd; iterator++)
                    {
                        byte b = *iterator;
                        if (b == 0)
                            continue;
                        *destination++ = b;
                    }
                }
            }
            finally
            {
                pool.Return(buffer);
            }
            return destination;
        }

        private static unsafe char* TryWriteToUtf16BufferCoreFast(byte* source, nuint length, char* destination, char* destinationEnd) // only for no zero-included data
        {
            ArrayPool<uint> pool = ArrayPool<uint>.Shared;
            uint[] buffer = pool.Rent(length);
            try
            {
                fixed (uint* ptr = buffer)
                {
                    uint* ptrEnd = TryWriteUtf8ToUtf32BufferCore(source, ptr, length);
                    for (uint* iterator = ptr; iterator < ptrEnd; iterator++)
                    {
                        uint unicodeValue = *iterator;
                        if (unicodeValue == 0)
                            continue;
                        if ((destination = TryWriteUtf16Character(destination, destinationEnd, unicodeValue)) == null)
                            return destinationEnd;
                    }
                }
            }
            finally
            {
                pool.Return(buffer);
            }
            return destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe partial bool HasSurrogateCharacters(char* source, nuint length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe partial uint* TryWriteUtf16ToUtf32BufferCore_HasSurrogate(char* source, uint* destination, nuint length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe partial uint* TryWriteUtf16ToUtf32BufferCore(char* source, uint* destination, nuint length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe partial uint* TryWriteUtf8ToUtf32BufferCore(byte* source, uint* destination, nuint length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe partial void EncodeUtf32ToUtf8(uint* source, nuint length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void EncodeUtf32ToUtf8ForSingleCodePoint(uint* ptr)
        {
            uint unicodeValue = *ptr;
            DebugHelper.ThrowIf(unicodeValue > Utf8EncodingLimit, $"Wrong input at {nameof(EncodeUtf32ToUtf8ForSingleCodePoint)}(uint*): unicodeValue = {unicodeValue}!");

            if (unicodeValue <= 0x007F) // Section 1 (U+0000 ~ U+007F)
                return;

            if (unicodeValue <= 0x07FF) // Section 2 (U+0080 ~ U+07FF)
            {
                *ptr = EncodeUtf32ToUtf8ForSingleCodePoint_Section2(unicodeValue);
                return;
            }

            if (unicodeValue <= 0xFFFF) // Section 3 (U+0800 ~ U+FFFF)
            {
                *ptr = EncodeUtf32ToUtf8ForSingleCodePoint_Section3(unicodeValue);
                return;
            }

            *ptr = EncodeUtf32ToUtf8ForSingleCodePoint_Section4(unicodeValue);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe uint EncodeUtf32ToUtf8ForSingleCodePoint_Section2(uint unicodeValue) 
            => Utf8Section2Head | (unicodeValue >> 6) | (Utf8TrailHeader | (unicodeValue & Utf8TrailMask)) << 8;

        [Inline(InlineBehavior.Remove)]
        private static unsafe uint EncodeUtf32ToUtf8ForSingleCodePoint_Section3(uint unicodeValue) 
            => Utf8Section3Head | (unicodeValue >> 12) | (Utf8TrailHeader | ((unicodeValue >> 6) & Utf8TrailMask)) << 8 |
                    (Utf8TrailHeader | (unicodeValue & Utf8TrailMask)) << 16;

        [Inline(InlineBehavior.Remove)]
        private static unsafe uint EncodeUtf32ToUtf8ForSingleCodePoint_Section4(uint unicodeValue) 
            => Utf8Section4Head | (unicodeValue >> 18) | (Utf8TrailHeader | ((unicodeValue >> 12) & Utf8TrailMask)) << 8 |
                (Utf8TrailHeader | ((unicodeValue >> 6) & Utf8TrailMask)) << 16 | (Utf8TrailHeader | (unicodeValue & Utf8TrailMask)) << 24;
    }
}
