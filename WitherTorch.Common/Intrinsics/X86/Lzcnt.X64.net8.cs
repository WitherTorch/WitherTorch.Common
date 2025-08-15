#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimeLzcntX64 = System.Runtime.Intrinsics.X86.Lzcnt.X64;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Lzcnt
    {
        partial class X64
        {
            public static partial bool IsSupported
            {
                [Inline(InlineBehavior.Keep, export: true)]
                get => RuntimeLzcntX64.IsSupported;
            }

            [Inline(InlineBehavior.Keep, export: true)]
            public static partial ulong LeadingZeroCount(ulong value) => RuntimeLzcntX64.LeadingZeroCount(value);
        }
    }
}
#endif