﻿using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static partial class ArrayHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsNullOrEmpty<T>([InlineParameter] T[] array) => array is null || array.Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNullItem<T>(T[] array) where T : class
        {
            int length = array.Length;
#pragma warning disable CS8500
            fixed (T* ptr = array)
                return SequenceHelper.Contains((nint*)ptr, (nint*)ptr + length, 0);
#pragma warning restore CS8500
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool HasNonNullItem<T>(T[] array) where T : class
            => HasNonNullItem(array, array.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasNonNullItem<T>(T[] array, int length) where T : class
        {
#pragma warning disable CS8500
            fixed (T* ptr = array)
                return SequenceHelper.ContainsExclude((nint*)ptr, (nint*)ptr + length, 0);
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNullItem<T>(T[] array) where T : class
        {
            int length = array.Length;
#pragma warning disable CS8500
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOf((nint*)ptr, (nint*)ptr + length, 0);
                return index == -1 ? null : array[index];
            }
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? FindFirstNonNullItem<T>(T[] array) where T : class
        {
            int length = array.Length;
#pragma warning disable CS8500
            fixed (T* ptr = array)
            {
                int index = SequenceHelper.IndexOfExclude((nint*)ptr, (nint*)ptr + length, 0);
                return index == -1 ? null : array[index];
            }
#pragma warning restore CS8500
        }
    }
}
