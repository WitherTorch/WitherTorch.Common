#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        private static partial bool ContainsCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Contains((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.Contains((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Contains((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Contains((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Contains((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Contains((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Contains((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Contains((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.Contains((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Contains((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Contains((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Contains((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

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
                    _ => ContainsCoreSlow(ptr, value, length)
                };
            }
            return ContainsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.Contains(ptr, length, value);

        private static partial bool ContainsExcludeCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.ContainsExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.ContainsExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.ContainsExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.ContainsExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.ContainsExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.ContainsExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.ContainsExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.ContainsExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.ContainsExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.ContainsExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.ContainsExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

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
                    _ => ContainsExcludeCoreSlow(ptr, value, length)
                };
            }
            return ContainsExcludeCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsExcludeCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.ContainsExclude(ptr, length, value);

        private static partial bool ContainsGreaterThanCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.ContainsGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.ContainsGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.ContainsGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.ContainsGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.ContainsGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.ContainsGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.ContainsGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.ContainsGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.ContainsGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

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
                    _ => ContainsGreaterThanCoreSlow(ptr, value, length)
                };
            }
            return ContainsGreaterThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsGreaterThanCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.ContainsGreaterThan(ptr, length, value);

        private static partial bool ContainsGreaterThanOrEqualsCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.ContainsGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.ContainsGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.ContainsGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.ContainsGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.ContainsGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.ContainsGreaterThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.ContainsGreaterThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length)
                };
            }
            return ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsGreaterThanOrEqualsCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.ContainsGreaterThanOrEquals(ptr, length, value);

        private static partial bool ContainsLessThanCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.ContainsLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.ContainsLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.ContainsLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.ContainsLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.ContainsLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.ContainsLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.ContainsLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.ContainsLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.ContainsLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.ContainsLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

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
                    _ => ContainsLessThanCoreSlow(ptr, value, length)
                };
            }
            return ContainsLessThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsLessThanCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.ContainsLessThan(ptr, length, value);

        private static partial bool ContainsLessThanOrEqualsCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte) || type == typeof(bool))
                return FastCore<byte>.ContainsLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.ContainsLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.ContainsLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(float))
                return FastCore<float>.ContainsLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.ContainsLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type == typeof(nint))
                return FastCore<nint>.ContainsLessThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.ContainsLessThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));

            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.ContainsLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.ContainsLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.ContainsLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.ContainsLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => FastCore<float>.ContainsLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => FastCore<double>.ContainsLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => ContainsLessThanOrEqualsCoreSlow(ptr, value, length)
                };
            }
            return ContainsLessThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsLessThanOrEqualsCoreSlow<T>(T* ptr, T value, nuint length) => SlowCore<T>.ContainsLessThanOrEquals(ptr, length, value);
    }
}
#endif