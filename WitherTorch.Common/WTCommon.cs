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
    }
}
