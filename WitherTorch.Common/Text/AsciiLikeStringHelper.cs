using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Text
{
    internal static class AsciiLikeStringHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_Utf16(byte* a, char* b, nuint length)
        {
            for (nuint i = 0; i < length; i++)
            {
                int comparison = unchecked((char)a[i]).CompareTo(b[i]);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equals_Utf16(byte* a, char* b, nuint length)
        {
            if (SequenceHelper.ContainsGreaterThan(b, length, Latin1EncodingHelper.Latin1EncodingLimit))
                return false;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(length);
            try
            {
                fixed (byte* ptrBuffer = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(b, ptrBuffer, length);
                    return SequenceHelper.Equals(a, ptrBuffer, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
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
                    Latin1EncodingHelper.WriteToUtf16BufferCore(value, ptrBuffer, valueLength);
                    return InternalSequenceHelper.PointerIndexOf(ptr, count, ptrBuffer, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
