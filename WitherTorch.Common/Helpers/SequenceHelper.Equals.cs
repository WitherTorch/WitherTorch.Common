using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string str, string str2)
        {
            if (ReferenceEquals(str, str2))
                return true;
            int length = str.Length;
            if (length != str2.Length)
                return false;
            fixed (char* ptr = str, ptr2 = str2)
                return EqualsCore(ptr, ptr + length, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string str, string str2, StringComparison comparison)
        {
            if (ReferenceEquals(str, str2))
                return true;
            int length = str.Length;
            if (length != str2.Length)
                return false;
            if (comparison == StringComparison.Ordinal)
            {
                fixed (char* ptr = str, ptr2 = str2)
                    return EqualsCore(ptr, ptr + length, ptr2);
            }
            return str.Equals(str2, comparison);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T[] array1, T[] array2) where T : unmanaged
        {
            if (ReferenceEquals(array1, array2))
                return true;
            int length = array1.Length;
            if (length != array2.Length)
                return false;
            fixed (T* ptr = array1, ptr2 = array2)
                return EqualsCore(ptr, ptr + length, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T* ptr, T* ptrEnd, T* ptr2) where T : unmanaged
        {
            if (ptr == ptr2)
                return true;
            return EqualsCore(ptr, ptrEnd, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsCore<T>(T* ptr, T* ptrEnd, T* ptr2) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.Equals((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2);
            if (typeof(T) == typeof(char))
                return Core<ushort>.Equals((ushort*)ptr, (ushort*)ptrEnd, (ushort*)ptr2);
            if (typeof(T) == typeof(nint))
                return Core.Equals((IntPtr*)ptr, (IntPtr*)ptrEnd, (IntPtr*)ptr2);
            if (typeof(T) == typeof(nuint))
                return Core.Equals((UIntPtr*)ptr, (UIntPtr*)ptrEnd, (UIntPtr*)ptr2);
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.Equals(ptr, ptrEnd, ptr2);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.Equals((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2),
                    TypeCode.SByte => Core<sbyte>.Equals((sbyte*)ptr, (sbyte*)ptrEnd, (sbyte*)ptr2),
                    TypeCode.Int16 => Core<short>.Equals((short*)ptr, (short*)ptrEnd, (short*)ptr2),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.Equals((ushort*)ptr, (ushort*)ptrEnd, (ushort*)ptr2),
                    TypeCode.Int32 => Core<int>.Equals((int*)ptr, (int*)ptrEnd, (int*)ptr2),
                    TypeCode.UInt32 => Core<uint>.Equals((uint*)ptr, (uint*)ptrEnd, (uint*)ptr2),
                    TypeCode.Int64 => Core<long>.Equals((long*)ptr, (long*)ptrEnd, (long*)ptr2),
                    TypeCode.UInt64 => Core<ulong>.Equals((ulong*)ptr, (ulong*)ptrEnd, (ulong*)ptr2),
                    TypeCode.Single => Core<float>.Equals((float*)ptr, (float*)ptrEnd, (float*)ptr2),
                    TypeCode.Double => Core<double>.Equals((double*)ptr, (double*)ptrEnd, (double*)ptr2),
                    _ => Core<T>.Equals(ptr, ptrEnd, ptr2)
                };
            }
            return Core<T>.Equals(ptr, ptrEnd, ptr2);
        }
    }
}
