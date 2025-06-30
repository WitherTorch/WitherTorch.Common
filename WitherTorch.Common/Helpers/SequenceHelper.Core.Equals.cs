using System;
using System.Collections.Generic;

using InlineMethod;
using System.Runtime.CompilerServices;


#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class Core
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(byte* ptr, byte* ptrEnd, byte* ptr2)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated && ptr + Vector512<byte>.Count < ptrEnd)
                {
                    do
                    {
                        Vector512<ulong> valueVector = Vector512.Load((ulong*)ptr);
                        Vector512<ulong> valueVector2 = Vector512.Load((ulong*)ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector512<byte>.Count;
                        ptr2 += Vector512<byte>.Count;
                    }
                    while (ptr + Vector512<byte>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                if (Vector256.IsHardwareAccelerated && ptr + Vector256<byte>.Count < ptrEnd)
                {
                    do
                    {
                        Vector256<ulong> valueVector = Vector256.Load((ulong*)ptr);
                        Vector256<ulong> valueVector2 = Vector256.Load((ulong*)ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector256<byte>.Count;
                        ptr2 += Vector256<byte>.Count;
                    }
                    while (ptr + Vector256<byte>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                if (Vector128.IsHardwareAccelerated && ptr + Vector128<byte>.Count < ptrEnd)
                {
                    do
                    {
                        Vector128<ulong> valueVector = Vector128.Load((ulong*)ptr);
                        Vector128<ulong> valueVector2 = Vector128.Load((ulong*)ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector128<byte>.Count;
                        ptr2 += Vector128<byte>.Count;
                    }
                    while (ptr + Vector128<byte>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
#else
                if (Vector.IsHardwareAccelerated && ptr + Vector<byte>.Count < ptrEnd)
                {
                    do
                    {
                        Vector<ulong> valueVector = UnsafeHelper.Read<Vector<ulong>>(ptr);
                        Vector<ulong> valueVector2 = UnsafeHelper.Read<Vector<ulong>>(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector<byte>.Count;
                        ptr2 += Vector<byte>.Count;
                    }
                    while (ptr + Vector<byte>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
#endif
                if (ptr + sizeof(nuint) < ptrEnd)
                {
                    do
                    {
                        nuint valueVector = UnsafeHelper.Read<nuint>(ptr);
                        nuint valueVector2 = UnsafeHelper.Read<nuint>(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += sizeof(nuint);
                        ptr2 += sizeof(nuint);
                    } while (ptr + sizeof(nuint) < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                {
                    nuint valueVector = 0, valueVector2 = 0;
                    uint byteCount = unchecked((uint)(ptrEnd - ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                    return valueVector == valueVector2;
                }
            }
        }

        unsafe partial class Core<T>
        {
            public static bool RangedAddAndEquals(T* ptr, T* ptrEnd, T* ptr2, T lowerBound, T higherBound, T valueToAddInRange)
            {
                if (CheckTypeCanBeVectorized())
                    return VectorizedRangedAddAndEquals(ref ptr, ptrEnd, ref ptr2, lowerBound, higherBound, valueToAddInRange);
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    for (; ptr < ptrEnd; ptr++, ptr2++)
                    {
                        if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                            RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                            return false;
                    }
                    return true;
                }
                throw new InvalidOperationException();
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals(ref T* ptr, in T* ptrEnd, ref T* ptr2, T lowerBound, T higherBound, T valueToAddInRange)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated && ptr + Vector512<T>.Count < ptrEnd)
                {
                    Vector512<T> vectorToAddInRange = Vector512.Create(valueToAddInRange);
                    Vector512<T> lowerBoundVector = Vector512.Create(lowerBound);
                    Vector512<T> higherBoundVector = Vector512.Create(higherBound);
                    do
                    {
                        Vector512<T> valueVector = VectorizedRangedAdd_512(Vector512.Load(ptr), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        Vector512<T> valueVector2 = VectorizedRangedAdd_512(Vector512.Load(ptr2), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        if (!valueVector.Equals(valueVector2))
                            return false;
                        ptr += Vector512<T>.Count;
                        ptr2 += Vector512<T>.Count;
                    } while (ptr + Vector512<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                if (Vector256.IsHardwareAccelerated && ptr + Vector256<T>.Count < ptrEnd)
                {
                    Vector256<T> vectorToAddInRange = Vector256.Create(valueToAddInRange);
                    Vector256<T> lowerBoundVector = Vector256.Create(lowerBound);
                    Vector256<T> higherBoundVector = Vector256.Create(higherBound);
                    do
                    {
                        Vector256<T> valueVector = VectorizedRangedAdd_256(Vector256.Load(ptr), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        Vector256<T> valueVector2 = VectorizedRangedAdd_256(Vector256.Load(ptr2), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        if (!valueVector.Equals(valueVector2))
                            return false;
                        ptr += Vector256<T>.Count;
                        ptr2 += Vector256<T>.Count;
                    } while (ptr + Vector256<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                if (Vector128.IsHardwareAccelerated && ptr + Vector128<T>.Count < ptrEnd)
                {
                    Vector128<T> vectorToAddInRange = Vector128.Create(valueToAddInRange);
                    Vector128<T> lowerBoundVector = Vector128.Create(lowerBound);
                    Vector128<T> higherBoundVector = Vector128.Create(higherBound);
                    do
                    {
                        Vector128<T> valueVector = VectorizedRangedAdd_128(Vector128.Load(ptr), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        Vector128<T> valueVector2 = VectorizedRangedAdd_128(Vector128.Load(ptr2), vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        if (!valueVector.Equals(valueVector2))
                            return false;
                        ptr += Vector128<T>.Count;
                        ptr2 += Vector128<T>.Count;
                    } while (ptr + Vector128<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return true;
                }
                if (Vector64.IsHardwareAccelerated)
                {
                    if (ptr + Vector64<T>.Count < ptrEnd)
                    {
                        Vector64<T> vectorToAddInRange = Vector64.Create(valueToAddInRange);
                        Vector64<T> lowerBoundVector = Vector64.Create(lowerBound);
                        Vector64<T> higherBoundVector = Vector64.Create(higherBound);
                        do
                        {
                            Vector64<T> valueVector = VectorizedRangedAdd_64(Vector64.Load(ptr), vectorToAddInRange,
                                lowerBoundVector, higherBoundVector);
                            Vector64<T> valueVector2 = VectorizedRangedAdd_64(Vector64.Load(ptr2), vectorToAddInRange,
                                lowerBoundVector, higherBoundVector);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector64<T>.Count;
                            ptr2 += Vector64<T>.Count;
                        } while (ptr + Vector64<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector64<T> vectorToAddInRange = Vector64.Create(valueToAddInRange);
                        Vector64<T> lowerBoundVector = Vector64.Create(lowerBound);
                        Vector64<T> higherBoundVector = Vector64.Create(higherBound);
                        Vector64<T> valueVector = default;
                        Vector64<T> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        valueVector = VectorizedRangedAdd_64(valueVector, vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        valueVector2 = VectorizedRangedAdd_64(valueVector2, vectorToAddInRange,
                            lowerBoundVector, higherBoundVector);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                            RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#else
                if (Vector.IsHardwareAccelerated)
                {
                    if (ptr + Vector<T>.Count < ptrEnd)
                    {
                        Vector<T> vectorToAddInRange = new Vector<T>(valueToAddInRange);
                        Vector<T> lowerBoundVector = new Vector<T>(lowerBound);
                        Vector<T> higherBoundVector = new Vector<T>(higherBound);
                        do
                        {
                            Vector<T> valueVector = VectorizedRangedAdd(UnsafeHelper.Read<Vector<T>>(ptr), vectorToAddInRange, lowerBoundVector, higherBoundVector);
                            Vector<T> valueVector2 = VectorizedRangedAdd(UnsafeHelper.Read<Vector<T>>(ptr2), vectorToAddInRange, lowerBoundVector, higherBoundVector);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector<T>.Count;
                            ptr2 += Vector<T>.Count;
                        } while (ptr + Vector<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector<T> vectorToAddInRange = new Vector<T>(valueToAddInRange);
                        Vector<T> lowerBoundVector = new Vector<T>(lowerBound);
                        Vector<T> higherBoundVector = new Vector<T>(higherBound);
                        Vector<T> valueVector = default;
                        Vector<T> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        valueVector = VectorizedRangedAdd(valueVector, vectorToAddInRange, lowerBoundVector, higherBoundVector);
                        valueVector2 = VectorizedRangedAdd(valueVector2, vectorToAddInRange, lowerBoundVector, higherBoundVector);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                            RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#endif
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                        RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                        return false;
                }
                return true;
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedRangedAdd_512(in Vector512<T> sourceVector, in Vector512<T> vectorToAdd,
                in Vector512<T> lowerBoundVector, in Vector512<T> higherBoundVector)
                => sourceVector + (Vector512.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector512.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedRangedAdd_256(in Vector256<T> sourceVector, in Vector256<T> vectorToAdd,
                in Vector256<T> lowerBoundVector, in Vector256<T> higherBoundVector)
                => sourceVector + (Vector256.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector256.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedRangedAdd_128(in Vector128<T> sourceVector, in Vector128<T> vectorToAdd,
                in Vector128<T> lowerBoundVector, in Vector128<T> higherBoundVector)
                => sourceVector + (Vector128.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector128.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedRangedAdd_64(in Vector64<T> sourceVector, in Vector64<T> vectorToAdd,
                in Vector64<T> lowerBoundVector, in Vector64<T> higherBoundVector)
                => sourceVector + (Vector64.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector64.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);
#else

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedRangedAdd(in Vector<T> sourceVector, in Vector<T> vectorToAdd,
                in Vector<T> lowerBoundVector, in Vector<T> higherBoundVector)
                => sourceVector + (Vector.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);
#endif

            [Inline(InlineBehavior.Remove)]
            private static T RangedAddFast(T source, T valueToAdd, T lowerBound, T higherBound)
            {
                if (IsGreaterOrEqualsFast(source, lowerBound) && IsLessOrEqualsFast(source, higherBound))
                    return UnsafeHelper.Add(source, valueToAdd);
                return source;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsGreaterOrEqualsFast(T a, T b)
            {
                if (IsUnsigned())
                    return UnsafeHelper.IsGreaterOrEqualsThanUnsigned(a, b);
                return UnsafeHelper.IsGreaterOrEqualsThan(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsGreaterOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) >= 0;

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsFast(T a, T b)
            {
                if (IsUnsigned())
                    return UnsafeHelper.IsLessOrEqualsThanUnsigned(a, b);
                return UnsafeHelper.IsLessOrEqualsThan(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) <= 0;
        }
    }
}
