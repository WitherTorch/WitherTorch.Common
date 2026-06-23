#if NET472_OR_GREATER
using System;
using System.Numerics;

using InlineMethod;

using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Helpers;

partial class SequenceHelper
{
    unsafe partial class FastCore<T>
    {
        private static partial Unit VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    switch (type)
                    {
                        case UnaryOperatorType.Identity:
                            break;
                        default:
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector<T>.Count;
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        Vector<T> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2);
                        UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
                        UnsafeHelper.WriteUnaligned(ptr2, DoOperation(sourceVector2, type));
                        return Unit.Default;
                    }
                    else
                    {
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
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
                    case UnaryOperatorType.Identity:
                        break;
                    default:
                        Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                        UnsafeHelper.Write(ptr, DoOperation(sourceVector, type));
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
                    case UnaryOperatorType.Identity:
                        break;
                    default:
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
                        break;
                }
                return Unit.Default;
            }
            else
                return ScalarizedUnaryOperationCore(ref ptr, ref length, type);

            [Inline(InlineBehavior.Remove)]
            static Vector<T> DoOperation(in Vector<T> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => sourceVector,
                    UnaryOperatorType.Not => ~sourceVector,
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }

    unsafe partial class FastCoreOfBoolean
    {
        private static partial Unit VectorizedUnaryOperationCore(ref bool* ptr, ref nuint length, UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<byte>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                    UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector<byte>.Count * 2)
                    {
                        bool* ptr2 = ptr + Vector<byte>.Count;
                        Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                        Vector<byte> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr2);
                        UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
                        UnsafeHelper.WriteUnaligned(ptr2, DoOperation(sourceVector2, type));
                        return Unit.Default;
                    }
                    else
                    {
                        Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
                        ptr += (nuint)Vector<byte>.Count;
                        length -= (nuint)Vector<byte>.Count;
                        goto TailProcess;
                    }
                }
            }

        VectorizedLoop:
            do
            {
                Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(ptr);
                UnsafeHelper.Write(ptr, DoOperation(sourceVector, type));
                ptr += (nuint)Vector<byte>.Count;
                length -= (nuint)Vector<byte>.Count;
            } while (length >= (nuint)Vector<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (FastCore.IsIdempotence(type))
            {
                ptr = ptr + length - (nuint)Vector<byte>.Count;
                Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(ptr);
                UnsafeHelper.WriteUnaligned(ptr, DoOperation(sourceVector, type));
                return Unit.Default;
            }
            else
                return ScalarizedUnaryOperationCore(ref ptr, ref length, type);

            [Inline(InlineBehavior.Remove)]
            static Vector<byte> Normalize(in Vector<byte> sourceVector) => sourceVector & Vector<byte>.One;

            [Inline(InlineBehavior.Remove)]
            static Vector<byte> DoOperation(in Vector<byte> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => Normalize(sourceVector),
                    UnaryOperatorType.Not => Vector.Equals(sourceVector, Vector<byte>.Zero),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif