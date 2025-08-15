using InlineMethod;

namespace WitherTorch.Common.Intrinsics.X86
{
    public static partial class X86Base
    {
        public static partial bool IsSupported { get; }

        [Inline(InlineBehavior.Keep, export: true)]
        public static (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId)
            => CpuId(functionId, subFunctionId: 0);

        public static partial (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId);

        public static partial uint BitScanForward(uint value);

        public static partial uint BitScanReverse(uint value);

        public static partial (int Quotient, int Remainder) DivRem(long dividend, int divisor);

        public static partial (uint Quotient, uint Remainder) DivRem(ulong dividend, uint divisor);

        public static partial (int Quotient, int Remainder) DivRem(uint lower, int upper, int divisor);

        public static partial (uint Quotient, uint Remainder) DivRem(uint lower, uint upper, uint divisor);

        public static partial (nint Quotient, nint Remainder) DivRem(nuint lower, nint upper, nint divisor);

        public static partial (nuint Quotient, nuint Remainder) DivRem(nuint lower, nuint upper, nuint divisor);
    }
}
