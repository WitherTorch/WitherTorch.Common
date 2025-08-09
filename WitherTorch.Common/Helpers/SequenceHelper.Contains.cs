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

        #region ContainsGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsGreaterOrEqualsThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan<T>(T* ptr, nuint length, T value) => ContainsGreaterOrEqualsThanCore(ptr, length, value);
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

        #region ContainsLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(str.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value, int startIndex)
        {
            int length = str.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(startIndex >= str.Length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                return ContainsLessOrEqualsThanCore(ptr + startIndex, unchecked((nuint)length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan<T>(T* ptr, nuint length, T value) => ContainsLessOrEqualsThanCore(ptr, length, value);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.Contains((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Contains((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.Contains((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.Contains((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.Contains((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.Contains((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.Contains((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.Contains((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Contains((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Contains((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.Contains((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.Contains((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsCoreSlow(ptr, length, value);
        }

        private static bool ContainsCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.Contains((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Contains((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Contains((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.Contains((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Contains((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Contains((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Contains((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Contains((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.Contains((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.Contains((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.Contains(ptr, length, value)
                };
            }
            return SlowCore<T>.Contains(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.ContainsExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.ContainsExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.ContainsExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.ContainsExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.ContainsExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.ContainsExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.ContainsExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsExcludeCoreSlow(ptr, length, value);
        }

        private static bool ContainsExcludeCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.ContainsExclude(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsExclude(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.ContainsGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.ContainsGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.ContainsGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.ContainsGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.ContainsGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.ContainsGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsGreaterThanCoreSlow(ptr, length, value);
        }

        private static bool ContainsGreaterThanCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.ContainsGreaterThan(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsGreaterThan(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterOrEqualsThanCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.ContainsGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.ContainsGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.ContainsGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.ContainsGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.ContainsGreaterOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.ContainsGreaterOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsGreaterOrEqualsThanCoreSlow(ptr, length, value);
        }

        private static bool ContainsGreaterOrEqualsThanCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.ContainsGreaterOrEqualsThan(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsGreaterOrEqualsThan(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.ContainsLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.ContainsLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.ContainsLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.ContainsLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.ContainsLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.ContainsLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.ContainsLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsLessThanCoreSlow(ptr, length, value);
        }

        private static bool ContainsLessThanCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.ContainsLessThan(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsLessThan(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessOrEqualsThanCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.ContainsLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.ContainsLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.ContainsLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.ContainsLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.ContainsLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.ContainsLessOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.ContainsLessOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return ContainsLessOrEqualsThanCoreSlow(ptr, length, value);
        }

        private static bool ContainsLessOrEqualsThanCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.ContainsLessOrEqualsThan(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsLessOrEqualsThan(ptr, length, value);
        }
        #endregion
    }
}
