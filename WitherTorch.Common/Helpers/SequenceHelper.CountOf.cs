using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500
        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOf<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOf(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOf<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOf((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOf((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfSlow(ptr, value, length);
        }

        private static nuint CountOfSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOf((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfExclude<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfExclude(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOfExclude<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOfExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOfExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfExcludeSlow(ptr, value, length);
        }

        private static nuint CountOfExcludeSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfGreaterThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfGreaterThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOfGreaterThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOfGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOfGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfGreaterThanSlow(ptr, value, length);
        }

        private static nuint CountOfGreaterThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfGreaterThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfGreaterThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfGreaterThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfGreaterThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOfGreaterThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOfGreaterThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfGreaterThanOrEqualsSlow(ptr, value, length);
        }

        private static nuint CountOfGreaterThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOfGreaterThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfGreaterThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfGreaterThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOfGreaterThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfLessThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfLessThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOfLessThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOfLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOfLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfLessThanSlow(ptr, value, length);
        }

        private static nuint CountOfLessThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfLessThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfLessThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CountOfLessThanOrEquals<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return FastCore<byte>.CountOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.CountOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return FastCore<short>.CountOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return FastCore<ushort>.CountOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.CountOfLessThanOrEquals((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.CountOfLessThanOrEquals((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return FastCore.CountOfLessThanOrEquals((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return FastCore.CountOfLessThanOrEquals((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return CountOfLessThanOrEqualsSlow(ptr, value, length);
        }

        private static nuint CountOfLessThanOrEqualsSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => FastCore<byte>.CountOfLessThanOrEquals((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.CountOfLessThanOrEquals((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.CountOfLessThanOrEquals((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => FastCore<ushort>.CountOfLessThanOrEquals((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
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
