using System.Runtime.CompilerServices;

using WitherTorch.Common.Text;

namespace WitherTorch.Common
{
    public static class WTCommon
    {
        private static StringCreateOptions _stringCreateOptions = StringCreateOptions.None;

        public const bool IsDebug
#if DEBUG
            = true;
#else
            = false;
#endif

        /// <summary>
        /// 是否讓 <see cref="Text.StringBase.Create(string)"/> 和 <see cref="Text.StringBase.Create(char*)"/> 在輸入字元皆為 ASCII 字元時壓縮該字串
        /// </summary>
        public static bool AllowAsciiStringCompression
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (_stringCreateOptions & StringCreateOptions.UseAsciiCompression) == StringCreateOptions.UseAsciiCompression;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value)
                    _stringCreateOptions |= StringCreateOptions.UseAsciiCompression;
                else
                    _stringCreateOptions &= ~StringCreateOptions.UseAsciiCompression;
            }
        }

        /// <summary>
        /// 是否讓 <see cref="Text.StringBase.Create(string)"/> 和 <see cref="Text.StringBase.Create(char*)"/> 在輸入字元皆為 Latin-1 字元時壓縮該字串
        /// </summary>
        public static bool AllowLatin1StringCompression
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (_stringCreateOptions & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseLatin1Compression;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value)
                    _stringCreateOptions |= StringCreateOptions.UseLatin1Compression;
                else
                    _stringCreateOptions &= ~StringCreateOptions.UseLatin1Compression;
            }
        }

        /// <summary>
        /// 是否讓 <see cref="Text.StringBase.Create(string)"/> 和 <see cref="Text.StringBase.Create(char*)"/> UTF-8 編碼長度更優時壓縮該字串
        /// </summary>
        public static bool AllowUtf8StringCompression
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (_stringCreateOptions & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseUtf8Compression;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value)
                    _stringCreateOptions |= StringCreateOptions.UseUtf8Compression;
                else
                    _stringCreateOptions &= ~StringCreateOptions.UseUtf8Compression;
            }
        }

        internal static StringCreateOptions StringCreateOptions => _stringCreateOptions;
    }
}
