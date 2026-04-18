#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        public static partial T* PointerIndexOf<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOf((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOf((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOf((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOf((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOf((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOf((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOf((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfSlow(ptr, value, length)
                };
            }
            return PointerIndexOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOf(ptr, length, value);

        public static partial T* PointerIndexOfExclude<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOfExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOfExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfExcludeSlow(ptr, value, length)
                };
            }
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfExcludeSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOfExclude(ptr, length, value);

        public static partial T* PointerIndexOfGreaterThan<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOfGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOfGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfGreaterThanSlow(ptr, value, length)
                };
            }
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfGreaterThanSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOfGreaterThan(ptr, length, value);

        public static partial T* PointerIndexOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOfGreaterThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOfGreaterThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length)
                };
            }
            return PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfGreaterThanOrEqualsSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOfGreaterThanOrEquals(ptr, length, value);

        public static partial T* PointerIndexOfLessThan<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOfLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOfLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfLessThanSlow(ptr, value, length)
                };
            }
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfLessThanSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOfLessThan(ptr, length, value);

        public static partial T* PointerIndexOfLessThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return (T*)FastCore<byte>.PointerIndexOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return (T*)FastCore<nint>.PointerIndexOfLessThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return (T*)FastCore<nuint>.PointerIndexOfLessThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => PointerIndexOfLessThanOrEqualsSlow(ptr, value, length)
                };
            }
            return PointerIndexOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfLessThanOrEqualsSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.PointerIndexOfLessThanOrEquals(ptr, length, value);
    }
}
#endif