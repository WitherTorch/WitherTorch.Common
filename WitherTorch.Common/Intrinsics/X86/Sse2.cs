using System.Numerics;

namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class Sse2
    {
        public static partial bool IsSupported { get; }

        public static partial int MoveMask(in M128<double> value);

        public static partial int MoveMask(in M128<byte> value);

        public static partial int MoveMask(in M128<sbyte> value);
    }
}
