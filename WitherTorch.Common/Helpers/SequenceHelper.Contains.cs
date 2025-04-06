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
                return ContainsCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool Contains<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsExclude(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsExcludeCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool ContainsExclude<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsExcludeCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterThanCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool ContainsGreaterThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsGreaterThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGreaterOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsGreaterOrEqualsThanCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool ContainsGreaterOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsGreaterOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessThanCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool ContainsLessThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsLessThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region ContainsLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsLessOrEqualsThan(string str, char value)
        {
            fixed (char* ptr = str)
                return ContainsLessOrEqualsThanCore(ptr, ptr + StringHelper.GetStringLengthByHeadPointer(ptr), value, 0);
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
        public static bool ContainsLessOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => ContainsLessOrEqualsThanCore(ptr, ptrEnd, value, 0);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.Contains(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.Contains(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.Contains(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.Contains(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.Contains(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.Contains((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.Contains((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.Contains((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.Contains((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.Contains((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.Contains((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.Contains((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.Contains((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.Contains((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.Contains((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.Contains(ptr, ptrEnd, value)
                };
            }
            return Core<T>.Contains(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.ContainsExclude(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsExclude(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsExclude(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsExclude(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.ContainsExclude(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.ContainsExclude((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.ContainsExclude((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.ContainsExclude((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.ContainsExclude((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.ContainsExclude((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.ContainsExclude((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.ContainsExclude((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.ContainsExclude((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.ContainsExclude((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.ContainsExclude((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.ContainsExclude(ptr, ptrEnd, value)
                };
            }
            return Core<T>.ContainsExclude(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.ContainsGreaterThan(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsGreaterThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsGreaterThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsGreaterThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.ContainsGreaterThan(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.ContainsGreaterThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.ContainsGreaterThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.ContainsGreaterThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.ContainsGreaterThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.ContainsGreaterThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.ContainsGreaterThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.ContainsGreaterThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.ContainsGreaterThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.ContainsGreaterThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.ContainsGreaterThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.ContainsGreaterThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.ContainsGreaterThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.ContainsGreaterOrEqualsThan(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsGreaterOrEqualsThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsGreaterOrEqualsThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsGreaterOrEqualsThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.ContainsGreaterOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.ContainsGreaterOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.ContainsGreaterOrEqualsThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.ContainsGreaterOrEqualsThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.ContainsGreaterOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.ContainsGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.ContainsGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.ContainsGreaterOrEqualsThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.ContainsGreaterOrEqualsThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.ContainsGreaterOrEqualsThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.ContainsGreaterOrEqualsThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.ContainsLessThan(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsLessThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsLessThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsLessThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.ContainsLessThan(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.ContainsLessThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.ContainsLessThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.ContainsLessThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.ContainsLessThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.ContainsLessThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.ContainsLessThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.ContainsLessThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.ContainsLessThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.ContainsLessThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.ContainsLessThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.ContainsLessThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.ContainsLessThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessOrEqualsThanCore<T>(T* ptr, T* ptrEnd, T value, [InlineParameter] int offset) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.ContainsLessOrEqualsThan(WithOffset((byte*)ptr, offset), (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return Core<ushort>.ContainsLessOrEqualsThan(WithOffset((ushort*)ptr, offset), (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return Core.ContainsLessOrEqualsThan(WithOffset((IntPtr*)ptr, offset), (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return Core.ContainsLessOrEqualsThan(WithOffset((UIntPtr*)ptr, offset), (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.ContainsLessOrEqualsThan(WithOffset(ptr, offset), ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.ContainsLessOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => Core<sbyte>.ContainsLessOrEqualsThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => Core<short>.ContainsLessOrEqualsThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.ContainsLessOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => Core<int>.ContainsLessOrEqualsThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => Core<uint>.ContainsLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => Core<long>.ContainsLessOrEqualsThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => Core<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => Core<float>.ContainsLessOrEqualsThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => Core<double>.ContainsLessOrEqualsThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.ContainsLessOrEqualsThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.ContainsLessOrEqualsThan(ptr, ptrEnd, value);
        }
        #endregion
    }
}
