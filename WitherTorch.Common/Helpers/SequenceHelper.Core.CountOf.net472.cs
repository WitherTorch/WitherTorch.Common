#if NET472_OR_GREATER
using System.Numerics;

using InlineMethod;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial nuint VectorizedCountOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method)
            {
                nuint counter = 0;

                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (length > (nuint)Vector<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        CountOf_CollectResult(in resultVector, ref counter, headRemainder, isFullyCheck: false, fromMostIndex: true);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        CountOf_CollectResult(in resultVector, ref counter, length, isFullyCheck: true, fromMostIndex: false);
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    CountOf_CollectResult(in resultVector, ref counter, 0, isFullyCheck: true, fromMostIndex: false);
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    CountOf_CollectResult(in resultVector, ref counter, length, isFullyCheck: false, fromMostIndex: true);
                }
                return counter;
            }

            [Inline(InlineBehavior.Remove)]
            private static void CountOf_CollectResult(in Vector<T> sourceVector, ref nuint counter, nuint offset, [InlineParameter] bool isFullyCheck, [InlineParameter] bool fromMostIndex)
            {
                T* ptr = (T*)UnsafeHelper.AsPointerIn(in sourceVector);
                T allBitSet = UnsafeHelper.GetAllBitSetValue<T>();
                if (isFullyCheck)
                {
                    switch (Vector<T>.Count)
                    {
                        case 4:
                            counter +=
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[0], allBitSet)) +
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[1], allBitSet)) +
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[2], allBitSet)) +
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[3], allBitSet));
                            break;
                        case 2:
                            counter +=
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[0], allBitSet)) +
                                MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[1], allBitSet));
                            break;
                        case 1:
                            counter += MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[0], allBitSet));
                            break;
                        default:
                            for (nuint i = 0; i < (nuint)Vector<T>.Count; i += 4, ptr += 4)
                            {
                                counter +=
                                    MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[0], allBitSet)) +
                                    MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[1], allBitSet)) +
                                    MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[2], allBitSet)) +
                                    MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[3], allBitSet));
                            }
                            break;
                    }
                }
                else
                {
                    if (fromMostIndex)
                        ptr += (nuint)Vector<T>.Count - offset;
                    for (; offset >= 4; offset -= 4, ptr += 4) // 4x 展開
                    {
                        counter +=
                            MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[0], allBitSet)) +
                            MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[1], allBitSet)) +
                            MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[2], allBitSet)) +
                            MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(ptr[3], allBitSet));
                    }
                    T* ptrEnd = ptr + offset;
                    if (ptr >= ptrEnd)
                        return;
                    counter += MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(*ptr, allBitSet));
                    ptr++;
                    if (ptr >= ptrEnd)
                        return;
                    counter += MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(*ptr, allBitSet));
                    ptr++;
                    if (ptr >= ptrEnd)
                        return;
                    counter += MathHelper.BooleanToNativeUnsigned(UnsafeHelper.Equals(*ptr, allBitSet));
                }
            }
        }
    }
}
#endif