namespace WitherTorch.Common.Intrinsics.X86
{
	partial class X86Base
	{
		public static partial class X64
		{
			public static partial bool IsSupported { get; }

			public static partial uint BitScanForward(ulong value);

			public static partial uint BitScanReverse(ulong value);

			public static partial (long Quotient, long Remainder) DivRem(ulong lower, long upper, long divisor);

			public static partial (ulong Quotient, ulong Remainder) DivRem(ulong lower, ulong upper, ulong divisor);
		}
	}
}
