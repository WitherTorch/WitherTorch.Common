namespace WitherTorch.Common
{
    public static class Limits
    {
        /// <summary>
        /// 在堆疊上配置位元組區塊的上限值
        /// </summary>
        public const int MaxStackallocBytes = 4096;
        /// <summary>
        /// 在堆疊上配置字元區塊的上限值
        /// </summary>
        public const int MaxStackallocChars = MaxStackallocBytes / sizeof(char);
        /// <summary>
        /// 陣列的最大容許大小
        /// </summary>
        public const int MaxArrayLength = 0x7FEFFFFF;
    }
}
