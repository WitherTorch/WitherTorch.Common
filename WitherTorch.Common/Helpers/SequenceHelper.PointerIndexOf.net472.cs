#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        public static partial T* PointerIndexOf<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOf_1Byte(ptr, length, value),
                2 => PointerIndexOf_2Bytes(ptr, length, value),
                4 => PointerIndexOf_4Bytes(ptr, length, value),
                8 => PointerIndexOf_8Bytes(ptr, length, value),
                _ => PointerIndexOfSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOf_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, default)
                    : (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, default);
            }
            return PointerIndexOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOf_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOf_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOf_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOf((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOf((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOf((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOf((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOf((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOf(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOf(ptr, length, value);
        }

        public static partial T* PointerIndexOfExclude<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOfExclude_1Byte(ptr, length, value),
                2 => PointerIndexOfExclude_2Bytes(ptr, length, value),
                4 => PointerIndexOfExclude_4Bytes(ptr, length, value),
                8 => PointerIndexOfExclude_8Bytes(ptr, length, value),
                _ => PointerIndexOfExcludeSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfExclude_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, default)
                    : (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, default);
            }
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfExclude_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfExclude_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfExclude_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfExcludeSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfExclude(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOfExclude(ptr, length, value);
        }

        public static partial T* PointerIndexOfGreaterThan<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOfGreaterThan_1Byte(ptr, length, value),
                2 => PointerIndexOfGreaterThan_2Bytes(ptr, length, value),
                4 => PointerIndexOfGreaterThan_4Bytes(ptr, length, value),
                8 => PointerIndexOfGreaterThan_8Bytes(ptr, length, value),
                _ => PointerIndexOfGreaterThanSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThan_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, default)
                    : null;
            }
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThan_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThan_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThan_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfGreaterThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfGreaterThan(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOfGreaterThan(ptr, length, value);
        }

        public static partial T* PointerIndexOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOfGreaterThanOrEquals_1Byte(ptr, length, value),
                2 => PointerIndexOfGreaterThanOrEquals_2Bytes(ptr, length, value),
                4 => PointerIndexOfGreaterThanOrEquals_4Bytes(ptr, length, value),
                8 => PointerIndexOfGreaterThanOrEquals_8Bytes(ptr, length, value),
                _ => PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThanOrEquals_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? ptr
                    : (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, default);
            }
            return PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThanOrEquals_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThanOrEquals_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfGreaterThanOrEquals_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfGreaterThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfGreaterThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOfGreaterThanOrEquals(ptr, length, value);
        }

        public static partial T* PointerIndexOfLessThan<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOfLessThan_1Byte(ptr, length, value),
                2 => PointerIndexOfLessThan_2Bytes(ptr, length, value),
                4 => PointerIndexOfLessThan_4Bytes(ptr, length, value),
                8 => PointerIndexOfLessThan_8Bytes(ptr, length, value),
                _ => PointerIndexOfLessThanSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThan_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? null
                    : (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, default);
            }
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThan_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThan_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThan_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfLessThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfLessThan(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOfLessThan(ptr, length, value);
        }

        public static partial T* PointerIndexOfLessThanOrEquals<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => PointerIndexOfLessThanOrEquals_1Byte(ptr, length, value),
                2 => PointerIndexOfLessThanOrEquals_2Bytes(ptr, length, value),
                4 => PointerIndexOfLessThanOrEquals_4Bytes(ptr, length, value),
                8 => PointerIndexOfLessThanOrEquals_8Bytes(ptr, length, value),
                _ => PointerIndexOfLessThanOrEqualsSlow(ptr, value, length)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThanOrEquals_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
            {
                return UnsafeHelper.Equals(value, default)
                    ? (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, default)
                    : ptr;
            }
            return PointerIndexOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThanOrEquals_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return (T*)FastCore<ushort>.PointerIndexOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return PointerIndexOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThanOrEquals_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return (T*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return (T*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return PointerIndexOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* PointerIndexOfLessThanOrEquals_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return (T*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return (T*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return PointerIndexOfLessThanOrEqualsSlow(ptr, value, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T* PointerIndexOfLessThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfLessThanOrEquals(ptr, length, value)
                };
            }
            return SlowCore<T>.PointerIndexOfLessThanOrEquals(ptr, length, value);
        }
    }
}
#endif