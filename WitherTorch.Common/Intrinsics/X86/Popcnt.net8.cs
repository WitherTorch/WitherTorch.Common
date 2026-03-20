#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimePopcnt = System.Runtime.Intrinsics.X86.Popcnt;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Popcnt
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimePopcnt.IsSupported;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint PopCount(uint value) => RuntimePopcnt.PopCount(value);
    }
}
#endif