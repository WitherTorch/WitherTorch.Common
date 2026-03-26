#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.Intrinsics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, UnaryOperatorType method)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedUnaryOperationCore_512(ref ptr, ref length, method);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedUnaryOperationCore_256(ref ptr, ref length, method);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedUnaryOperationCore_128(ref ptr, ref length, method);
                else
                    VectorizedUnaryOperationCore_64(ref ptr, ref length, method);
                ScalarizedUnaryOperationCore(ref ptr, ref length, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore_512(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector512<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector512<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector512<T>.Count;
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        Vector512<T> sourceVector2 = Vector512.Load(ptr2);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        VectorizedUnaryOperation(sourceVector2, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        ptr += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    VectorizedUnaryOperation(sourceVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                } while (length >= (nuint)Vector512<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore_256(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector256<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector256<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector256<T>.Count;
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        Vector256<T> sourceVector2 = Vector256.Load(ptr2);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        VectorizedUnaryOperation(sourceVector2, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        ptr += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    VectorizedUnaryOperation(sourceVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                } while (length >= (nuint)Vector256<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore_128(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector128<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector128<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector128<T>.Count;
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        Vector128<T> sourceVector2 = Vector128.Load(ptr2);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        VectorizedUnaryOperation(sourceVector2, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        ptr += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    VectorizedUnaryOperation(sourceVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                } while (length >= (nuint)Vector128<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore_64(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (length > (nuint)Vector64<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector64<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector64<T>.Count;
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        Vector64<T> sourceVector2 = Vector64.Load(ptr2);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        VectorizedUnaryOperation(sourceVector2, method).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        VectorizedUnaryOperation(sourceVector, method).Store(ptr);
                        ptr += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        return;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    VectorizedUnaryOperation(sourceVector, method).StoreAligned(ptr);
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                } while (length >= (nuint)Vector64<T>.Count);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedUnaryOperation(in Vector512<T> sourceVector,
            [InlineParameter] UnaryOperatorType method)
            => method switch
            {
                    UnaryOperatorType.Identity => sourceVector,
                UnaryOperatorType.Not => ~sourceVector,
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedUnaryOperation(in Vector256<T> sourceVector,
                [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => sourceVector,
                    UnaryOperatorType.Not => ~sourceVector,
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedUnaryOperation(in Vector128<T> sourceVector,
                [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => sourceVector,
                    UnaryOperatorType.Not => ~sourceVector,
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedUnaryOperation(in Vector64<T> sourceVector,
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
