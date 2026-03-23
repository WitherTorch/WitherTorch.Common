#if NET472_OR_GREATER
using System;
using System.Numerics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, UnaryOperatorType method)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector<T>.Count;
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        Vector<T> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2);
                        UnsafeHelper.WriteUnaligned(ptr, VectorizedUnaryOperation(sourceVector, method));
                        UnsafeHelper.WriteUnaligned(ptr2, VectorizedUnaryOperation(sourceVector2, method));
                        return;
                    }
                    else
                    {
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, VectorizedUnaryOperation(sourceVector, method));
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    UnsafeHelper.Write(ptr, VectorizedUnaryOperation(sourceVector, method));
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedUnaryOperationCore(ref ptr, ref length, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedUnaryOperation(in Vector<T> sourceVector,
                [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => sourceVector,
                    UnaryOperatorType.Not => ~sourceVector,
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif
