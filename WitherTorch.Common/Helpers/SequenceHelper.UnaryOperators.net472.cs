#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Structures;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers;

unsafe partial class SequenceHelper
{
    private static partial Unit IdentityCore<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(bool))
            return FastCoreOfBoolean.Identity((bool*)ptr, length);
        return Unit.Default;
    }

    private static partial Unit NotCore<T>(T* ptr, nuint length)
        => sizeof(T) switch
        {
            1 => NotCore_1Byte(ptr, length),
            2 => NotCore_2Bytes(ptr, length),
            4 => NotCore_4Bytes(ptr, length),
            8 => NotCore_8Bytes(ptr, length),
            _ => NotCoreSlow(ptr, length),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Unit NotCore_1Byte<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(sbyte))
            return FastCore<sbyte>.Not((sbyte*)ptr, length);
        if (typeof(T) == typeof(byte))
            return FastCore<byte>.Not((byte*)ptr, length);
        if (typeof(T) == typeof(bool))
            return FastCoreOfBoolean.Not((bool*)ptr, length);
        return NotCoreSlow(ptr, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Unit NotCore_2Bytes<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(short))
            return FastCore<short>.Not((short*)ptr, length);
        if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
            return FastCore<ushort>.Not((ushort*)ptr, length);
        return NotCoreSlow(ptr, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Unit NotCore_4Bytes<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(int) || typeof(T) == typeof(nint))
            return FastCore<int>.Not((int*)ptr, length);
        if (typeof(T) == typeof(uint) || typeof(T) == typeof(nuint))
            return FastCore<uint>.Not((uint*)ptr, length);
        if (typeof(T) == typeof(float))
            return FastCore<float>.Not((float*)ptr, length);
        return NotCoreSlow(ptr, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Unit NotCore_8Bytes<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(long) || typeof(T) == typeof(nint))
            return FastCore<long>.Not((long*)ptr, length);
        if (typeof(T) == typeof(ulong) || typeof(T) == typeof(nuint))
            return FastCore<ulong>.Not((ulong*)ptr, length);
        if (typeof(T) == typeof(double))
            return FastCore<double>.Not((double*)ptr, length);
        return NotCoreSlow(ptr, length);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Unit NotCoreSlow<T>(T* ptr, nuint length)
    {
        Type type = typeof(T);
        if (type.IsEnum)
        {
            return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
            {
                TypeCode.Byte => FastCore<byte>.Not((byte*)ptr, length),
                TypeCode.SByte => FastCore<sbyte>.Not((sbyte*)ptr, length),
                TypeCode.Int16 => FastCore<short>.Not((short*)ptr, length),
                TypeCode.UInt16 => FastCore<ushort>.Not((ushort*)ptr, length),
                TypeCode.Int32 => FastCore<int>.Not((int*)ptr, length),
                TypeCode.UInt32 => FastCore<uint>.Not((uint*)ptr, length),
                TypeCode.Int64 => FastCore<long>.Not((long*)ptr, length),
                TypeCode.UInt64 => FastCore<ulong>.Not((ulong*)ptr, length),
                _ => SlowCore<T>.Not(ptr, length),
            };
        }
        return SlowCore<T>.Not(ptr, length);
    }
}
#endif