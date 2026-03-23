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
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, BinaryOperatorType type)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedBinaryOperationCore_512(ref ptr, ref length, value, type);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedBinaryOperationCore_256(ref ptr, ref length, value, type);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedBinaryOperationCore_128(ref ptr, ref length, value, type);
                else if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    VectorizedBinaryOperationCore_64(ref ptr, ref length, value, type);
                else
                    throw new InvalidOperationException("Unreachable branch!");
                if (!FastCore.IsIdempotence(type))
                    ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_512(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType type)
            {
                Vector512<T> valueVector = Vector512.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
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
                                valueVector.Store(ptr);
                                break;
                            default:
                                Vector512<T> sourceVector = Vector512.Load(ptr);
                                VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                break;
                        }
                        if (length > (nuint)Vector512<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector512<T>.Count;
                            length -= (nuint)Vector512<T>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector512<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector512<T>.Count * 2)
                        {
                            T* ptr2 = ptr + Vector512<T>.Count;
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    valueVector.Store(ptr2);
                                    break;
                                default:
                                    Vector512<T> sourceVector = Vector512.Load(ptr);
                                    Vector512<T> sourceVector2 = Vector512.Load(ptr2);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    VectorizedBinaryOperation(sourceVector2, valueVector, type).Store(ptr2);
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    break;
                                default:
                                    Vector512<T> sourceVector = Vector512.Load(ptr);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    break;
                            }
                            ptr += (nuint)Vector512<T>.Count;
                            length -= (nuint)Vector512<T>.Count;
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
                            valueVector.StoreAligned(ptr);
                            break;
                        default:
                            Vector512<T> sourceVector = Vector512.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).StoreAligned(ptr);
                            break;
                    }
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                } while (length >= (nuint)Vector512<T>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<T>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            valueVector.Store(ptr);
                            break;
                        default:
                            Vector512<T> sourceVector = Vector512.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_256(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType type)
            {
                Vector256<T> valueVector = Vector256.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
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
                                valueVector.Store(ptr);
                                break;
                            default:
                                Vector256<T> sourceVector = Vector256.Load(ptr);
                                VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                break;
                        }
                        if (length > (nuint)Vector256<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector256<T>.Count;
                            length -= (nuint)Vector256<T>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector256<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector256<T>.Count * 2)
                        {
                            T* ptr2 = ptr + Vector256<T>.Count;
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    valueVector.Store(ptr2);
                                    break;
                                default:
                                    Vector256<T> sourceVector = Vector256.Load(ptr);
                                    Vector256<T> sourceVector2 = Vector256.Load(ptr2);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    VectorizedBinaryOperation(sourceVector2, valueVector, type).Store(ptr2);
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    break;
                                default:
                                    Vector256<T> sourceVector = Vector256.Load(ptr);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    break;
                            }
                            ptr += (nuint)Vector256<T>.Count;
                            length -= (nuint)Vector256<T>.Count;
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
                            valueVector.StoreAligned(ptr);
                            break;
                        default:
                            Vector256<T> sourceVector = Vector256.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).StoreAligned(ptr);
                            break;
                    }
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                } while (length >= (nuint)Vector256<T>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<T>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            valueVector.Store(ptr);
                            break;
                        default:
                            Vector256<T> sourceVector = Vector256.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_128(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType type)
            {
                Vector128<T> valueVector = Vector128.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
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
                                valueVector.Store(ptr);
                                break;
                            default:
                                Vector128<T> sourceVector = Vector128.Load(ptr);
                                VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                break;
                        }
                        if (length > (nuint)Vector128<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector128<T>.Count;
                            length -= (nuint)Vector128<T>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector128<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector128<T>.Count * 2)
                        {
                            T* ptr2 = ptr + Vector128<T>.Count;
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    valueVector.Store(ptr2);
                                    break;
                                default:
                                    Vector128<T> sourceVector = Vector128.Load(ptr);
                                    Vector128<T> sourceVector2 = Vector128.Load(ptr2);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    VectorizedBinaryOperation(sourceVector2, valueVector, type).Store(ptr2);
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    break;
                                default:
                                    Vector128<T> sourceVector = Vector128.Load(ptr);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    break;
                            }
                            ptr += (nuint)Vector128<T>.Count;
                            length -= (nuint)Vector128<T>.Count;
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
                            valueVector.StoreAligned(ptr);
                            break;
                        default:
                            Vector128<T> sourceVector = Vector128.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).StoreAligned(ptr);
                            break;
                    }
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                } while (length >= (nuint)Vector128<T>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<T>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            valueVector.Store(ptr);
                            break;
                        default:
                            Vector128<T> sourceVector = Vector128.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_64(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType type)
            {
                Vector64<T> valueVector = Vector64.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
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
                                valueVector.Store(ptr);
                                break;
                            default:
                                Vector64<T> sourceVector = Vector64.Load(ptr);
                                VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                break;
                        }
                        if (length > (nuint)Vector64<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector64<T>.Count;
                            length -= (nuint)Vector64<T>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector64<T>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector64<T>.Count * 2)
                        {
                            T* ptr2 = ptr + Vector64<T>.Count;
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    valueVector.Store(ptr2);
                                    break;
                                default:
                                    Vector64<T> sourceVector = Vector64.Load(ptr);
                                    Vector64<T> sourceVector2 = Vector64.Load(ptr2);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    VectorizedBinaryOperation(sourceVector2, valueVector, type).Store(ptr2);
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case BinaryOperatorType.Left:
                                    break;
                                case BinaryOperatorType.Right:
                                    valueVector.Store(ptr);
                                    break;
                                default:
                                    Vector64<T> sourceVector = Vector64.Load(ptr);
                                    VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                                    break;
                            }
                            ptr += (nuint)Vector64<T>.Count;
                            length -= (nuint)Vector64<T>.Count;
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
                            valueVector.StoreAligned(ptr);
                            break;
                        default:
                            Vector64<T> sourceVector = Vector64.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).StoreAligned(ptr);
                            break;
                    }
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                } while (length >= (nuint)Vector64<T>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<T>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Left:
                            break;
                        case BinaryOperatorType.Right:
                            valueVector.Store(ptr);
                            break;
                        default:
                            Vector64<T> sourceVector = Vector64.Load(ptr);
                            VectorizedBinaryOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedBinaryOperation(in Vector512<T> sourceVector, in Vector512<T> valueVector,
                [InlineParameter] BinaryOperatorType type)
                => type switch
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
                    BinaryOperatorType.Min => Vector512.Min(sourceVector, valueVector),
                    BinaryOperatorType.Max => Vector512.Max(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(type)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedBinaryOperation(in Vector256<T> sourceVector, in Vector256<T> valueVector,
                [InlineParameter] BinaryOperatorType type)
                => type switch
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
                    BinaryOperatorType.Min => Vector256.Min(sourceVector, valueVector),
                    BinaryOperatorType.Max => Vector256.Max(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(type)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedBinaryOperation(in Vector128<T> sourceVector, in Vector128<T> valueVector,
                [InlineParameter] BinaryOperatorType type)
                => type switch
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
                    BinaryOperatorType.Min => Vector128.Min(sourceVector, valueVector),
                    BinaryOperatorType.Max => Vector128.Max(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(type)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedBinaryOperation(in Vector64<T> sourceVector, in Vector64<T> valueVector,
                [InlineParameter] BinaryOperatorType type)
                => type switch
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
                    BinaryOperatorType.Min => Vector64.Min(sourceVector, valueVector),
                    BinaryOperatorType.Max => Vector64.Max(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(type)),
                };
        }
    }
}
#endif