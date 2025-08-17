namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Avx
    {
        public static partial bool IsSupported { get; }

        public static partial int MoveMask(in M128<float> value);

        public static partial int MoveMask(in M256<float> value);

        public static partial int MoveMask(in M128<double> value);

        public static partial int MoveMask(in M256<double> value);

        public static partial int MoveMask(in M128<byte> value);

        public static partial int MoveMask(in M128<sbyte> value);
    }
}
