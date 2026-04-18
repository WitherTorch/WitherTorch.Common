#if NET472_OR_GREATER
using System;
using System.Numerics;

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
                                UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
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
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                            UnsafeHelper.WriteUnaligned(ptr2, DoOperation(sourceVector2, valueVector, type));
                            return Unit.Value;
                        }
                        else
                        {
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
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
                            UnsafeHelper.Write(ptr, DoOperation(sourceVector, valueVector, type));
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
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                            break;
                    }
                    return Unit.Value;
                }
                else
                    return ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);

                [Inline(InlineBehavior.Remove)]
                static Vector<T> DoOperation(in Vector<T> sourceVector, in Vector<T> valueVector,
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

        unsafe partial class FastCoreOfBoolean
        {
            private static partial Unit VectorizedBinaryOperationCore(ref bool* ptr, ref nuint length, bool value, BinaryOperatorType type)
            {
                DebugHelper.ThrowIf(type == BinaryOperatorType.Divide && !value);
                Vector<byte> valueVector = new Vector<byte>(MathHelper.BooleanToUInt8(value));

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<byte>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    if (FastCore.IsIdempotence(type))
                    {
                        switch (type)
                        {
                            case BinaryOperatorType.Right:
                                UnsafeHelper.WriteUnaligned(ptr, valueVector);
                                break;
                            default:
                                Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                                UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                                break;
                        }
                        if (length > (nuint)Vector<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector<byte>.Count;
                            length -= (nuint)Vector<byte>.Count;
                            goto TailProcess;
                        }
                    }
                    else
                    {
                        if (length > (nuint)Vector<byte>.Count * 2)
                        {
                            headRemainder = (UnsafeHelper.SizeOf<Vector<byte>>() - headRemainder) / UnsafeHelper.SizeOf<bool>(); // 取得數量
                            ScalarizedBinaryOperationCore(ref ptr, ref headRemainder, value, type);
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else if (length == (nuint)Vector<byte>.Count * 2)
                        {
                            bool* ptr2 = ptr + Vector<byte>.Count;
                            Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                            Vector<byte> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2);
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                            UnsafeHelper.WriteUnaligned(ptr2, DoOperation(sourceVector2, valueVector, type));
                            return Unit.Value;
                        }
                        else
                        {
                            Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                            ptr += (nuint)Vector<byte>.Count;
                            length -= (nuint)Vector<byte>.Count;
                            goto TailProcess;
                        }
                    }
                }

            VectorizedLoop:
                do
                {
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            UnsafeHelper.Write(ptr, valueVector);
                            break;
                        default:
                            Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(ptr);
                            UnsafeHelper.Write(ptr, DoOperation(sourceVector, valueVector, type));
                            break;
                    }
                    ptr += (nuint)Vector<byte>.Count;
                    length -= (nuint)Vector<byte>.Count;
                } while (length >= (nuint)Vector<byte>.Count);
                goto TailProcess;

            TailProcess:
                if (FastCore.IsIdempotence(type))
                {
                    ptr = ptr + length - (nuint)Vector<byte>.Count;
                    switch (type)
                    {
                        case BinaryOperatorType.Right:
                            UnsafeHelper.WriteUnaligned(ptr, valueVector);
                            break;
                        default:
                            Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, valueVector, type));
                            break;
                    }
                    return Unit.Value;
                }
                else
                    return ScalarizedBinaryOperationCore(ref ptr, ref length, value, type);

                [Inline(InlineBehavior.Remove)]
                static Vector<byte> Normalize(in Vector<byte> sourceVector) => sourceVector & Vector<byte>.One;

                [Inline(InlineBehavior.Remove)]
                static Vector<byte> DoOperation(in Vector<byte> sourceVector, in Vector<byte> valueVector,
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

