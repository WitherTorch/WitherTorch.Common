using System.Runtime.CompilerServices;

using WitherTorch.Common;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Buffers;


#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Text
{
    internal static class InternalStringHelper
    {
        public const char Latin1StringLimit = '\u00ff';

        // LL = Latin1 to Latin1
        // LU = Latin1 to Utf16

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_LL(byte* a, byte* b, nuint length)
        {
            for (nuint i = 0; i < length; i++)
            {
                int comparison = a[i].CompareTo(b[i]);
                if (comparison != 0)
                    return comparison;
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CompareTo_LU(byte* a, char* b, nuint length)
        {
            for (nuint i = 0; i < length; i++)
            {
                int comparison = unchecked((char)a[i]).CompareTo(b[i]);
                if (comparison != 0)
                    return comparison;
            }
            return 0;
        }

        public static unsafe char* PointerIndexOf(char* ptr, nuint count, byte* value, nuint valueLength)
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, count, unchecked((char)*value));

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* ptrBuffer = buffer)
                {
                    WidenAndCopyTo(value, valueLength, ptrBuffer);
                    return PointerIndexOf(ptr, count, ptrBuffer, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        public static unsafe byte* PointerIndexOf(byte* ptr, nuint count, char* value, nuint valueLength)
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
            {
                char valueHead = *value;
                if (valueHead > Latin1StringLimit)
                    return null;
                return SequenceHelper.PointerIndexOf(ptr, count, unchecked((byte)valueHead));
            }

            if (SequenceHelper.ContainsGreaterThan(value, valueLength, unchecked((char)byte.MaxValue)))
                return null;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* ptrBuffer = buffer)
                {
                    NarrowAndCopyTo(value, valueLength, ptrBuffer);
                    return PointerIndexOf(ptr, count, ptrBuffer, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        public static unsafe T* PointerIndexOf<T>(T* ptr, T* ptrEnd, T* value, nuint valueLength) where T : unmanaged
        {
            if (ptrEnd < ptr)
                return null;

            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, ptrEnd, *value);

            return PointerIndexOfCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value, valueLength);
        }

        public static unsafe T* PointerIndexOf<T>(T* ptr, nuint count, T* value, nuint valueLength) where T : unmanaged
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, count, *value);

            return PointerIndexOfCore(ptr, count, value, valueLength);
        }

        private static unsafe T* PointerIndexOfCore<T>(T* ptr, nuint count, T* value, nuint valueLength) where T : unmanaged
        {
            T valueHead = *value;
            value++;
            valueLength--;

            T* ptrEnd = ptr + count - valueLength;
            while ((ptr = SequenceHelper.PointerIndexOf(ptr, ptrEnd, valueHead)) != null)
            {
                if (SequenceHelper.Equals(ptr + 1, value, valueLength))
                    return ptr;
                ptr++;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void NarrowAndCopyTo(char* source, nuint length, byte* dest)
        {
            char* sourceEnd = source + length;
#if NET8_0_OR_GREATER
            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 2;
                Vector512<byte>* destLimit = ((Vector512<byte>*)dest) + 1;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector512<ushort> sourceVectorLow = Vector512.Load((ushort*)source);
                        Vector512<ushort> sourceVectorHigh = Vector512.Load((ushort*)(((Vector512<ushort>*)source) + 1));
                        Vector512.Narrow(sourceVectorLow, sourceVectorHigh).Store(dest);
                        source = (char*)sourceLimit;
                        dest = (byte*)destLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 2;
                Vector256<byte>* destLimit = ((Vector256<byte>*)dest) + 1;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector256<ushort> sourceVectorLow = Vector256.Load((ushort*)source);
                        Vector256<ushort> sourceVectorHigh = Vector256.Load((ushort*)(((Vector256<ushort>*)source) + 1));
                        Vector256.Narrow(sourceVectorLow, sourceVectorHigh).Store(dest);
                        source = (char*)sourceLimit;
                        dest = (byte*)destLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 2;
                Vector128<byte>* destLimit = ((Vector128<byte>*)dest) + 1;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector128<ushort> sourceVectorLow = Vector128.Load((ushort*)source);
                        Vector128<ushort> sourceVectorHigh = Vector128.Load((ushort*)(((Vector128<ushort>*)source) + 1));
                        Vector128.Narrow(sourceVectorLow, sourceVectorHigh).Store(dest);
                        source = (char*)sourceLimit;
                        dest = (byte*)destLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* sourceLimit = ((Vector64<ushort>*)source) + 2;
                Vector64<byte>* destLimit = ((Vector64<byte>*)dest) + 1;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector64<ushort> sourceVectorLow = Vector64.Load((ushort*)source);
                        Vector64<ushort> sourceVectorHigh = Vector64.Load((ushort*)(((Vector64<ushort>*)source) + 1));
                        Vector64.Narrow(sourceVectorLow, sourceVectorHigh).Store(dest);
                        source = (char*)sourceLimit;
                        dest = (byte*)destLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
#else
            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 2;
                Vector<byte>* destLimit = ((Vector<byte>*)dest) + 1;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector<ushort> sourceVectorLow = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector<ushort> sourceVectorHigh = UnsafeHelper.ReadUnaligned<Vector<ushort>>(((Vector<ushort>*)source) + 1);
                        UnsafeHelper.WriteUnaligned(dest, Vector.Narrow(sourceVectorLow, sourceVectorHigh));
                        source = (char*)sourceLimit;
                        dest = (byte*)destLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
#endif
            for (; source < sourceEnd; source++, dest++)
                *dest = unchecked((byte)*source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WidenAndCopyTo(byte* source, nuint length, char* dest)
        {
            byte* sourceEnd = source + length;
#if NET8_0_OR_GREATER
            if (Limits.UseVector512())
            {
                Vector512<byte>* sourceLimit = ((Vector512<byte>*)source) + 1;
                Vector512<ushort>* destLimit = ((Vector512<ushort>*)dest) + 2;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector512<byte> sourceVector = Vector512.Load(source);
                        (Vector512<ushort> destVectorLow, Vector512<ushort> destVectorHigh) = Vector512.Widen(sourceVector);
                        destVectorLow.Store((ushort*)dest);
                        destVectorHigh.Store((ushort*)(((Vector512<ushort>*)dest) + 1));
                        source = (byte*)sourceLimit;
                        dest = (char*)destLimit;
                        destLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<byte>* sourceLimit = ((Vector256<byte>*)source) + 1;
                Vector256<ushort>* destLimit = ((Vector256<ushort>*)dest) + 2;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector256<byte> sourceVector = Vector256.Load(source);
                        (Vector256<ushort> destVectorLow, Vector256<ushort> destVectorHigh) = Vector256.Widen(sourceVector);
                        destVectorLow.Store((ushort*)dest);
                        destVectorHigh.Store((ushort*)(((Vector256<ushort>*)dest) + 1));
                        source = (byte*)sourceLimit;
                        dest = (char*)destLimit;
                        destLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<byte>* sourceLimit = ((Vector128<byte>*)source) + 1;
                Vector128<ushort>* destLimit = ((Vector128<ushort>*)dest) + 2;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector128<byte> sourceVector = Vector128.Load(source);
                        (Vector128<ushort> destVectorLow, Vector128<ushort> destVectorHigh) = Vector128.Widen(sourceVector);
                        destVectorLow.Store((ushort*)dest);
                        destVectorHigh.Store((ushort*)(((Vector128<ushort>*)dest) + 1));
                        source = (byte*)sourceLimit;
                        dest = (char*)destLimit;
                        destLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<byte>* sourceLimit = ((Vector64<byte>*)source) + 1;
                Vector64<ushort>* destLimit = ((Vector64<ushort>*)dest) + 2;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector64<byte> sourceVector = Vector64.Load(source);
                        (Vector64<ushort> destVectorLow, Vector64<ushort> destVectorHigh) = Vector64.Widen(sourceVector);
                        destVectorLow.Store((ushort*)dest);
                        destVectorHigh.Store((ushort*)(((Vector64<ushort>*)dest) + 1));
                        source = (byte*)sourceLimit;
                        dest = (char*)destLimit;
                        destLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
#else
            if (Limits.UseVector())
            {
                Vector<byte>* sourceLimit = ((Vector<byte>*)source) + 1;
                Vector<ushort>* destLimit = ((Vector<ushort>*)dest) + 2;
                if (sourceLimit < sourceEnd)
                {
                    do
                    {
                        Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(source);
                        Vector.Widen(sourceVector, out Vector<ushort> destVectorLow, out Vector<ushort> destVectorHigh);
                        UnsafeHelper.WriteUnaligned(dest, destVectorLow);
                        UnsafeHelper.WriteUnaligned(((Vector<ushort>*)dest) + 1, destVectorHigh);
                        source = (byte*)sourceLimit;
                        dest = (char*)destLimit;
                        destLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }
#endif
            for (; source < sourceEnd; source++, dest++)
                *dest = unchecked((char)*source);
        }
    }
}
