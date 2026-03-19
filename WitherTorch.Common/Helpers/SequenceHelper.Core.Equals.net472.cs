#if NET472_OR_GREATER
using System.Numerics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore
        {
            private static partial bool VectorizedEquals(byte* ptr, byte* ptr2, nuint length)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<byte>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector<byte>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                    Vector<byte> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector<byte>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector<byte>>() - headRemainder; // 取得數量
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
                            ptr += (nuint)Vector<byte>.Count;
                            ptr2 += (nuint)Vector<byte>.Count;
                            length -= (nuint)Vector<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(ptr);
                    Vector<byte> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector<byte>.Count;
                        ptr2 += (nuint)Vector<byte>.Count;
                        length -= (nuint)Vector<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector<byte>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(ptr);
                    Vector<byte> sourceVector2 = UnsafeHelper.Read<Vector<byte>>(ptr2);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector<byte>.Count;
                        ptr2 += (nuint)Vector<byte>.Count;
                        length -= (nuint)Vector<byte>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector<byte>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector<byte>.Count;
                    Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                    Vector<byte> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2);
                    return Vector.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }
        }

        unsafe partial class FastCore<T>
        {
            private static partial bool VectorizedRangedAddAndEquals(T* ptr, T* ptr2, nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                Vector<T> valueToAddInRangeVector = new Vector<T>(valueToAddInRange);
                Vector<T> lowerBoundVector = new Vector<T>(lowerBound);
                Vector<T> higherBoundVector = new Vector<T>(higherBound);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                nuint headRemainder2 = (nuint)ptr2 % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                {
                    if (headRemainder2 == 0)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    Vector<T> sourceVector = VectorizedRangedAdd(
                        UnsafeHelper.ReadUnaligned<Vector<T>>(ptr), 
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector<T> sourceVector2 = VectorizedRangedAdd(
                        UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        if (length > (nuint)Vector<T>.Count * 2)
                        {
                            bool isSameRemainder = headRemainder == headRemainder2;
                            headRemainder = UnsafeHelper.SizeOf<Vector<T>>() - headRemainder; // 取得數量
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
                            ptr += (nuint)Vector<T>.Count;
                            ptr2 += (nuint)Vector<T>.Count;
                            length -= (nuint)Vector<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return false;
                }

            VectorizedLoop_PartialAligned:
                do
                {
                    Vector<T> sourceVector = VectorizedRangedAdd(
                        UnsafeHelper.Read<Vector<T>>(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector<T> sourceVector2 = VectorizedRangedAdd(
                        UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector<T>.Count;
                        ptr2 += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            VectorizedLoop_FullAligned:
                do
                {
                    Vector<T> sourceVector = VectorizedRangedAdd(
                        UnsafeHelper.Read<Vector<T>>(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector<T> sourceVector2 = VectorizedRangedAdd(
                        UnsafeHelper.Read<Vector<T>>(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    if (Vector.EqualsAll(sourceVector, sourceVector2))
                    {
                        ptr += (nuint)Vector<T>.Count;
                        ptr2 += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        continue;
                    }
                    return false;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    ptr2 = ptr2 + length - (nuint)Vector<T>.Count;
                    Vector<T> sourceVector = VectorizedRangedAdd(
                        UnsafeHelper.ReadUnaligned<Vector<T>>(ptr),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    Vector<T> sourceVector2 = VectorizedRangedAdd(
                        UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2),
                        valueToAddInRangeVector, lowerBoundVector, higherBoundVector);
                    return Vector.EqualsAll(sourceVector, sourceVector2);
                }
                else
                    return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedRangedAdd(in Vector<T> sourceVector, in Vector<T> vectorToAdd,
                in Vector<T> lowerBoundVector, in Vector<T> higherBoundVector)
                => sourceVector + (Vector.GreaterThanOrEqual(sourceVector, lowerBoundVector) &
                Vector.LessThanOrEqual(sourceVector, higherBoundVector) & vectorToAdd);
        }
    }
}
#endif
