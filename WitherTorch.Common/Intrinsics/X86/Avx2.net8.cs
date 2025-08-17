#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;

using InlineMethod;

using RuntimeAvx2 = System.Runtime.Intrinsics.X86.Avx2;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Avx2
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimeAvx2.IsSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<byte> value)
            => RuntimeAvx2.MoveMask(value.AsVector256());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<sbyte> value)
            => RuntimeAvx2.MoveMask(value.AsVector256());
    }
}
#endif