using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

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
            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(byte* ptr, byte* ptrEnd, byte* ptr2)
            {
                nuint* vectorOperationCounts = stackalloc nuint[InternalShared.VectorClassCount + 1];
                InternalShared.CalculateOperationCount<byte>(unchecked((nuint)MathHelper.MakeUnsigned(ptrEnd - ptr)), vectorOperationCounts);
                nuint operationCount;
#if NET6_0_OR_GREATER
                if (Limits.UseVector512() && (operationCount = vectorOperationCounts[0]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector512<byte> valueVector = Vector512.Load(ptr);
                        Vector512<byte> valueVector2 = Vector512.Load(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector512<byte>.Count;
                        ptr2 += Vector512<byte>.Count;
                    } while (++i < operationCount);
                }
                if (Limits.UseVector256() && (operationCount = vectorOperationCounts[1]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector256<byte> valueVector = Vector256.Load(ptr);
                        Vector256<byte> valueVector2 = Vector256.Load(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector256<byte>.Count;
                        ptr2 += Vector256<byte>.Count;
                    } while (++i < operationCount);
                }
                if (Limits.UseVector128() && (operationCount = vectorOperationCounts[2]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector128<byte> valueVector = Vector128.Load(ptr);
                        Vector128<byte> valueVector2 = Vector128.Load(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector128<byte>.Count;
                        ptr2 += Vector128<byte>.Count;
                    } while (++i < operationCount);
                }
                if (Limits.UseVector64() && (operationCount = vectorOperationCounts[3]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        if (UnsafeHelper.Read<ulong>(ptr) != UnsafeHelper.Read<ulong>(ptr2))
                            return false;
                        ptr += 8;
                        ptr2 += 8;
                    } while (++i < operationCount);
                }
#else
                if (Limits.UseVector()&& (operationCount = vectorOperationCounts[0]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector<byte> valueVector = UnsafeHelper.Read<Vector<byte>>(ptr);
                        Vector<byte> valueVector2 = UnsafeHelper.Read<Vector<byte>>(ptr2);
                        if (valueVector != valueVector2)
                            return false;
                        ptr += Vector<byte>.Count;
                        ptr2 += Vector<byte>.Count;
                    } while (++i < operationCount);
                }
#endif
                operationCount = vectorOperationCounts[InternalShared.VectorClassCount];
                do
                {
                    switch (operationCount)
                    {
                        case 7:
                            if (ptr != ptr2)
                                return false;
                            ptr++;
                            ptr2++;
                            goto case 6;
                        case 6:
                            if (UnsafeHelper.ReadUnaligned<ushort>(ptr) != UnsafeHelper.ReadUnaligned<ushort>(ptr2))
                                return false;
                            ptr += 2;
                            ptr2 += 2;
                            goto case 4;
                        case 5:
                            if (ptr != ptr2)
                                return false;
                            ptr++;
                            ptr2++;
                            goto case 4;
                        case 4:
                            if (UnsafeHelper.ReadUnaligned<uint>(ptr) != UnsafeHelper.ReadUnaligned<uint>(ptr2))
                                return false;
                            goto case 0;
                        case 3:
                            if (ptr != ptr2)
                                return false;
                            ptr++;
                            ptr2++;
                            goto case 2;
                        case 2:
                            if (UnsafeHelper.ReadUnaligned<ushort>(ptr) != UnsafeHelper.ReadUnaligned<ushort>(ptr2))
                                return false;
                            goto case 0;
                        case 1:
                            if (ptr != ptr2)
                                return false;
                            goto case 0;
                        case 0:
                            return true;
                        default:
                            if (UnsafeHelper.ReadUnaligned<ulong>(ptr) != UnsafeHelper.ReadUnaligned<ulong>(ptr2))
                                return false;
                            operationCount -= 8;
                            ptr += 8;
                            ptrEnd += 8;
                            continue;
                    }
                } while (true);
            }
        }

        unsafe partial class Core<T>
        {
            public static bool RangedAddAndEquals(T* ptr, T* ptrEnd, T* ptr2, T lowerBound, T higherBound, T valueToAddInRange)
            {
                if (CheckTypeCanBeVectorized())
                {
                    nuint* vectorOperationCounts = stackalloc nuint[InternalShared.VectorClassCount + 1];
                    InternalShared.CalculateOperationCount<T>(unchecked((nuint)MathHelper.MakeUnsigned(ptrEnd - ptr)), vectorOperationCounts);
                    return VectorizedRangedAddAndEquals(ref ptr, ptrEnd, ref ptr2, lowerBound, higherBound, valueToAddInRange, vectorOperationCounts);
                }
                if (UnsafeHelper.IsPrimitiveType<T>())
                    return FastRangedAddAndEquals(ref ptr, ptrEnd, ref ptr2, lowerBound, higherBound, valueToAddInRange);
                throw new InvalidOperationException();
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals(ref T* ptr, in T* ptrEnd, ref T* ptr2, T lowerBound, T higherBound, T valueToAddInRange,
                nuint* vectorOperationCounts)
            {
                nuint operationCount;
#if NET6_0_OR_GREATER
                if (Limits.UseVector512() && (operationCount = vectorOperationCounts[0]) > 0)
                {
                    Vector512<T> vectorToAddInRange = Vector512.Create(valueToAddInRange);
                    Vector512<T> lowerBoundVector = Vector512.Create(lowerBound);
                    Vector512<T> higherBoundVector = Vector512.Create(higherBound);
                    nuint i = 0;
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
                    } while (++i < operationCount);
                }
                if (Limits.UseVector256() && (operationCount = vectorOperationCounts[1]) > 0)
                {
                    Vector256<T> vectorToAddInRange = Vector256.Create(valueToAddInRange);
                    Vector256<T> lowerBoundVector = Vector256.Create(lowerBound);
                    Vector256<T> higherBoundVector = Vector256.Create(higherBound);
                    nuint i = 0;
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
                    } while (++i < operationCount);
                }
                if (Limits.UseVector128() && (operationCount = vectorOperationCounts[2]) > 0)
                {
                    Vector128<T> vectorToAddInRange = Vector128.Create(valueToAddInRange);
                    Vector128<T> lowerBoundVector = Vector128.Create(lowerBound);
                    Vector128<T> higherBoundVector = Vector128.Create(higherBound);
                    nuint i = 0;
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
                    } while (++i < operationCount);
                }
                if (Limits.UseVector64() && (operationCount = vectorOperationCounts[3]) > 0)
                {
                    Vector64<T> vectorToAddInRange = Vector64.Create(valueToAddInRange);
                    Vector64<T> lowerBoundVector = Vector64.Create(lowerBound);
                    Vector64<T> higherBoundVector = Vector64.Create(higherBound);
                    nuint i = 0;
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
                    } while (++i < operationCount);
                }
#else
                if (Limits.UseVector() && (operationCount = vectorOperationCounts[0]) > 0)
                {
                    Vector<T> vectorToAddInRange = new Vector<T>(valueToAddInRange);
                    Vector<T> lowerBoundVector = new Vector<T>(lowerBound);
                    Vector<T> higherBoundVector = new Vector<T>(higherBound);
                    nuint i = 0;
                    do
                    {
                        Vector<T> valueVector = VectorizedRangedAdd(UnsafeHelper.ReadUnaligned<Vector<T>>(ptr), vectorToAddInRange, lowerBoundVector, higherBoundVector);
                        Vector<T> valueVector2 = VectorizedRangedAdd(UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2), vectorToAddInRange, lowerBoundVector, higherBoundVector);
                        if (!valueVector.Equals(valueVector2))
                            return false;
                        ptr += Vector<T>.Count;
                        ptr2 += Vector<T>.Count;
                    } while (++i < operationCount);
                }
#endif
                operationCount = vectorOperationCounts[InternalShared.VectorClassCount];
                for (nuint i = 0; i < operationCount; i++, ptr++)
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
            private static bool FastRangedAddAndEquals(ref T* ptr, in T* ptrEnd, ref T* ptr2, T lowerBound, T higherBound, T valueToAddInRange)
            {
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                        RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                        return false;
                }
                return true;
            }

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
