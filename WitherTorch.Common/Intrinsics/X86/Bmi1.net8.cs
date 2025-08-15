#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimeBmi1 = System.Runtime.Intrinsics.X86.Bmi1;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimeBmi1.IsSupported;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint TrailingZeroCount(uint value) => RuntimeBmi1.TrailingZeroCount(value);
    }
}
#endif