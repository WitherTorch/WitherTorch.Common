#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using WitherTorch.Common.Helpers;

#pragma warning disable CS8500

namespace WitherTorch.Common.Intrinsics
{
    partial class M128
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M128<T> AsM128<T>(in this Vector128<T> vector)
            => ref UnsafeHelper.As<Vector128<T>, M128<T>>(ref UnsafeHelper.AsRefIn(in vector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly Vector128<T> AsVector128<T>(in this M128<T> _this)
            => ref UnsafeHelper.As<M128<T>, Vector128<T>>(ref UnsafeHelper.AsRefIn(in _this));
    }
}
#endif
