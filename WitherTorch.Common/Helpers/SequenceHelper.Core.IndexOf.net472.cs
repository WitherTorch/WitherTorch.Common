#if NET472_OR_GREATER
using System.Numerics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method, bool accurateResult)
            {
                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector<T>.Zero)
                    {
                        if (length > (nuint)Vector<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector<T>.Count;
                            length -= (nuint)Vector<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? IndexOf_FindResult(in resultVector, ref ptr) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector<T>.Zero)
                    {
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        continue;
                    }
                    return accurateResult ? IndexOf_FindResult(in resultVector, ref ptr) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector<T>.Zero)
                        return null;
                    return accurateResult ? IndexOf_FindResult(in resultVector, ref ptr) : (T*)Booleans.TrueNative;
                }
                else
                    return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* IndexOf_FindResult(in Vector<T> sourceVector, ref T* sourcePointer)
            {
                T* ptr = (T*)UnsafeHelper.AsPointerIn(in sourceVector);
                T allBitSet = UnsafeHelper.GetAllBitsSetValue<T>();
                switch (Vector<T>.Count)
                {
                    case 4:
                        if (UnsafeHelper.Equals(ptr[0], allBitSet))
                            return sourcePointer + 0;
                        if (UnsafeHelper.Equals(ptr[1], allBitSet))
                            return sourcePointer + 1;
                        if (UnsafeHelper.Equals(ptr[2], allBitSet))
                            return sourcePointer + 2;
                        return sourcePointer + 3;
                    case 2:
                        return sourcePointer + MathHelper.BooleanToNativeUnsigned(UnsafeHelper.NotEquals(ptr[0], allBitSet));
                    case 1:
                        return sourcePointer;
                    default:
                        for (nuint i = 0; i < (nuint)Vector<T>.Count; i += 4, ptr += 4, sourcePointer += 4)
                        {
                            if (UnsafeHelper.Equals(ptr[0], allBitSet))
                                return sourcePointer + 0;
                            if (UnsafeHelper.Equals(ptr[1], allBitSet))
                                return sourcePointer + 1;
                            if (UnsafeHelper.Equals(ptr[2], allBitSet))
                                return sourcePointer + 2;
                            if (UnsafeHelper.Equals(ptr[3], allBitSet))
                                return sourcePointer + 3;
                        }
                        break;
                }
                return null;
            }
        }
    }
}
#endif