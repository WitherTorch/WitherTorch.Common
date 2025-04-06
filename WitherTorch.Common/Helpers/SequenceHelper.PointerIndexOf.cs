using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOf<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOf((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOf((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOf((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOf((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOf(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOf((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOf((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOf((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOf((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOf((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOf((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOf((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOf((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOf((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOf((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOf(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOf(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfExclude<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOfExclude((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfExclude((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfExclude((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfExclude((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOfExclude(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOfExclude((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOfExclude((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOfExclude((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOfExclude((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOfExclude((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOfExclude((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOfExclude((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOfExclude((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOfExclude((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOfExclude((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOfExclude(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOfExclude(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOfGreaterThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfGreaterThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfGreaterThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOfGreaterThan(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOfGreaterThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOfGreaterThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOfGreaterThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOfGreaterThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOfGreaterThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOfGreaterThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOfGreaterThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOfGreaterThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOfGreaterThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOfGreaterOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfGreaterOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfGreaterOrEqualsThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfGreaterOrEqualsThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOfGreaterOrEqualsThan(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOfGreaterOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOfGreaterOrEqualsThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOfGreaterOrEqualsThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOfGreaterOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOfGreaterOrEqualsThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOfGreaterOrEqualsThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOfGreaterOrEqualsThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOfGreaterOrEqualsThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOfLessThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfLessThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfLessThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfLessThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOfLessThan(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOfLessThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOfLessThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOfLessThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOfLessThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOfLessThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOfLessThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOfLessThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOfLessThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOfLessThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOfLessThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOfLessThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return (T*)Core<byte>.PointerIndexOfLessOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfLessOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfLessOrEqualsThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfLessOrEqualsThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.PointerIndexOfLessOrEqualsThan(ptr, ptrEnd, value);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)Core<byte>.PointerIndexOfLessOrEqualsThan((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)Core<sbyte>.PointerIndexOfLessOrEqualsThan((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)Core<short>.PointerIndexOfLessOrEqualsThan((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)Core<ushort>.PointerIndexOfLessOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)Core<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)Core<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)Core<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)Core<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)Core<float>.PointerIndexOfLessOrEqualsThan((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)Core<double>.PointerIndexOfLessOrEqualsThan((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value)),
                    _ => Core<T>.PointerIndexOfLessOrEqualsThan(ptr, ptrEnd, value)
                };
            }
            return Core<T>.PointerIndexOfLessOrEqualsThan(ptr, ptrEnd, value);
        }
    }
}
