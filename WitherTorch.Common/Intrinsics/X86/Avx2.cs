namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Avx2
    {
        public static partial bool IsSupported { get; }

        public static partial int MoveMask(in M256<byte> value);

        public static partial int MoveMask(in M256<sbyte> value);
    }
}
