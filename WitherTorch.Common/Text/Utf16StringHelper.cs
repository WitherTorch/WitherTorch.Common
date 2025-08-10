using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Text
{
    internal static partial class Utf16StringHelper
    {
        // UTF-16 代理對範圍
        private const char SurrogateStart = unchecked((char)0b_1101_1000_0000_0000);
        private const char SurrogateEnd = unchecked((char)0b_1101_1111_1111_1111);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasSurrogateCharacters(char* ptr, nuint length) 
            => VectorizedHasSurrogateCharacters(ptr, ptr + length);

        private static unsafe bool LegacyHasSurrogateCharacters(char* ptr, char* ptrEnd)
        {
            for (; ptr < ptrEnd; ptr++)
            {
                if (IsSurrogateCharacter(*ptr))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSurrogateCharacter(char c)
            => c >= SurrogateStart && c <= SurrogateEnd;
    }
}
