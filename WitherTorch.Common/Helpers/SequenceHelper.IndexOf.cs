using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        #pragma warning disable CS8500

        #region IndexOf
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOf(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOf(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOf(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOf(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOf(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOf(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOf(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOf<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOf(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion

        #region IndexOfExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfExclude(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfExclude<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOfExclude<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOfExclude(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion

        #region IndexOfGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterThan<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOfGreaterThan<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOfGreaterThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion

        #region IndexOfGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfGreaterOrEqualsThan<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOfGreaterOrEqualsThan<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOfGreaterOrEqualsThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion

        #region IndexOfLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessThan<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOfLessThan<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOfLessThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion

        #region IndexOfLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(str.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value) 
        {
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(array.Length), value), ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(length - startIndex), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T[] array, T value, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, unchecked((uint)count), value), ptr, startIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLessOrEqualsThan<T>(T* ptr, int length, T value) 
            => ConvertToIndex32(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint IndexOfLessOrEqualsThan<T>(T* ptr, nint length, T value)
            => ConvertToIndexNative(PointerIndexOfLessOrEqualsThan(ptr, MathHelper.MakeUnsigned(length), value), ptr);
        #endregion
    }
}
