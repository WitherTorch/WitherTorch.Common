#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using WitherTorch.Common.Helpers;

#pragma warning disable CS8500

namespace WitherTorch.Common.Intrinsics
{
    partial class M512
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M512<T> AsM512<T>(in this Vector512<T> vector)
            => ref UnsafeHelper.As<Vector512<T>, M512<T>>(ref UnsafeHelper.AsRefIn(in vector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly Vector512<T> AsVector512<T>(in this M512<T> _this)
            => ref UnsafeHelper.As<M512<T>, Vector512<T>>(ref UnsafeHelper.AsRefIn(in _this));
    }
}
#endif
