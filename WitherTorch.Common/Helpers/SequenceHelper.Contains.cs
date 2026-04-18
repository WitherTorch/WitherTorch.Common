using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        #region Contains
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(T* ptr, nuint length, T value) => ContainsCore(ptr, length, value);
        #endregion

        #region ContainsExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsExcludeCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude<T>(T* ptr, nuint length, T value) => ContainsExcludeCore(ptr, length, value);
        #endregion

        #region ContainsGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsGreaterThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan<T>(T* ptr, nuint length, T value) => ContainsGreaterThanCore(ptr, length, value);
        #endregion

        #region ContainsGreaterThanOrEquals
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterThanOrEqualsCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsGreaterThanOrEqualsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsGreaterThanOrEqualsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsGreaterThanOrEqualsCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsGreaterThanOrEqualsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsGreaterThanOrEqualsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThanOrEquals<T>(T* ptr, nuint length, T value) => ContainsGreaterThanOrEqualsCore(ptr, length, value);
        #endregion

        #region ContainsLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsLessThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan<T>(T* ptr, nuint length, T value) => ContainsLessThanCore(ptr, length, value);
        #endregion

        #region ContainsLessThanOrEquals
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessThanOrEqualsCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsLessThanOrEqualsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsLessThanOrEqualsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsLessThanOrEqualsCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsLessThanOrEqualsCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsLessThanOrEqualsCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThanOrEquals<T>(T* ptr, nuint length, T value) => ContainsLessThanOrEqualsCore(ptr, length, value);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsExcludeCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsGreaterThanCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsGreaterThanOrEqualsCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsLessThanCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial bool ContainsLessThanOrEqualsCore<T>(T* ptr, nuint length, T value);
        #endregion
    }
}
