#if NET8_0_OR_GREATER
using InlineMethod;

using RuntimePopcntX64 = System.Runtime.Intrinsics.X86.Popcnt.X64;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Popcnt
    {
        partial class X64
        {
            public static partial bool IsSupported
            {
                [Inline(InlineBehavior.Keep, export: true)]
                get => RuntimePopcntX64.IsSupported;
            }

            [Inline(InlineBehavior.Keep, export: true)]
            public static partial ulong PopCount(ulong value) => RuntimePopcntX64.PopCount(value);
        }
    }
}
#endif