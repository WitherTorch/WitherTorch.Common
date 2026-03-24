using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static class ReadOnlyViewExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsMemory<T>(this ReadOnlyView<T> _this) where T : unmanaged
        {
            object? source = _this._source;
            if (source is not null)
            {
                if (source is T[] array)
                    return new ReadOnlyMemory<T>(array, _this._startIndex, _this._count);
                if (typeof(T) == typeof(char) && source is string str)
                    return UnsafeHelper.As<ReadOnlyMemory<char>, ReadOnlyMemory<T>>(str.AsMemory(_this._startIndex, _this._count));
            }
            return ReadOnlyMemory<T>.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsSpan<T>(this ReadOnlyView<T> _this) where T : unmanaged
        {
            object? source = _this._source;
            if (source is not null)
            {
                if (source is T[] array)
                    return new ReadOnlySpan<T>(array, _this._startIndex, _this._count);
                if (typeof(T) == typeof(char) && source is string str)
                    return MemoryMarshal.Cast<char, T>(str.AsSpan(_this._startIndex, _this._count));
            }
            return ReadOnlySpan<T>.Empty;
        }
    }
}
