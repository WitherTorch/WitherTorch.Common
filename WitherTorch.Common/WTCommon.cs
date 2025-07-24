namespace WitherTorch.Common
{
    public static class WTCommon
    {
        public const bool IsDebug
#if DEBUG
            = true;
#else
            = false;
#endif

        /// <summary>
        /// 是否讓 <see cref="Text.StringBase.Create(string)"/> 和 <see cref="Text.StringBase.Create(char*)"/> 在輸入字元皆為 Latin-1 字元時壓縮該字串
        /// </summary>
        public static bool AllowLatin1StringCompression { get; set; } = false;
    }
}
