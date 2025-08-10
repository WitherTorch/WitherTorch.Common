using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class HolderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsMemory<T>(this IHolder<T[]> _this) where T : unmanaged
            => _this.Value.AsMemory();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsMemory<T>(this IHolder<ArraySegment<T>> _this) where T : unmanaged
            => _this.Value.AsMemory();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<char> AsMemory(this IHolder<string> _this)
            => _this.Value.AsMemory();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsSpan<T>(this IHolder<T[]> _this) where T : unmanaged
            => _this.Value.AsSpan();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsSpan<T>(this IHolder<ArraySegment<T>> _this) where T : unmanaged
            => _this.Value.AsSpan();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> AsSpan(this IHolder<string> _this)
            => _this.Value.AsSpan();
    }
}
