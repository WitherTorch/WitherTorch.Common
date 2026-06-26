#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using RiceTea.Core.Structures;

#pragma warning disable CS8500

namespace RiceTea.Core.Helpers;

unsafe partial class SequenceHelper
{
    private static partial Unit IdentityCore<T>(T* ptr, nuint length)
    {
        if (typeof(T) == typeof(bool))
            return FastCoreOfBoolean.Identity((bool*)ptr, length);
        return Unit.Default;
    }

    private static partial Unit NotCore<T>(T* ptr, nuint length)
    {
        Type type = typeof(T);
        if (type == typeof(bool))
            return FastCoreOfBoolean.Not((bool*)ptr, length);
        if (type == typeof(sbyte))
            return FastCore<sbyte>.Not((sbyte*)ptr, length);
        if (type == typeof(byte))
            return FastCore<byte>.Not((byte*)ptr, length);
        if (type == typeof(short))
            return FastCore<short>.Not((short*)ptr, length);
        if (type == typeof(ushort) || type == typeof(char))
            return FastCore<ushort>.Not((ushort*)ptr, length);
        if (type == typeof(int))
            return FastCore<int>.Not((int*)ptr, length);
        if (type == typeof(uint))
            return FastCore<uint>.Not((uint*)ptr, length);
        if (type == typeof(long))
            return FastCore<long>.Not((long*)ptr, length);
        if (type == typeof(ulong))
            return FastCore<ulong>.Not((ulong*)ptr, length);
        if (type == typeof(nint))
            return FastCore<nint>.Not((nint*)ptr, length);
        if (type == typeof(nuint))
            return FastCore<nuint>.Not((nuint*)ptr, length);
        if (type == typeof(float))
            return FastCore<float>.Not((float*)ptr, length);
        if (type == typeof(double))
            return FastCore<double>.Not((double*)ptr, length);
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
        return NotCoreSlow(ptr, length);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Unit NotCoreSlow<T>(T* ptr, nuint length) => SlowCore<T>.Not(ptr, length);
}
#endif