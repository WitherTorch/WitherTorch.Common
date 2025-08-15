namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Bmi1
    {
        public static partial bool IsSupported { get; }

        public static partial uint TrailingZeroCount(uint value);
    }
}
