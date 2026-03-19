#if NET8_0_OR_GREATER
using System;
using System.Runtime.Intrinsics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, BinaryOperationMethod method)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedBinaryOperationCore_512(ref ptr, ref length, value, method);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedBinaryOperationCore_256(ref ptr, ref length, value, method);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedBinaryOperationCore_128(ref ptr, ref length, value, method);
                else if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    VectorizedBinaryOperationCore_64(ref ptr, ref length, value, method);
                else
                    throw new InvalidOperationException("Unreachable branch!");
                ScalarizedBinaryOperationCore(ref ptr, ref length, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_512(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                Vector512<T> valueVector = Vector512.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector512<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector512<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector512<T>.Count;
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        Vector512<T> sourceVector2 = Vector512.Load(ptr2);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        VectorizedBinaryOperation(sourceVector2, valueVector, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        ptr += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    VectorizedBinaryOperation(sourceVector, valueVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                } while (length >= (nuint)Vector512<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_256(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                Vector256<T> valueVector = Vector256.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector256<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector256<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector256<T>.Count;
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        Vector256<T> sourceVector2 = Vector256.Load(ptr2);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        VectorizedBinaryOperation(sourceVector2, valueVector, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        ptr += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    VectorizedBinaryOperation(sourceVector, valueVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                } while (length >= (nuint)Vector256<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_128(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                Vector128<T> valueVector = Vector128.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector128<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector128<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector128<T>.Count;
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        Vector128<T> sourceVector2 = Vector128.Load(ptr2);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        VectorizedBinaryOperation(sourceVector2, valueVector, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        ptr += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    VectorizedBinaryOperation(sourceVector, valueVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                } while (length >= (nuint)Vector128<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_64(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                Vector64<T> valueVector = Vector64.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector64<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector64<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector64<T>.Count;
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        Vector64<T> sourceVector2 = Vector64.Load(ptr2);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        VectorizedBinaryOperation(sourceVector2, valueVector, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        VectorizedBinaryOperation(sourceVector, valueVector, method).Store(ptr);
                        ptr += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    VectorizedBinaryOperation(sourceVector, valueVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                } while (length >= (nuint)Vector64<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedBinaryOperation(in Vector512<T> sourceVector, in Vector512<T> valueVector,
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

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedBinaryOperation(in Vector256<T> sourceVector, in Vector256<T> valueVector,
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

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedBinaryOperation(in Vector128<T> sourceVector, in Vector128<T> valueVector,
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

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedBinaryOperation(in Vector64<T> sourceVector, in Vector64<T> valueVector,
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