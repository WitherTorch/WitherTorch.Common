using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        #region Contains
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(char* ptr, char* ptrEnd, char value)
            => ContainsCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsExcludeCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(char* ptr, char* ptrEnd, char value)
            => ContainsExcludeCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsExcludeCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(char* ptr, char* ptrEnd, char value)
            => ContainsGreaterThanCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsGreaterThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(char* ptr, char* ptrEnd, char value)
            => ContainsGreaterOrEqualsThanCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsGreaterOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsLessThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(char* ptr, char* ptrEnd, char value)
            => ContainsLessThanCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsLessThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr, ptr + UnsafeStringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(char[] array, char value)
        {
            fixed (char* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(char[] array, char value, int startIndex)
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(char[] array, char value, int startIndex, int count)
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + array.Length, value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, ptr + length, value, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(char* ptr, char* ptrEnd, char value)
            => ContainsLessOrEqualsThanCore(ptr, ptrEnd, value, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsLessOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region Core Methods
        [Inline(InlineBehavior.Remove)]
        private static bool ContainsCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.Contains(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.Contains(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.Contains(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.Contains(WithOffset(ptr, offset), ptrEnd, value);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ContainsExcludeCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsExclude(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsExclude(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsExclude(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.ContainsExclude(WithOffset(ptr, offset), ptrEnd, value);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ContainsGreaterThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsGreaterThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsGreaterThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsGreaterThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.ContainsGreaterThan(WithOffset(ptr, offset), ptrEnd, value);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ContainsGreaterOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsGreaterOrEqualsThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsGreaterOrEqualsThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsGreaterOrEqualsThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.ContainsGreaterOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ContainsLessThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsLessThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsLessThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsLessThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.ContainsLessThan(WithOffset(ptr, offset), ptrEnd, value);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ContainsLessOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsLessOrEqualsThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsLessOrEqualsThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsLessOrEqualsThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.ContainsLessOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
        }
        #endregion
    }
}
