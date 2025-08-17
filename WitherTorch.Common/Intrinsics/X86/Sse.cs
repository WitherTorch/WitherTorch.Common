namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Sse
    {
        public static partial bool IsSupported { get; }

        public static partial int MoveMask(in M128<float> value);
    }
}
