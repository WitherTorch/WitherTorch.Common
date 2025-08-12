using System;
using System.Runtime.CompilerServices;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

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

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsWhiteSpaceCharacter(byte c)
        {
            // 空白字元表，來源: https://www.unicode.org/Public/UCD/latest/ucd/PropList.txt
            const int CharacterCount = 8;
            ReadOnlySpan<byte> whiteSpaceCharacters = "\u0009\u000A\u000B\u000C\u000D\u0020\u0085\u00A0"u8;

            DebugHelper.ThrowIf(whiteSpaceCharacters.Length != CharacterCount);

            fixed (byte* ptr = whiteSpaceCharacters)
                return SequenceHelper.Contains(ptr, CharacterCount, c);
        }
    }
}
