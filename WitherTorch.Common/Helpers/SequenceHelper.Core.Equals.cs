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
#pragma warning disable CS8500
    partial class SequenceHelper
    {
        unsafe partial class FastCore
        {
            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(byte* ptr, byte* ptr2, nuint length)
            {
                byte* ptrEnd = ptr + length;
#if NET6_0_OR_GREATER
                if (Limits.UseVector512())
                {
                    Vector512<byte>* ptrLimit = ((Vector512<byte>*)ptr) + 1;
                    Vector512<byte>* ptrLimit2 = ((Vector512<byte>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            if (Vector512.Load(ptr) != Vector512.Load(ptr2))
                                return false;
                            ptr = (byte*)ptrLimit;
                            ptr2 = (byte*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Limits.UseVector256())
                {
                    Vector256<byte>* ptrLimit = ((Vector256<byte>*)ptr) + 1;
                    Vector256<byte>* ptrLimit2 = ((Vector256<byte>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            if (Vector256.Load(ptr) != Vector256.Load(ptr2))
                                return false;
                            ptr = (byte*)ptrLimit;
                            ptr2 = (byte*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Limits.UseVector128())
                {
                    Vector128<byte>* ptrLimit = ((Vector128<byte>*)ptr) + 1;
                    Vector128<byte>* ptrLimit2 = ((Vector128<byte>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            if (Vector128.Load(ptr) != Vector128.Load(ptr2))
                                return false;
                            ptr = (byte*)ptrLimit;
                            ptr2 = (byte*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
#else
                if (Limits.UseVector())
                {
                    Vector<byte>* ptrLimit = ((Vector<byte>*)ptr) + 1;
                    Vector<byte>* ptrLimit2 = ((Vector<byte>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            if (UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr) != UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2))
                                return false;
                            ptr = (byte*)ptrLimit;
                            ptr2 = (byte*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
#endif
                length = unchecked((nuint)(ptrEnd - ptr));
                do
                {
                    switch (length)
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
                            length -= 8;
                            ptr += 8;
                            ptr2 += 8;
                            continue;
                    }
                } while (true);
            }
        }

        unsafe partial class FastCore<T>
        {
            public static bool RangedAddAndEquals(T* ptr, T* ptr2, nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                T* ptrEnd = ptr + length;
                if (CheckTypeCanBeVectorized())
                    return VectorizedRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
                return LegacyRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals(ref T* ptr, ref T* ptr2, T* ptrEnd, T lowerBound, T higherBound, T valueToAddInRange)
            {
#if NET6_0_OR_GREATER
                if (Limits.UseVector512())
                {
                    Vector512<T>* ptrLimit = ((Vector512<T>*)ptr) + 1;
                    Vector512<T>* ptrLimit2 = ((Vector512<T>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
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
                            ptr = (T*)ptrLimit;
                            ptr2 = (T*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return false;
                    }
                }
                if (Limits.UseVector256())
                {
                    Vector256<T>* ptrLimit = ((Vector256<T>*)ptr) + 1;
                    Vector256<T>* ptrLimit2 = ((Vector256<T>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
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
                            ptr = (T*)ptrLimit;
                            ptr2 = (T*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return false;
                    }
                }
                if (Limits.UseVector128())
                {
                    Vector128<T>* ptrLimit = ((Vector128<T>*)ptr) + 1;
                    Vector128<T>* ptrLimit2 = ((Vector128<T>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
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
                            ptr = (T*)ptrLimit;
                            ptr2 = (T*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return false;
                    }
                }
                if (Limits.UseVector64())
                {
                    Vector64<T>* ptrLimit = ((Vector64<T>*)ptr) + 1;
                    Vector64<T>* ptrLimit2 = ((Vector64<T>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
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
                            ptr = (T*)ptrLimit;
                            ptr2 = (T*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return false;
                    }
                }
#else
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    Vector<T>* ptrLimit2 = ((Vector<T>*)ptr2) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector<T> vectorToAddInRange = new Vector<T>(valueToAddInRange);
                        Vector<T> lowerBoundVector = new Vector<T>(lowerBound);
                        Vector<T> higherBoundVector = new Vector<T>(higherBound);
                        do
                        {
                            Vector<T> valueVector = VectorizedRangedAdd(UnsafeHelper.ReadUnaligned<Vector<T>>(ptr), vectorToAddInRange,
                                lowerBoundVector, higherBoundVector);
                            Vector<T> valueVector2 = VectorizedRangedAdd(UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2), vectorToAddInRange,
                                lowerBoundVector, higherBoundVector);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr = (T*)ptrLimit;
                            ptr2 = (T*)ptrLimit2++;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return false;
                    }
                }
#endif
                return LegacyRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
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
            private static bool LegacyRangedAddAndEquals(ref T* ptr, ref T* ptr2, T* ptrEnd, T lowerBound, T higherBound, T valueToAddInRange)
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
                if (UnsafeHelper.IsUnsigned<T>())
                    return UnsafeHelper.IsGreaterOrEqualsThanUnsigned(a, b);
                return UnsafeHelper.IsGreaterOrEqualsThan(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsGreaterOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) >= 0;

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsFast(T a, T b)
            {
                if (UnsafeHelper.IsUnsigned<T>())
                    return UnsafeHelper.IsLessOrEqualsThanUnsigned(a, b);
                return UnsafeHelper.IsLessOrEqualsThan(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) <= 0;
        }
    }

    unsafe partial class SlowCore<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(T* ptr, T* ptr2, nuint length)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (nuint i = 0; i < length; i++)
            {
                if (comparer.Equals(ptr[i], ptr2[i]))
                    continue;
                return false;
            }
            return true;
        }
    }
}
