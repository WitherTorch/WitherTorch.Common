#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        public static partial nuint CountOf<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOf_1Byte(ptr, length, value),
                2 => CountOf_2Bytes(ptr, length, value),
                4 => CountOf_4Bytes(ptr, length, value),
                8 => CountOf_8Bytes(ptr, length, value),
                _ => CountOfSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOf_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOf((byte*)ptr, length, default)
                    : FastCore<byte>.CountOfExclude((byte*)ptr, length, default);
            }
            return CountOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOf_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOf_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOf_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOf(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOf(ptr, length, value);
        }

        public static partial nuint CountOfExclude<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOfExclude_1Byte(ptr, length, value),
                2 => CountOfExclude_2Bytes(ptr, length, value),
                4 => CountOfExclude_4Bytes(ptr, length, value),
                8 => CountOfExclude_8Bytes(ptr, length, value),
                _ => CountOfExcludeSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfExclude_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOfExclude((byte*)ptr, length, default)
                    : FastCore<byte>.CountOf((byte*)ptr, length, default);
            }
            return CountOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfExclude_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfExclude_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfExclude_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfExcludeSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOfExclude(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOfExclude(ptr, length, value);
        }

        public static partial nuint CountOfGreaterThan<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOfGreaterThan_1Byte(ptr, length, value),
                2 => CountOfGreaterThan_2Bytes(ptr, length, value),
                4 => CountOfGreaterThan_4Bytes(ptr, length, value),
                8 => CountOfGreaterThan_8Bytes(ptr, length, value),
                _ => CountOfGreaterThanSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThan_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOfExclude((byte*)ptr, length, default)
                    : 0;
            }
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThan_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThan_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThan_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfGreaterThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOfGreaterThan(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOfGreaterThan(ptr, length, value);
        }

        public static partial nuint CountOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOfGreaterThanOrEquals_1Byte(ptr, length, value),
                2 => CountOfGreaterThanOrEquals_2Bytes(ptr, length, value),
                4 => CountOfGreaterThanOrEquals_4Bytes(ptr, length, value),
                8 => CountOfGreaterThanOrEquals_8Bytes(ptr, length, value),
                _ => CountOfGreaterThanOrEqualsSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThanOrEquals_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? length
                    : FastCore<byte>.CountOfExclude((byte*)ptr, length, default);
            }
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThanOrEquals_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThanOrEquals_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfGreaterThanOrEquals_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfGreaterThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOfGreaterThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOfGreaterThanOrEquals(ptr, length, value);
        }

        public static partial nuint CountOfLessThan<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOfLessThan_1Byte(ptr, length, value),
                2 => CountOfLessThan_2Bytes(ptr, length, value),
                4 => CountOfLessThan_4Bytes(ptr, length, value),
                8 => CountOfLessThan_8Bytes(ptr, length, value),
                _ => CountOfLessThanSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThan_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? 0
                    : FastCore<byte>.CountOf((byte*)ptr, length, default);
            }
            return CountOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThan_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThan_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThan_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfLessThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOfLessThan(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOfLessThan(ptr, length, value);
        }

        public static partial nuint CountOfLessThanOrEquals<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => CountOfLessThanOrEquals_1Byte(ptr, length, value),
                2 => CountOfLessThanOrEquals_2Bytes(ptr, length, value),
                4 => CountOfLessThanOrEquals_4Bytes(ptr, length, value),
                8 => CountOfLessThanOrEquals_8Bytes(ptr, length, value),
                _ => CountOfLessThanOrEqualsSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThanOrEquals_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? FastCore<byte>.CountOf((byte*)ptr, length, default)
                    : length;
            }
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThanOrEquals_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.CountOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThanOrEquals_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint CountOfLessThanOrEquals_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static nuint CountOfLessThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.CountOfLessThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.CountOfLessThanOrEquals(ptr, length, value);
        }
    }
}
#endif