using System;
using System.Runtime.CompilerServices;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    public static partial class ArrayHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsNullOrEmpty<T>([InlineParameter] T?[] array) => array is null || array.Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNullItem<T>(T[] array) where T : class
        {
            fixed (T* ptr = array)
                return SequenceHelper.Contains((nint*)ptr, MathHelper.MakeUnsigned(array.Length), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNullItem<T>(T[] array, int startIndex, int count) where T : class
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return SequenceHelper.Contains((nint*)ptr + startIndex, unchecked((nuint)count), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNonNullItem<T>(T?[] array) where T : class
        {
            fixed (T* ptr = array)
                return SequenceHelper.ContainsExclude((nint*)ptr, MathHelper.MakeUnsigned(array.Length), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNonNullItem<T>(T?[] array, int startIndex, int count) where T : class
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return SequenceHelper.ContainsExclude((nint*)ptr + startIndex, unchecked((nuint)count), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNullItem<T>(T?[] array) where T : class
        {
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOf((nint*)ptr, array.Length, 0);
                return index == -1 ? null : array[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNullItem<T>(T?[] array, int startIndex, int count) where T : class
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOf((nint*)ptr + startIndex, count, 0);
                return index == -1 ? null : array[startIndex + index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNonNullItem<T>(T?[] array) where T : class
        {
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOfExclude((nint*)ptr, array.Length, 0);
                return index == -1 ? null : array[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNonNullItem<T>(T?[] array, int startIndex, int count) where T : class
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOfExclude((nint*)ptr + startIndex, count, 0);
                return index == -1 ? null : array[startIndex + index];
            }
        }
    }
}
