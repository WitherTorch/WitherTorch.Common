#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        public static partial nuint CountOf<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOf((byte*)ptr, length, default)
                    : FastCore<byte>.CountOfExclude((byte*)ptr, length, default);
            }
            if (type == typeof(short))
                return FastCore<short>.CountOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOf((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOf((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOf((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOf((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOf((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOf((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOf((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfSlow(ptr, value, length)
                };
            }
            return CountOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOf(ptr, length, value);

        public static partial nuint CountOfExclude<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOfExclude((byte*)ptr, length, default)
                    : FastCore<byte>.CountOf((byte*)ptr, length, default);
            }
            if (type == typeof(short))
                return FastCore<short>.CountOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOfExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOfExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfExcludeSlow(ptr, value, length)
                };
            }
            return CountOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfExcludeSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOfExclude(ptr, length, value);

        public static partial nuint CountOfGreaterThan<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOfExclude((byte*)ptr, length, default)
                    : 0;
            }
            if (type == typeof(short))
                return FastCore<short>.CountOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOfGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOfGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfGreaterThanSlow(ptr, value, length)
                };
            }
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfGreaterThanSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOfGreaterThan(ptr, length, value);

        public static partial nuint CountOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? length
                    : FastCore<byte>.CountOfExclude((byte*)ptr, length, default);
            }
            if (type == typeof(short))
                return FastCore<short>.CountOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOfGreaterThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOfGreaterThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfGreaterThanOrEqualsSlow(ptr, value, length)
                };
            }
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfGreaterThanOrEqualsSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOfGreaterThanOrEquals(ptr, length, value);

        public static partial nuint CountOfLessThan<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? 0
                    : FastCore<byte>.CountOf((byte*)ptr, length, default);
            }
            if (type == typeof(short))
                return FastCore<short>.CountOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOfLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOfLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfLessThanSlow(ptr, value, length)
                };
            }
            return CountOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfLessThanSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOfLessThan(ptr, length, value);

        public static partial nuint CountOfLessThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.CountOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOf((byte*)ptr, length, default)
                    : length;
            }
            if (type == typeof(short))
                return FastCore<short>.CountOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.CountOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.CountOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.CountOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.CountOfLessThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.CountOfLessThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.CountOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.CountOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.CountOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.CountOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => CountOfLessThanOrEqualsSlow(ptr, value, length)
                };
            }
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfLessThanOrEqualsSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.CountOfLessThanOrEquals(ptr, length, value);
    }
}
#endif