#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimeLzcnt = System.Runtime.Intrinsics.X86.Lzcnt;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Lzcnt
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimeLzcnt.IsSupported;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint LeadingZeroCount(uint value) => RuntimeLzcnt.LeadingZeroCount(value);
    }
}
#endif