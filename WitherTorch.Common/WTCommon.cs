namespace WitherTorch.Common
{
    public static class WTCommon
    {
        /// <summary>
        /// 是否可以在 <see cref="Text.StringBuilderTiny"/> 上使用 <see cref="Text.StringBuilderTiny.SetStartPointer(char*, int)"/> 來配置堆疊位元組區塊 (參考用，無實質限制)
        /// </summary>
        public const bool UseStackallocStringBuilder = true;
    }
}
