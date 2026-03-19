#if NET8_0_OR_GREATER
using System;
using System.Runtime.Intrinsics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
#pragma warning disable CS8500
    partial class SequenceHelper
    {
        unsafe partial class FastCore
        {
            private static partial bool VectorizedEquals(byte* ptr, byte* ptr2, nuint length)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                    return VectorizedEquals_512(ref ptr, ref ptr2, ref length);
                if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                    return VectorizedEquals_256(ref ptr, ref ptr2, ref length);
                if (Limits.UseVector128() && length >= (nuint)Vector128<byte>.Count)
                    return VectorizedEquals_128(ref ptr, ref ptr2, ref length);
                if (Limits.UseVector64() && length >= (nuint)Vector64<byte>.Count)
                    return VectorizedEquals_64(ref ptr, ref ptr2, ref length);
                throw new InvalidOperationException("Unreachable branch!");
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals_512(ref byte* ptr, ref byte* ptr2, ref nuint length)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<byte>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector512<byte>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector512<byte> sourceVector = Vector512.Load(ptr);
                    Vector512<byte> sourceVector2 = Vector512.Load(ptr2);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector512<byte>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector512<byte>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector512<byte>.Count;
                            ptr2 += (nuint)Vector512<byte>.Count;
                            length -= (nuint)Vector512<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector512<byte> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512<byte> sourceVector2 = Vector512.Load(ptr2);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector512<byte>.Count;
                        ptr2 += (nuint)Vector512<byte>.Count;
                        length -= (nuint)Vector512<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector512<byte>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector512<byte> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512<byte> sourceVector2 = Vector512.LoadAligned(ptr2);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector512<byte>.Count;
                        ptr2 += (nuint)Vector512<byte>.Count;
                        length -= (nuint)Vector512<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector512<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<byte>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector512<byte>.Count;
                    Vector512<byte> sourceVector = Vector512.Load(ptr);
                    Vector512<byte> sourceVector2 = Vector512.Load(ptr2);
                    return Vector512.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals_256(ref byte* ptr, ref byte* ptr2, ref nuint length)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<byte>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector256<byte>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector256<byte> sourceVector = Vector256.Load(ptr);
                    Vector256<byte> sourceVector2 = Vector256.Load(ptr2);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector256<byte>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector256<byte>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector256<byte>.Count;
                            ptr2 += (nuint)Vector256<byte>.Count;
                            length -= (nuint)Vector256<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector256<byte> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256<byte> sourceVector2 = Vector256.Load(ptr2);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector256<byte>.Count;
                        ptr2 += (nuint)Vector256<byte>.Count;
                        length -= (nuint)Vector256<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector256<byte>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector256<byte> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256<byte> sourceVector2 = Vector256.LoadAligned(ptr2);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector256<byte>.Count;
                        ptr2 += (nuint)Vector256<byte>.Count;
                        length -= (nuint)Vector256<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector256<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<byte>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector256<byte>.Count;
                    Vector256<byte> sourceVector = Vector256.Load(ptr);
                    Vector256<byte> sourceVector2 = Vector256.Load(ptr2);
                    return Vector256.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals_128(ref byte* ptr, ref byte* ptr2, ref nuint length)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<byte>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector128<byte>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector128<byte> sourceVector = Vector128.Load(ptr);
                    Vector128<byte> sourceVector2 = Vector128.Load(ptr2);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector128<byte>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector128<byte>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector128<byte>.Count;
                            ptr2 += (nuint)Vector128<byte>.Count;
                            length -= (nuint)Vector128<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector128<byte> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128<byte> sourceVector2 = Vector128.Load(ptr2);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector128<byte>.Count;
                        ptr2 += (nuint)Vector128<byte>.Count;
                        length -= (nuint)Vector128<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector128<byte>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector128<byte> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128<byte> sourceVector2 = Vector128.LoadAligned(ptr2);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector128<byte>.Count;
                        ptr2 += (nuint)Vector128<byte>.Count;
                        length -= (nuint)Vector128<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector128<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<byte>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector128<byte>.Count;
                    Vector128<byte> sourceVector = Vector128.Load(ptr);
                    Vector128<byte> sourceVector2 = Vector128.Load(ptr2);
                    return Vector128.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals_64(ref byte* ptr, ref byte* ptr2, ref nuint length)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<byte>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector64<byte>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector64<byte> sourceVector = Vector64.Load(ptr);
                    Vector64<byte> sourceVector2 = Vector64.Load(ptr2);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector64<byte>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector64<byte>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector64<byte>.Count;
                            ptr2 += (nuint)Vector64<byte>.Count;
                            length -= (nuint)Vector64<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector64<byte> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64<byte> sourceVector2 = Vector64.Load(ptr2);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector64<byte>.Count;
                        ptr2 += (nuint)Vector64<byte>.Count;
                        length -= (nuint)Vector64<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector64<byte>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector64<byte> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64<byte> sourceVector2 = Vector64.LoadAligned(ptr2);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector64<byte>.Count;
                        ptr2 += (nuint)Vector64<byte>.Count;
                        length -= (nuint)Vector64<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector64<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<byte>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector64<byte>.Count;
                    Vector64<byte> sourceVector = Vector64.Load(ptr);
                    Vector64<byte> sourceVector2 = Vector64.Load(ptr2);
                    return Vector64.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }
        }

        unsafe partial class FastCore<T>
        {
            private static partial bool VectorizedRangedAddAndEquals(T* ptr, T* ptr2, nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    return VectorizedRangedAddAndEquals_512(ref ptr, ref ptr2, ref length, lowerBound, higherBound, valueToAddInRange);
                if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    return VectorizedRangedAddAndEquals_256(ref ptr, ref ptr2, ref length, lowerBound, higherBound, valueToAddInRange);
                if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    return VectorizedRangedAddAndEquals_128(ref ptr, ref ptr2, ref length, lowerBound, higherBound, valueToAddInRange);
                if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    return VectorizedRangedAddAndEquals_64(ref ptr, ref ptr2, ref length, lowerBound, higherBound, valueToAddInRange);
                throw new InvalidOperationException("Unreachable branch!");
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals_512(ref T* ptr, ref T* ptr2, ref nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                Vector512<T> valueToAddInRangeVector = Vector512.Create(valueToAddInRange);
                Vector512<T> lowerBoundVector = Vector512.Create(lowerBound);
                Vector512<T> higherBoundVector = Vector512.Create(higherBound);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector512<T> sourceVector = VectorizedRangedAdd(
                        Vector512.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector512<T> sourceVector2 = VectorizedRangedAdd(
                        Vector512.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector512<T>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector512<T>.Count;
                            ptr2 += (nuint)Vector512<T>.Count;
                            length -= (nuint)Vector512<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector512<T> sourceVector = VectorizedRangedAdd(
                        Vector512.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector512<T> sourceVector2 = VectorizedRangedAdd(
                        Vector512.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector512<T>.Count;
                        ptr2 += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector512<T> sourceVector = VectorizedRangedAdd(
                        Vector512.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector512<T> sourceVector2 = VectorizedRangedAdd(
                        Vector512.LoadAligned(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector512.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector512<T>.Count;
                        ptr2 += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<T>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector512<T>.Count;
                    Vector512<T> sourceVector = VectorizedRangedAdd(
                        Vector512.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector512<T> sourceVector2 = VectorizedRangedAdd(
                        Vector512.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    return Vector512.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals_256(ref T* ptr, ref T* ptr2, ref nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                Vector256<T> valueToAddInRangeVector = Vector256.Create(valueToAddInRange);
                Vector256<T> lowerBoundVector = Vector256.Create(lowerBound);
                Vector256<T> higherBoundVector = Vector256.Create(higherBound);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector256<T> sourceVector = VectorizedRangedAdd(
                        Vector256.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector256<T> sourceVector2 = VectorizedRangedAdd(
                        Vector256.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector256<T>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector256<T>.Count;
                            ptr2 += (nuint)Vector256<T>.Count;
                            length -= (nuint)Vector256<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector256<T> sourceVector = VectorizedRangedAdd(
                        Vector256.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector256<T> sourceVector2 = VectorizedRangedAdd(
                        Vector256.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector256<T>.Count;
                        ptr2 += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector256<T> sourceVector = VectorizedRangedAdd(
                        Vector256.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector256<T> sourceVector2 = VectorizedRangedAdd(
                        Vector256.LoadAligned(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector256.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector256<T>.Count;
                        ptr2 += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<T>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector256<T>.Count;
                    Vector256<T> sourceVector = VectorizedRangedAdd(
                        Vector256.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector256<T> sourceVector2 = VectorizedRangedAdd(
                        Vector256.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    return Vector256.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals_128(ref T* ptr, ref T* ptr2, ref nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                Vector128<T> valueToAddInRangeVector = Vector128.Create(valueToAddInRange);
                Vector128<T> lowerBoundVector = Vector128.Create(lowerBound);
                Vector128<T> higherBoundVector = Vector128.Create(higherBound);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector128<T> sourceVector = VectorizedRangedAdd(
                        Vector128.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector128<T> sourceVector2 = VectorizedRangedAdd(
                        Vector128.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector128<T>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector128<T>.Count;
                            ptr2 += (nuint)Vector128<T>.Count;
                            length -= (nuint)Vector128<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector128<T> sourceVector = VectorizedRangedAdd(
                        Vector128.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector128<T> sourceVector2 = VectorizedRangedAdd(
                        Vector128.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector128<T>.Count;
                        ptr2 += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector128<T> sourceVector = VectorizedRangedAdd(
                        Vector128.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector128<T> sourceVector2 = VectorizedRangedAdd(
                        Vector128.LoadAligned(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector128.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector128<T>.Count;
                        ptr2 += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<T>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector128<T>.Count;
                    Vector128<T> sourceVector = VectorizedRangedAdd(
                        Vector128.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector128<T> sourceVector2 = VectorizedRangedAdd(
                        Vector128.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    return Vector128.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedRangedAddAndEquals_64(ref T* ptr, ref T* ptr2, ref nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                Vector64<T> valueToAddInRangeVector = Vector64.Create(valueToAddInRange);
                Vector64<T> lowerBoundVector = Vector64.Create(lowerBound);
                Vector64<T> higherBoundVector = Vector64.Create(higherBound);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector64<T> sourceVector = VectorizedRangedAdd(
                        Vector64.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector64<T> sourceVector2 = VectorizedRangedAdd(
                        Vector64.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector64<T>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder; // 取得數量
                            ptr += headRemainder;
                            ptr2 += headRemainder;
                            length -= headRemainder;
                            if (isSameRemainder)
                                goto VectorizedLoop_FullAligned;
                            else
                                goto VectorizedLoop_PartialAligned;
                        }
                        else
                        {
                            ptr += (nuint)Vector64<T>.Count;
                            ptr2 += (nuint)Vector64<T>.Count;
                            length -= (nuint)Vector64<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector64<T> sourceVector = VectorizedRangedAdd(
                        Vector64.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector64<T> sourceVector2 = VectorizedRangedAdd(
                        Vector64.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector64<T>.Count;
                        ptr2 += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector64<T> sourceVector = VectorizedRangedAdd(
                        Vector64.LoadAligned(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector64<T> sourceVector2 = VectorizedRangedAdd(
                        Vector64.LoadAligned(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector64.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector64<T>.Count;
                        ptr2 += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<T>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector64<T>.Count;
                    Vector64<T> sourceVector = VectorizedRangedAdd(
                        Vector64.Load(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector64<T> sourceVector2 = VectorizedRangedAdd(
                        Vector64.Load(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    return Vector64.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedRangedAdd(in Vector512<T> sourceVector, in Vector512<T> vectorToAdd,
                in Vector512<T> lowerBoundVector, in Vector512<T> higherBoundVector)
                => sourceVector + (Vector512.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector512.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedRangedAdd(in Vector256<T> sourceVector, in Vector256<T> vectorToAdd,
                in Vector256<T> lowerBoundVector, in Vector256<T> higherBoundVector)
                => sourceVector + (Vector256.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector256.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedRangedAdd(in Vector128<T> sourceVector, in Vector128<T> vectorToAdd,
                in Vector128<T> lowerBoundVector, in Vector128<T> higherBoundVector)
                => sourceVector + (Vector128.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector128.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedRangedAdd(in Vector64<T> sourceVector, in Vector64<T> vectorToAdd,
                in Vector64<T> lowerBoundVector, in Vector64<T> higherBoundVector)
                => sourceVector + (Vector64.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector64.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);
        }
    }
}
#endif
