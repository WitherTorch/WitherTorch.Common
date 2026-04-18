#if NET472_OR_GREATER
using System;
using System.Diagnostics;
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
            => sizeof(T) switch
            {
                1 => RightCore_1Byte(ptr, length, value),
                2 => RightCore_2Bytes(ptr, length, value),
                4 => RightCore_4Bytes(ptr, length, value),
                8 => RightCore_8Bytes(ptr, length, value),
                _ => RightCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit RightCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Right((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Right((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Right((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return RightCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit RightCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Right((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Right((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return RightCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit RightCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Right((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Right((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Right((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return RightCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit RightCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Right((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Right((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Right((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return RightCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit RightCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Right(ptr, length, value);
        }

        private static partial Unit OrCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => OrCore_1Byte(ptr, length, value),
                2 => OrCore_2Bytes(ptr, length, value),
                4 => OrCore_4Bytes(ptr, length, value),
                8 => OrCore_8Bytes(ptr, length, value),
                _ => OrCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit OrCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Or((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Or((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Or((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return OrCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit OrCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Or((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Or((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return OrCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit OrCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Or((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Or((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Or((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return OrCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit OrCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Or((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Or((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Or((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return OrCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit OrCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Or(ptr, length, value);
        }

        private static partial Unit AndCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => AndCore_1Byte(ptr, length, value),
                2 => AndCore_2Bytes(ptr, length, value),
                4 => AndCore_4Bytes(ptr, length, value),
                8 => AndCore_8Bytes(ptr, length, value),
                _ => AndCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AndCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.And((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.And((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return AndCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AndCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.And((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.And((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return AndCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AndCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.And((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.And((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.And((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return AndCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AndCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.And((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.And((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.And((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return AndCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit AndCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.And(ptr, length, value);
        }

        private static partial Unit XorCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => XorCore_1Byte(ptr, length, value),
                2 => XorCore_2Bytes(ptr, length, value),
                4 => XorCore_4Bytes(ptr, length, value),
                8 => XorCore_8Bytes(ptr, length, value),
                _ => XorCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit XorCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Xor((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Xor((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return XorCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit XorCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Xor((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Xor((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return XorCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit XorCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Xor((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Xor((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Xor((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return XorCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit XorCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Xor((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Xor((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Xor((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return XorCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit XorCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Xor(ptr, length, value);
        }

        private static partial Unit AddCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => AddCore_1Byte(ptr, length, value),
                2 => AddCore_2Bytes(ptr, length, value),
                4 => AddCore_4Bytes(ptr, length, value),
                8 => AddCore_8Bytes(ptr, length, value),
                _ => AddCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AddCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Add((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Add((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return AddCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AddCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Add((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Add((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return AddCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AddCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Add((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Add((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Add((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return AddCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit AddCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Add((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Add((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Add((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return AddCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit AddCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Add(ptr, length, value);
        }

        private static partial Unit SubtractCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => SubtractCore_1Byte(ptr, length, value),
                2 => SubtractCore_2Bytes(ptr, length, value),
                4 => SubtractCore_4Bytes(ptr, length, value),
                8 => SubtractCore_8Bytes(ptr, length, value),
                _ => SubtractCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit SubtractCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Subtract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Subtract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Xor((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return SubtractCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit SubtractCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Subtract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Subtract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return SubtractCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit SubtractCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Subtract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Subtract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Subtract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return SubtractCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit SubtractCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Subtract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Subtract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Subtract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return SubtractCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit SubtractCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Subtract(ptr, length, value);
        }

        private static partial Unit MultiplyCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => MultiplyCore_1Byte(ptr, length, value),
                2 => MultiplyCore_2Bytes(ptr, length, value),
                4 => MultiplyCore_4Bytes(ptr, length, value),
                8 => MultiplyCore_8Bytes(ptr, length, value),
                _ => MultiplyCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MultiplyCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Multiply((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Multiply((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return MultiplyCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MultiplyCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Multiply((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Multiply((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return MultiplyCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MultiplyCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Multiply((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Multiply((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Multiply((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return MultiplyCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MultiplyCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Multiply((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Multiply((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Multiply((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return MultiplyCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MultiplyCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Multiply(ptr, length, value);
        }

        private static partial Unit DivideCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => DivideCore_1Byte(ptr, length, value),
                2 => DivideCore_2Bytes(ptr, length, value),
                4 => DivideCore_4Bytes(ptr, length, value),
                8 => DivideCore_8Bytes(ptr, length, value),
                _ => DivideCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit DivideCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Divide((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Divide((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Divide((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return DivideCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit DivideCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Divide((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Divide((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return DivideCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit DivideCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Divide((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Divide((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Divide((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return DivideCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit DivideCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Divide((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Divide((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Divide((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return DivideCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit DivideCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Divide(ptr, length, value);
        }

        private static partial Unit MinCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => MinCore_1Byte(ptr, length, value),
                2 => MinCore_2Bytes(ptr, length, value),
                4 => MinCore_4Bytes(ptr, length, value),
                8 => MinCore_8Bytes(ptr, length, value),
                _ => MinCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MinCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Min((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Min((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.And((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return MinCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MinCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Min((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Min((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return MinCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MinCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Min((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Min((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Min((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return MinCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MinCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Min((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Min((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Min((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return MinCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MinCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Min(ptr, length, value);
        }

        private static partial Unit MaxCore<T>(T* ptr, nuint length, T value)
            => sizeof(T) switch
            {
                1 => MaxCore_1Byte(ptr, length, value),
                2 => MaxCore_2Bytes(ptr, length, value),
                4 => MaxCore_4Bytes(ptr, length, value),
                8 => MaxCore_8Bytes(ptr, length, value),
                _ => MaxCoreSlow(ptr, length, value),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MaxCore_1Byte<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(sbyte))
                return FastCore<sbyte>.Max((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
            if (typeof(T) == typeof(byte))
                return FastCore<byte>.Max((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
            if (typeof(T) == typeof(bool))
                return FastCoreOfBoolean.Or((bool*)ptr, length, UnsafeHelper.As<T, bool>(value));
            return MaxCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MaxCore_2Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(short))
                return FastCore<short>.Max((short*)ptr, length, UnsafeHelper.As<T, short>(value));
            if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
                return FastCore<ushort>.Max((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
            return MaxCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MaxCore_4Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
                return FastCore<int>.Max((int*)ptr, length, UnsafeHelper.As<T, int>(value));
            if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
                return FastCore<uint>.Max((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
            if (typeof(T) == typeof(float))
                return FastCore<float>.Max((float*)ptr, length, UnsafeHelper.As<T, float>(value));
            return MaxCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Unit MaxCore_8Bytes<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
                return FastCore<long>.Max((long*)ptr, length, UnsafeHelper.As<T, long>(value));
            if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
                return FastCore<ulong>.Max((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
            if (typeof(T) == typeof(double))
                return FastCore<double>.Max((double*)ptr, length, UnsafeHelper.As<T, double>(value));
            return MaxCoreSlow(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Unit MaxCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
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
            return SlowCore<T>.Max(ptr, length, value);
        }
    }
}
#endif