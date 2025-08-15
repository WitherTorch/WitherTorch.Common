#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimeBmi1X64 = System.Runtime.Intrinsics.X86.Bmi1.X64;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        partial class X64
        {
            public static partial bool IsSupported
            {
                [Inline(InlineBehavior.Keep, export: true)]
                get => RuntimeBmi1X64.IsSupported;
            }

            [Inline(InlineBehavior.Keep, export: true)]
            public static partial ulong TrailingZeroCount(ulong value) => RuntimeBmi1X64.TrailingZeroCount(value);
        }
    }
}
#endif