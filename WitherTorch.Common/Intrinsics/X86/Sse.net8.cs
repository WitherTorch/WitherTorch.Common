#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using InlineMethod;

using RuntimeSse = System.Runtime.Intrinsics.X86.Sse;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Sse
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimeSse.IsSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<float> value)
            => RuntimeSse.MoveMask(value.AsVector128());
    }
}
#endif