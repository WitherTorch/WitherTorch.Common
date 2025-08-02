using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal static class Utf8StringHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_Utf16(byte* a, char* b, nuint length)
        {
            byte* aEnd = (byte*)UnsafeHelper.PointerMaxValue;
            char* bEnd = b + length;
            while ((b = Utf8EncodingHelper.TryReadUtf16Character(b, bEnd, out uint unicodeValueB)) != null)
            {
                if ((a = Utf8EncodingHelper.TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null)
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
                if ((a = Utf8EncodingHelper.TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null)
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
                if ((a = Utf8EncodingHelper.TryReadUtf8Character(a, aEnd, out uint unicodeValue)) == null ||
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
            while ((b = Utf8EncodingHelper.TryReadUtf16Character(b, bEnd, out uint unicodeValueB)) != null)
            {
                if ((a = Utf8EncodingHelper.TryReadUtf8Character(a, aEnd, out uint unicodeValueA)) == null ||
                    unicodeValueA != unicodeValueB)
                    return false;
            }
            return true;
        }
    }
}
