#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;

using InlineMethod;

using RuntimeAvx = System.Runtime.Intrinsics.X86.Avx;
using RuntimeSse = System.Runtime.Intrinsics.X86.Sse;
using RuntimeSse2 = System.Runtime.Intrinsics.X86.Sse2;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Avx
    {
        public static partial bool IsSupported
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => RuntimeAvx.IsSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<float> value) 
            => RuntimeSse.MoveMask(value.AsVector128());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<float> value) 
            => RuntimeAvx.MoveMask(value.AsVector256());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<double> value)
            => RuntimeSse2.MoveMask(value.AsVector128());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<double> value)
            => RuntimeAvx.MoveMask(value.AsVector256());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<byte> value)
            => RuntimeSse2.MoveMask(value.AsVector128());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<sbyte> value)
            => RuntimeSse2.MoveMask(value.AsVector128());
    }
}
#endif