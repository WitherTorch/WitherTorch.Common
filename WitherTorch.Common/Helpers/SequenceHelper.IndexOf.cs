using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        #region IndexOf
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfCore(ptr, ptrEnd, value, 0);
        #endregion

        #region IndexOfExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfExcludeCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfExcludeCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfExcludeCore(ptr, ptrEnd, value, 0);
        #endregion

        #region IndexOfGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfGreaterThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfGreaterThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfGreaterThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region IndexOfGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfGreaterOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region IndexOfLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfLessThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfLessThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfLessThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region IndexOfLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return IndexOfLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => IndexOfLessOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region Core Methods
        [Inline(InlineBehavior.Remove)]
        private static int IndexOfCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOf(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }

        [Inline(InlineBehavior.Remove)]
        private static int IndexOfExcludeCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOfExclude(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }

        [Inline(InlineBehavior.Remove)]
        private static int IndexOfGreaterThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOfGreaterThan(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }

        [Inline(InlineBehavior.Remove)]
        private static int IndexOfGreaterOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOfGreaterOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }

        [Inline(InlineBehavior.Remove)]
        private static int IndexOfLessThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOfLessThan(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }

        [Inline(InlineBehavior.Remove)]
        private static int IndexOfLessOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            T* result = PointerIndexOfLessOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
            return result == null ? -1 : unchecked((int)(result - ptr));
        }
        #endregion
    }
}
