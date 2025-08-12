using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

using WitherTorch.Common.Helpers;

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

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsWhiteSpaceCharacter(char c)
        {
            string table = c <= Latin1EncodingHelper.Latin1EncodingLimit
                ? GetWhiteSpaceTableForLatin1(out nuint count)
                : GetWhiteSpaceTableForNonLatin1(out count);

            fixed (char* ptr = table)
                return SequenceHelper.Contains(ptr, count, c);
        }

        [Inline(InlineBehavior.Remove)]
        private static string GetWhiteSpaceTableForLatin1(out nuint count)
        {
            // 空白字元表，來源: https://www.unicode.org/Public/UCD/latest/ucd/PropList.txt
            const int CharacterCount = 8;
            string whiteSpaceCharacters = 
                "\u0009\u000A\u000B\u000C" +
                "\u000D\u0020\u0085\u00A0";

            DebugHelper.ThrowIf(whiteSpaceCharacters.Length != CharacterCount);

            count = CharacterCount;
            return whiteSpaceCharacters;
        }

        [Inline(InlineBehavior.Remove)]
        private static string GetWhiteSpaceTableForNonLatin1(out nuint count)
        {
            // 空白字元表，來源: https://www.unicode.org/Public/UCD/latest/ucd/PropList.txt
            const int CharacterCount = 17;
            string whiteSpaceCharacters = 
                "\u1680\u2000\u2001\u2002" +
                "\u2003\u2004\u2005\u2006" +
                "\u2007\u2008\u2009\u200A" +
                "\u2028\u2029\u202F\u205F" +
                "\u3000";

            DebugHelper.ThrowIf(whiteSpaceCharacters.Length != CharacterCount);

            count = CharacterCount;
            return whiteSpaceCharacters;
        }
    }
}
