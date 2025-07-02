using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500
        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOf<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOf(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOf<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOf((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOf((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOf((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOf((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOf((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOf((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOf((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOf((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOf((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOf((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfSlow(ptr, value, length);
        }

        private static T* PointerIndexOfSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.PointerIndexOf(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOf(ptr, value, length);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfExclude<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfExclude(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfExclude<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfExclude((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOfExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfExclude((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfExclude((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOfExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOfExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfExcludeSlow(ptr, value, length);
        }

        private static T* PointerIndexOfExcludeSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.PointerIndexOfExclude(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOfExclude(ptr, value, length);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfGreaterThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfGreaterThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOfGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOfGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfGreaterThanSlow(ptr, value, length);
        }

        private static T* PointerIndexOfGreaterThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.PointerIndexOfGreaterThan(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOfGreaterThan(ptr, value, length);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfGreaterOrEqualsThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfGreaterOrEqualsThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterOrEqualsThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOfGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOfGreaterOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOfGreaterOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfGreaterOrEqualsThanSlow(ptr, value, length);
        }

        private static T* PointerIndexOfGreaterOrEqualsThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfGreaterOrEqualsThan(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOfGreaterOrEqualsThan(ptr, value, length);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfLessThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfLessThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOfLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOfLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOfLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfLessThanSlow(ptr, value, length);
        }

        private static T* PointerIndexOfLessThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
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
                    _ => SlowCore<T>.PointerIndexOfLessThan(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOfLessThan(ptr, value, length);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfLessOrEqualsThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfLessOrEqualsThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessOrEqualsThan<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
                return (T*)FastCore<byte>.PointerIndexOfLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(sbyte))
                return (T*)FastCore<sbyte>.PointerIndexOfLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(short))
                return (T*)FastCore<short>.PointerIndexOfLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
                return (T*)FastCore<ushort>.PointerIndexOfLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(int))
                return (T*)FastCore<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint))
                return (T*)FastCore<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(long))
                return (T*)FastCore<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong))
                return (T*)FastCore<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(float))
                return (T*)FastCore<float>.PointerIndexOfLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (typeof(T) == typeof(double))
                return (T*)FastCore<double>.PointerIndexOfLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (typeof(T) == typeof(nint))
                return (T*)FastCore.PointerIndexOfLessOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (typeof(T) == typeof(nuint))
                return (T*)FastCore.PointerIndexOfLessOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            return PointerIndexOfLessOrEqualsThanSlow(ptr, value, length);
        }

        private static T* PointerIndexOfLessOrEqualsThanSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => (T*)FastCore<byte>.PointerIndexOfLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => (T*)FastCore<sbyte>.PointerIndexOfLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => (T*)FastCore<short>.PointerIndexOfLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.Char or TypeCode.UInt16 => (T*)FastCore<ushort>.PointerIndexOfLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => (T*)FastCore<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => (T*)FastCore<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => (T*)FastCore<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => (T*)FastCore<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    TypeCode.Single => (T*)FastCore<float>.PointerIndexOfLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(value)),
                    TypeCode.Double => (T*)FastCore<double>.PointerIndexOfLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(value)),
                    _ => SlowCore<T>.PointerIndexOfLessOrEqualsThan(ptr, value, length)
                };
            }
            return SlowCore<T>.PointerIndexOfLessOrEqualsThan(ptr, value, length);
        }
    }
}
