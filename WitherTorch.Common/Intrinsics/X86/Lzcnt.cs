namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Lzcnt
    {
        public static partial bool IsSupported { get; }

        public static partial uint LeadingZeroCount(uint value);
    }
}
