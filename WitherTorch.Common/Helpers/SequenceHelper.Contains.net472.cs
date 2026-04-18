#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        private static partial bool ContainsCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsCore_1Byte(ptr, length, value),
                2 => ContainsCore_2Bytes(ptr, length, value),
                4 => ContainsCore_4Bytes(ptr, length, value),
                8 => ContainsCore_8Bytes(ptr, length, value),
                _ => ContainsCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Contains((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.Contains((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Contains((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Contains((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Contains((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Contains((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Contains((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Contains((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Contains((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Contains((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsCoreSlow<T>(T* ptr, T value, nuint length)
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

        private static partial bool ContainsExcludeCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsExcludeCore_1Byte(ptr, length, value),
                2 => ContainsExcludeCore_2Bytes(ptr, length, value),
                4 => ContainsExcludeCore_4Bytes(ptr, length, value),
                8 => ContainsExcludeCore_8Bytes(ptr, length, value),
                _ => ContainsExcludeCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.ContainsExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsExcludeCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.ContainsExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsExcludeCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.ContainsExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.ContainsExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsExcludeCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsExcludeCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.ContainsExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsExcludeCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsExcludeCoreSlow<T>(T* ptr, T value, nuint length)
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

        private static partial bool ContainsGreaterThanCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsGreaterThanCore_1Byte(ptr, length, value),
                2 => ContainsGreaterThanCore_2Bytes(ptr, length, value),
                4 => ContainsGreaterThanCore_4Bytes(ptr, length, value),
                8 => ContainsGreaterThanCore_8Bytes(ptr, length, value),
                _ => ContainsGreaterThanCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.ContainsGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsGreaterThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.ContainsGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsGreaterThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.ContainsGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsGreaterThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.ContainsGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsGreaterThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsGreaterThanCoreSlow<T>(T* ptr, T value, nuint length)
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

        private static partial bool ContainsGreaterThanOrEqualsCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsGreaterThanOrEqualsCore_1Byte(ptr, length, value),
                2 => ContainsGreaterThanOrEqualsCore_2Bytes(ptr, length, value),
                4 => ContainsGreaterThanOrEqualsCore_4Bytes(ptr, length, value),
                8 => ContainsGreaterThanOrEqualsCore_8Bytes(ptr, length, value),
                _ => ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanOrEqualsCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.ContainsGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanOrEqualsCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.ContainsGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanOrEqualsCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsGreaterThanOrEqualsCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsGreaterThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsGreaterThanOrEqualsCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.ContainsGreaterThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsGreaterThanOrEquals(ptr, length, value);
        }

        private static partial bool ContainsLessThanCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsLessThanCore_1Byte(ptr, length, value),
                2 => ContainsLessThanCore_2Bytes(ptr, length, value),
                4 => ContainsLessThanCore_4Bytes(ptr, length, value),
                8 => ContainsLessThanCore_8Bytes(ptr, length, value),
                _ => ContainsLessThanCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.ContainsLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsLessThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.ContainsLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsLessThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.ContainsLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.ContainsLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsLessThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.ContainsLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsLessThanCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsLessThanCoreSlow<T>(T* ptr, T value, nuint length)
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

        private static partial bool ContainsLessThanOrEqualsCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => ContainsLessThanOrEqualsCore_1Byte(ptr, length, value),
                2 => ContainsLessThanOrEqualsCore_2Bytes(ptr, length, value),
                4 => ContainsLessThanOrEqualsCore_4Bytes(ptr, length, value),
                8 => ContainsLessThanOrEqualsCore_8Bytes(ptr, length, value),
                _ => ContainsLessThanOrEqualsCoreSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanOrEqualsCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.ContainsLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(bool))
                return FastCore<byte>.ContainsLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            return ContainsLessThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanOrEqualsCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.ContainsLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.ContainsLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return ContainsLessThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanOrEqualsCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.ContainsLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return ContainsLessThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsLessThanOrEqualsCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.ContainsLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return ContainsLessThanOrEqualsCoreSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ContainsLessThanOrEqualsCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.ContainsLessThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.ContainsLessThanOrEquals(ptr, length, value);
        }
    }
}
#endif