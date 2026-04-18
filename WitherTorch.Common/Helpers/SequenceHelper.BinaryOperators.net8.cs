#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Structures;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        private static partial Unit LeftCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Left((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return Unit.Value;
        }

        private static partial Unit RightCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Right((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Right((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Right((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Right((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Right((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Right((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Right((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Right((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Right((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Right((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Right((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Right((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Right((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Right((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Right((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Right((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Right((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Right((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Right((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Right((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Right((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Right(ptr, length, value),
                };
            }
            return RightCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit RightCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Right(ptr, length, value);

        private static partial Unit OrCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Or((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Or((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Or((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Or((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Or((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Or((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Or((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Or((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Or((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Or((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Or((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Or((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Or((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Or((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Or((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Or((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Or((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Or((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Or((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Or((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Or((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Or(ptr, length, value),
                };
            }
            return OrCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit OrCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Or(ptr, length, value);

        private static partial Unit AndCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.And((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.And((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.And((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.And((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.And((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.And((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.And((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.And((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.And((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.And((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.And((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.And((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.And((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.And((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.And((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.And((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.And((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.And((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.And((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.And((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.And(ptr, length, value),
                };
            }
            return AndCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit AndCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.And(ptr, length, value);

        private static partial Unit XorCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Xor((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Xor((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Xor((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Xor((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Xor((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Xor((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Xor((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Xor((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Xor((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Xor((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Xor((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Xor((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Xor((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Xor((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Xor((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Xor((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Xor((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Xor((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Xor((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Xor((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Xor(ptr, length, value),
                };
            }
            return XorCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit XorCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Xor(ptr, length, value);

        private static partial Unit AddCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Add((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Add((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Add((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Add((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Add((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Add((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Add((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Add((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Add((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Add((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Add((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Add((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Add((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Add((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Add((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Add((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Add((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Add((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Add((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Add((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Add(ptr, length, value),
                };
            }
            return AddCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit AddCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Add(ptr, length, value);

        private static partial Unit SubtractCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Subtract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Subtract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Subtract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Subtract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Subtract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Subtract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Subtract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Subtract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Subtract((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Subtract((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Subtract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Subtract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Subtract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Subtract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Subtract((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Subtract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Subtract((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Subtract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Subtract((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Subtract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Subtract(ptr, length, value),
                };
            }
            return SubtractCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit SubtractCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Subtract(ptr, length, value);

        private static partial Unit MultiplyCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Multiply((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Multiply((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Multiply((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Multiply((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Multiply((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Multiply((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Multiply((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Multiply((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Multiply((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Multiply((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Multiply((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Multiply((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Multiply((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Multiply((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Multiply((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Multiply((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Multiply((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Multiply((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Multiply((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Multiply((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Multiply(ptr, length, value),
                };
            }
            return MultiplyCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MultiplyCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Multiply(ptr, length, value);

        private static partial Unit DivideCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Divide((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Divide((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Divide((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Divide((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Divide((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Divide((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Divide((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Divide((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Divide((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Divide((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Divide((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Divide((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Divide((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Divide((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Divide((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Divide((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Divide((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Divide((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Divide((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Divide((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Divide((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Divide(ptr, length, value),
                };
            }
            return DivideCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit DivideCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Divide(ptr, length, value);

        private static partial Unit MaxCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.Or((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Max((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Max((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Max((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Max((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Max((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Max((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Max((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Max((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Max((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Max((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Max((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Max((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Max((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Max((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Max((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Max((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Max((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Max((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Max((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Max((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Max(ptr, length, value),
                };
            }
            return MaxCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MaxCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Max(ptr, length, value);

        private static partial Unit MinCore<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            if (type == typeof(sbyte))
                return FastCore<sbyte>.Min((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (type == typeof(byte))
                return FastCore<byte>.Min((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (type == typeof(short))
                return FastCore<short>.Min((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (type == typeof(ushort) || type == typeof(char))
                return FastCore<ushort>.Min((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            if (type == typeof(int))
                return FastCore<int>.Min((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (type == typeof(uint))
                return FastCore<uint>.Min((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (type == typeof(long))
                return FastCore<long>.Min((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (type == typeof(ulong))
                return FastCore<ulong>.Min((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (type == typeof(nint))
                return FastCore<nint>.Min((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
            if (type == typeof(nuint))
                return FastCore<nuint>.Min((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
            if (type == typeof(float))
                return FastCore<float>.Min((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            if (type == typeof(double))
                return FastCore<double>.Min((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Byte => FastCore<byte>.Min((byte*)ptr, length, UnsafeHelper.As<T, byte>(value)),
                    TypeCode.SByte => FastCore<sbyte>.Min((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value)),
                    TypeCode.Int16 => FastCore<short>.Min((short*)ptr, length, UnsafeHelper.As<T, short>(value)),
                    TypeCode.UInt16 => FastCore<ushort>.Min((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value)),
                    TypeCode.Int32 => FastCore<int>.Min((int*)ptr, length, UnsafeHelper.As<T, int>(value)),
                    TypeCode.UInt32 => FastCore<uint>.Min((uint*)ptr, length, UnsafeHelper.As<T, uint>(value)),
                    TypeCode.Int64 => FastCore<long>.Min((long*)ptr, length, UnsafeHelper.As<T, long>(value)),
                    TypeCode.UInt64 => FastCore<ulong>.Min((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value)),
                    _ => SlowCore<T>.Min(ptr, length, value),
                };
            }
            return MinCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MinCoreSlow<T>(T* ptr, nuint length, T value) => SlowCore<T>.Min(ptr, length, value);
    }
}
#endif