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
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, BinaryOperationMethod method)
            {
                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector<T>.Count;
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        Vector<T> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2);
                        UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, method));
                        UnsafeHelper.WriteUnaligned(ptr2, VectorizedBinaryOperation(sourceVector2, valueVector, method));
                        return;
                    }
                    else
                    {
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, method));
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    UnsafeHelper.Write(ptr, VectorizedBinaryOperation(sourceVector, valueVector, method));
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedBinaryOperationCore(ref ptr, ref length, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedBinaryOperation(in Vector<T> sourceVector, in Vector<T> valueVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => sourceVector | valueVector,
                    BinaryOperationMethod.And => sourceVector & valueVector,
                    BinaryOperationMethod.Xor => sourceVector ^ valueVector,
                    BinaryOperationMethod.Add => sourceVector + valueVector,
                    BinaryOperationMethod.Substract => sourceVector - valueVector,
                    BinaryOperationMethod.Multiply => sourceVector * valueVector,
                    BinaryOperationMethod.Divide => sourceVector / valueVector,
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif

