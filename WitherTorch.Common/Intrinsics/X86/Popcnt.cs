namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Popcnt
    {
        public static partial bool IsSupported { get; }

        public static partial uint PopCount(uint value);
    }
}
