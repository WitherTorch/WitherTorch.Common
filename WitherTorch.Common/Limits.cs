namespace WitherTorch.Common
{
    public static class Limits
    {
        public const int MaxStackallocBytes = 4096;

        public const int MaxStackallocChars = MaxStackallocBytes / sizeof(char);

        public const int MaxArrayLength = 0x7FEFFFFF;
    }
}
