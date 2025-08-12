using System.Runtime.CompilerServices;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Helpers
{
    public static class CharHelper
    {
        /// <inheritdoc cref="char.IsWhiteSpace(char)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(char c)
            => Utf16StringHelper.IsWhiteSpaceCharacter(c);

        /// <inheritdoc cref="char.IsWhiteSpace(string, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(string s, int index)
            => Utf16StringHelper.IsWhiteSpaceCharacter(s[index]);

        /// <inheritdoc cref="char.IsWhiteSpace(char)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(byte c)
            => AsciiLikeStringHelper.IsWhiteSpaceCharacter(c);
    }
}
