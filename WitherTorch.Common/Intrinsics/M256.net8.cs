#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using WitherTorch.Common.Helpers;

#pragma warning disable CS8500

namespace WitherTorch.Common.Intrinsics
{
    partial class M256
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M256<T> AsM256<T>(in this Vector256<T> vector)
            => ref UnsafeHelper.As<Vector256<T>, M256<T>>(ref UnsafeHelper.AsRefIn(in vector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly Vector256<T> AsVector256<T>(in this M256<T> _this)
            => ref UnsafeHelper.As<M256<T>, Vector256<T>>(ref UnsafeHelper.AsRefIn(in _this));
    }
}
#endif
