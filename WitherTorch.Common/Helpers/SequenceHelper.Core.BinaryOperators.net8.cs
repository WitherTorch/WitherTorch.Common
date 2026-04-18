#if NET8_0_OR_GREATER
using System;
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial Unit VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, BinaryOperatorType type)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedBinaryOperationCore_512(ref ptr, ref length, value, type);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedBinaryOperationCore_256(ref ptr, ref length, value, type);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedBinaryOperationCore_128(ref ptr, ref length, value, type);
                else
                    VectorizedBinaryOperationCore_64(ref ptr, ref length, value, type);
                if (FastCore.IsIdempotence(type))
                    return Unit.Value;
                else
                    return ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);
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
                                DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            Vector512<T> sourceVector = Vector512.Load(ptr);
                            Vector512<T> sourceVector2 = Vector512.Load(ptr2);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            DoOperation(sourceVector2, valueVector, type).Store(ptr2);
                        }
                        else
                        {
                            Vector512<T> sourceVector = Vector512.Load(ptr);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            DoOperation(sourceVector, valueVector, type).StoreAligned(ptr);
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
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector512<T> DoOperation(in Vector512<T> sourceVector, in Vector512<T> valueVector,
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
                                DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            Vector256<T> sourceVector = Vector256.Load(ptr);
                            Vector256<T> sourceVector2 = Vector256.Load(ptr2);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            DoOperation(sourceVector2, valueVector, type).Store(ptr2);
                        }
                        else
                        {
                            Vector256<T> sourceVector = Vector256.Load(ptr);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            DoOperation(sourceVector, valueVector, type).StoreAligned(ptr);
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
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector256<T> DoOperation(in Vector256<T> sourceVector, in Vector256<T> valueVector,
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
                                DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            Vector128<T> sourceVector = Vector128.Load(ptr);
                            Vector128<T> sourceVector2 = Vector128.Load(ptr2);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            DoOperation(sourceVector2, valueVector, type).Store(ptr2);
                        }
                        else
                        {
                            Vector128<T> sourceVector = Vector128.Load(ptr);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            DoOperation(sourceVector, valueVector, type).StoreAligned(ptr);
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
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector128<T> DoOperation(in Vector128<T> sourceVector, in Vector128<T> valueVector,
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
                                DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            Vector64<T> sourceVector = Vector64.Load(ptr);
                            Vector64<T> sourceVector2 = Vector64.Load(ptr2);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            DoOperation(sourceVector2, valueVector, type).Store(ptr2);
                        }
                        else
                        {
                            Vector64<T> sourceVector = Vector64.Load(ptr);
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
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
                            DoOperation(sourceVector, valueVector, type).StoreAligned(ptr);
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
                            DoOperation(sourceVector, valueVector, type).Store(ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector64<T> DoOperation(in Vector64<T> sourceVector, in Vector64<T> valueVector,
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

        unsafe partial class FastCoreOfBoolean
        {
            private static partial Unit VectorizedBinaryOperationCore(ref bool* ptr, ref nuint length, bool value, BinaryOperatorType type)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                    VectorizedBinaryOperationCore_512(ref ptr, ref length, value, type);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                    VectorizedBinaryOperationCore_256(ref ptr, ref length, value, type);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<bool>.Count)
                    VectorizedBinaryOperationCore_128(ref ptr, ref length, value, type);
                else
                    VectorizedBinaryOperationCore_64(ref ptr, ref length, value, type);
                return FastCore.IsIdempotence(type) ? Unit.Value : ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_512(ref bool* ptr, ref nuint length, bool value, [InlineParameter] BinaryOperatorType type)
            {
                Vector512<byte> valueVector = Vector512.Create(MathHelper.BooleanToUInt8(value));

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<byte>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Right:
                                valueVector.Store((byte*)ptr);
                                break;
                            default:
                                Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                                DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                                break;
                        }
                        if (length > (nuint)Vector512<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector512<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector512<byte>.Count;
                            length -= (nuint)Vector512<byte>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector512<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector512<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector512<byte>.Count * 2)
                        {
                            bool* ptr2 = ptr + Vector512<byte>.Count;
                            Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                            Vector512<byte> sourceVector2 = Vector512.Load((byte*)ptr2);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            DoOperation(sourceVector2, valueVector, type).Store((byte*)ptr2);
                        }
                        else
                        {
                            Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            ptr += (nuint)Vector512<byte>.Count;
                            length -= (nuint)Vector512<byte>.Count;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.StoreAligned((byte*)ptr);
                            break;
                        default:
                            Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).StoreAligned((byte*)ptr);
                            break;
                    }
                    ptr += (nuint)Vector512<byte>.Count;
                    length -= (nuint)Vector512<byte>.Count;
                } while (length >= (nuint)Vector512<byte>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<byte>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.Store((byte*)ptr);
                            break;
                        default:
                            Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector512<byte> Normalize(in Vector512<byte> sourceVector) => sourceVector & Vector512<byte>.One;

                [Inline(InlineBehavior.Remove)]
                static Vector512<byte> DoOperation(in Vector512<byte> sourceVector, in Vector512<byte> valueVector,
                    [InlineParameter] BinaryOperatorType method)
                    => method switch
                    {
                        BinaryOperatorType.Left or BinaryOperatorType.Divide => Normalize(sourceVector),
                        BinaryOperatorType.Right => valueVector,
                        BinaryOperatorType.Or or BinaryOperatorType.Max => Normalize(sourceVector) | valueVector,
                        BinaryOperatorType.And or BinaryOperatorType.Multiply or BinaryOperatorType.Min => sourceVector & valueVector,
                        BinaryOperatorType.Xor or BinaryOperatorType.Add or BinaryOperatorType.Subtract => Normalize(sourceVector) ^ valueVector,
                        _ => throw new ArgumentOutOfRangeException(nameof(method)),
                    };
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_256(ref bool* ptr, ref nuint length, bool value, [InlineParameter] BinaryOperatorType type)
            {
                Vector256<byte> valueVector = Vector256.Create(MathHelper.BooleanToUInt8(value));

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<byte>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Right:
                                valueVector.Store((byte*)ptr);
                                break;
                            default:
                                Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                                DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                                break;
                        }
                        if (length > (nuint)Vector256<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector256<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector256<byte>.Count;
                            length -= (nuint)Vector256<byte>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector256<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector256<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector256<byte>.Count * 2)
                        {
                            bool* ptr2 = ptr + Vector256<byte>.Count;
                            Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                            Vector256<byte> sourceVector2 = Vector256.Load((byte*)ptr2);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            DoOperation(sourceVector2, valueVector, type).Store((byte*)ptr2);
                        }
                        else
                        {
                            Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            ptr += (nuint)Vector256<byte>.Count;
                            length -= (nuint)Vector256<byte>.Count;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.StoreAligned((byte*)ptr);
                            break;
                        default:
                            Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).StoreAligned((byte*)ptr);
                            break;
                    }
                    ptr += (nuint)Vector256<byte>.Count;
                    length -= (nuint)Vector256<byte>.Count;
                } while (length >= (nuint)Vector256<byte>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<byte>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.Store((byte*)ptr);
                            break;
                        default:
                            Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector256<byte> Normalize(in Vector256<byte> sourceVector) => sourceVector & Vector256<byte>.One;

                [Inline(InlineBehavior.Remove)]
                static Vector256<byte> DoOperation(in Vector256<byte> sourceVector, in Vector256<byte> valueVector,
                    [InlineParameter] BinaryOperatorType method)
                    => method switch
                    {
                        BinaryOperatorType.Left or BinaryOperatorType.Divide => Normalize(sourceVector),
                        BinaryOperatorType.Right => valueVector,
                        BinaryOperatorType.Or or BinaryOperatorType.Max => Normalize(sourceVector) | valueVector,
                        BinaryOperatorType.And or BinaryOperatorType.Multiply or BinaryOperatorType.Min => sourceVector & valueVector,
                        BinaryOperatorType.Xor or BinaryOperatorType.Add or BinaryOperatorType.Subtract => Normalize(sourceVector) ^ valueVector,
                        _ => throw new ArgumentOutOfRangeException(nameof(method)),
                    };
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_128(ref bool* ptr, ref nuint length, bool value, [InlineParameter] BinaryOperatorType type)
            {
                Vector128<byte> valueVector = Vector128.Create(MathHelper.BooleanToUInt8(value));

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<byte>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Right:
                                valueVector.Store((byte*)ptr);
                                break;
                            default:
                                Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                                DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                                break;
                        }
                        if (length > (nuint)Vector128<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector128<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector128<byte>.Count;
                            length -= (nuint)Vector128<byte>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector128<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector128<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector128<byte>.Count * 2)
                        {
                            bool* ptr2 = ptr + Vector128<byte>.Count;
                            Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                            Vector128<byte> sourceVector2 = Vector128.Load((byte*)ptr2);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            DoOperation(sourceVector2, valueVector, type).Store((byte*)ptr2);
                        }
                        else
                        {
                            Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            ptr += (nuint)Vector128<byte>.Count;
                            length -= (nuint)Vector128<byte>.Count;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.StoreAligned((byte*)ptr);
                            break;
                        default:
                            Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).StoreAligned((byte*)ptr);
                            break;
                    }
                    ptr += (nuint)Vector128<byte>.Count;
                    length -= (nuint)Vector128<byte>.Count;
                } while (length >= (nuint)Vector128<byte>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<byte>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.Store((byte*)ptr);
                            break;
                        default:
                            Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector128<byte> Normalize(in Vector128<byte> sourceVector) => sourceVector & Vector128<byte>.One;

                [Inline(InlineBehavior.Remove)]
                static Vector128<byte> DoOperation(in Vector128<byte> sourceVector, in Vector128<byte> valueVector,
                    [InlineParameter] BinaryOperatorType method)
                    => method switch
                    {
                        BinaryOperatorType.Left or BinaryOperatorType.Divide => Normalize(sourceVector),
                        BinaryOperatorType.Right => valueVector,
                        BinaryOperatorType.Or or BinaryOperatorType.Max => Normalize(sourceVector) | valueVector,
                        BinaryOperatorType.And or BinaryOperatorType.Multiply or BinaryOperatorType.Min => sourceVector & valueVector,
                        BinaryOperatorType.Xor or BinaryOperatorType.Add or BinaryOperatorType.Subtract => Normalize(sourceVector) ^ valueVector,
                        _ => throw new ArgumentOutOfRangeException(nameof(method)),
                    };
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore_64(ref bool* ptr, ref nuint length, bool value, [InlineParameter] BinaryOperatorType type)
            {
                Vector64<byte> valueVector = Vector64.Create(MathHelper.BooleanToUInt8(value));

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<byte>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Right:
                                valueVector.Store((byte*)ptr);
                                break;
                            default:
                                Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                                DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                                break;
                        }
                        if (length > (nuint)Vector64<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector64<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector64<byte>.Count;
                            length -= (nuint)Vector64<byte>.Count;
                            goto TailProcess_Special;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector64<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector64<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector64<byte>.Count * 2)
                        {
                            bool* ptr2 = ptr + Vector64<byte>.Count;
                            Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                            Vector64<byte> sourceVector2 = Vector64.Load((byte*)ptr2);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            DoOperation(sourceVector2, valueVector, type).Store((byte*)ptr2);
                        }
                        else
                        {
                            Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            ptr += (nuint)Vector64<byte>.Count;
                            length -= (nuint)Vector64<byte>.Count;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.StoreAligned((byte*)ptr);
                            break;
                        default:
                            Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).StoreAligned((byte*)ptr);
                            break;
                    }
                    ptr += (nuint)Vector64<byte>.Count;
                    length -= (nuint)Vector64<byte>.Count;
                } while (length >= (nuint)Vector64<byte>.Count);

            TailProcess_Special:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<byte>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            valueVector.Store((byte*)ptr);
                            break;
                        default:
                            Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                            DoOperation(sourceVector, valueVector, type).Store((byte*)ptr);
                            break;
                    }
                }

                [Inline(InlineBehavior.Remove)]
                static Vector64<byte> Normalize(in Vector64<byte> sourceVector) => sourceVector & Vector64<byte>.One;

                [Inline(InlineBehavior.Remove)]
                static Vector64<byte> DoOperation(in Vector64<byte> sourceVector, in Vector64<byte> valueVector,
                    [InlineParameter] BinaryOperatorType method)
                    => method switch
                    {
                        BinaryOperatorType.Left or BinaryOperatorType.Divide => Normalize(sourceVector),
                        BinaryOperatorType.Right => valueVector,
                        BinaryOperatorType.Or or BinaryOperatorType.Max => Normalize(sourceVector) | valueVector,
                        BinaryOperatorType.And or BinaryOperatorType.Multiply or BinaryOperatorType.Min => sourceVector & valueVector,
                        BinaryOperatorType.Xor or BinaryOperatorType.Add or BinaryOperatorType.Subtract => Normalize(sourceVector) ^ valueVector,
                        _ => throw new ArgumentOutOfRangeException(nameof(method)),
                    };
            }
        }
    }
}
#endif