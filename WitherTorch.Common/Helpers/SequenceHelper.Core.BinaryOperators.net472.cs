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
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, BinaryOperatorType type)
            {
                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Left:
                                break;
                            case BinaryOperatorType.Right:
                                UnsafeHelper.WriteUnaligned(ptr, valueVector);
                                break;
                            default:
                                Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                                UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, type));
                                break;
                        }
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
                    else
                    {
                        if (length > (nuint)Vector<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector<T>.Count * 2)
                        {
                            T* ptr2 = ptr + Vector<T>.Count;
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            Vector<T> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, type));
                            UnsafeHelper.WriteUnaligned(ptr2, VectorizedBinaryOperation(sourceVector2, valueVector, type));
                            return;
                        }
                        else
                        {
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, type));
                            ptr += (nuint)Vector<T>.Count;
                            length -= (nuint)Vector<T>.Count;
                            goto TailProcess;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            UnsafeHelper.Write(ptr, valueVector);
                            break;
                        default:
                            Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                            UnsafeHelper.Write(ptr, VectorizedBinaryOperation(sourceVector, valueVector, type));
                            break;
                    }
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (FastCore.IsIdempotence(type))
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            UnsafeHelper.WriteUnaligned(ptr, valueVector);
                            break;
                        default:
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperation(sourceVector, valueVector, type));
                            break;
                    }
                }
                else
                    ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedBinaryOperation(in Vector<T> sourceVector, in Vector<T> valueVector,
                [InlineParameter] BinaryOperatorType method)
                => method switch
                {
                    BinaryOperatorType.Left => sourceVector,
                    BinaryOperatorType.Right => valueVector,
                    BinaryOperatorType.Or => sourceVector | valueVector,
                    BinaryOperatorType.And => sourceVector & valueVector,
                    BinaryOperatorType.Xor => sourceVector ^ valueVector,
                    BinaryOperatorType.Add => sourceVector + valueVector,
                    BinaryOperatorType.Subtract => sourceVector - valueVector,
                    BinaryOperatorType.Multiply => sourceVector * valueVector,
                    BinaryOperatorType.Divide => sourceVector / valueVector,
                    BinaryOperatorType.Min => Vector.Min(sourceVector, valueVector),
                    BinaryOperatorType.Max => Vector.Max(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif

