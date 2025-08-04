#if NET472_OR_GREATER
using System.Numerics;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore
        {
            public static partial bool Equals(byte* ptr, byte* ptr2, nuint length)
            {
                byte* ptrEnd = ptr + length;
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
                length = unchecked((nuint)(ptrEnd - ptr));
                do
                {
                    switch (length)
                    {
                        case 7:
                            if (*ptr != *ptr2)
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
                            if (*ptr != *ptr2)
                                return false;
                            ptr++;
                            ptr2++;
                            goto case 4;
                        case 4:
                            if (UnsafeHelper.ReadUnaligned<uint>(ptr) != UnsafeHelper.ReadUnaligned<uint>(ptr2))
                                return false;
                            goto case 0;
                        case 3:
                            if (*ptr != *ptr2)
                                return false;
                            ptr++;
                            ptr2++;
                            goto case 2;
                        case 2:
                            if (UnsafeHelper.ReadUnaligned<ushort>(ptr) != UnsafeHelper.ReadUnaligned<ushort>(ptr2))
                                return false;
                            goto case 0;
                        case 1:
                            if (*ptr != *ptr2)
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
            private static partial bool VectorizedRangedAddAndEquals(ref T* ptr, ref T* ptr2, T* ptrEnd, T lowerBound, T higherBound, T valueToAddInRange)
            {
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
                return LegacyRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
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
