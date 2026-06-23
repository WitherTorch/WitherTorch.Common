#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Helpers;

partial class SequenceHelper
{
    unsafe partial class FastCore<T>
    {
        private static partial Unit VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, UnaryOperatorType type)
        {
            if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                VectorizedUnaryOperationCore_512(ref ptr, ref length, type);
            else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                VectorizedUnaryOperationCore_256(ref ptr, ref length, type);
            else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                VectorizedUnaryOperationCore_128(ref ptr, ref length, type);
            else
                VectorizedUnaryOperationCore_64(ref ptr, ref length, type);
            return ScalarizedUnaryOperationCore(ref ptr, ref length, type);
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

    unsafe partial class FastCoreOfBoolean
    {
        private static partial Unit VectorizedUnaryOperationCore(ref bool* ptr, ref nuint length, UnaryOperatorType type)
        {
            if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                VectorizedUnaryOperationCore_512(ref ptr, ref length, type);
            else if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                VectorizedUnaryOperationCore_256(ref ptr, ref length, type);
            else if (Limits.UseVector128() && length >= (nuint)Vector128<bool>.Count)
                VectorizedUnaryOperationCore_128(ref ptr, ref length, type);
            else
                VectorizedUnaryOperationCore_64(ref ptr, ref length, type);
            return FastCore.IsIdempotence(type) ? Unit.Default : ScalarizedUnaryOperationCore(ref ptr, ref length, type);
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedUnaryOperationCore_512(ref bool* ptr, ref nuint length, [InlineParameter] UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<byte>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                    DoOperation(sourceVector, type).Store((byte*)ptr);
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector512<byte>.Count * 2)
                    {
                        bool* ptr2 = ptr + Vector512<byte>.Count;
                        Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                        Vector512<byte> sourceVector2 = Vector512.Load((byte*)ptr2);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        DoOperation(sourceVector2, type).Store((byte*)ptr2);
                    }
                    else
                    {
                        Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        ptr += (nuint)Vector512<byte>.Count;
                        length -= (nuint)Vector512<byte>.Count;
                    }
                }
            }

        VectorizedLoop:
            do
            {
                Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                DoOperation(sourceVector, type).StoreAligned((byte*)ptr);
                ptr += (nuint)Vector512<byte>.Count;
                length -= (nuint)Vector512<byte>.Count;
            } while (length >= (nuint)Vector512<byte>.Count);

        TailProcess_Special:
            if (length > 0)
            {
                ptr = ptr + length - (nuint)Vector512<byte>.Count;
                Vector512<byte> sourceVector = Vector512.Load((byte*)ptr);
                DoOperation(sourceVector, type).Store((byte*)ptr);
            }

            [Inline(InlineBehavior.Remove)]
            static Vector512<byte> Normalize(in Vector512<byte> sourceVector) => sourceVector & Vector512<byte>.One;

            [Inline(InlineBehavior.Remove)]
            static Vector512<byte> DoOperation(in Vector512<byte> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => Normalize(sourceVector),
                    UnaryOperatorType.Not => Vector512.Equals(sourceVector, Vector512<byte>.Zero),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedUnaryOperationCore_256(ref bool* ptr, ref nuint length, [InlineParameter] UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<byte>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                    DoOperation(sourceVector, type).Store((byte*)ptr);
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector256<byte>.Count * 2)
                    {
                        bool* ptr2 = ptr + Vector256<byte>.Count;
                        Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                        Vector256<byte> sourceVector2 = Vector256.Load((byte*)ptr2);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        DoOperation(sourceVector2, type).Store((byte*)ptr2);
                    }
                    else
                    {
                        Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        ptr += (nuint)Vector256<byte>.Count;
                        length -= (nuint)Vector256<byte>.Count;
                    }
                }
            }

        VectorizedLoop:
            do
            {
                Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                DoOperation(sourceVector, type).StoreAligned((byte*)ptr);
                ptr += (nuint)Vector256<byte>.Count;
                length -= (nuint)Vector256<byte>.Count;
            } while (length >= (nuint)Vector256<byte>.Count);

        TailProcess_Special:
            if (length > 0)
            {
                ptr = ptr + length - (nuint)Vector256<byte>.Count;
                Vector256<byte> sourceVector = Vector256.Load((byte*)ptr);
                DoOperation(sourceVector, type).Store((byte*)ptr);
            }

            [Inline(InlineBehavior.Remove)]
            static Vector256<byte> Normalize(in Vector256<byte> sourceVector) => sourceVector & Vector256<byte>.One;

            [Inline(InlineBehavior.Remove)]
            static Vector256<byte> DoOperation(in Vector256<byte> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => Normalize(sourceVector),
                    UnaryOperatorType.Not => Vector256.Equals(sourceVector, Vector256<byte>.Zero),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedUnaryOperationCore_128(ref bool* ptr, ref nuint length, [InlineParameter] UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<byte>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                    DoOperation(sourceVector, type).Store((byte*)ptr);
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector128<byte>.Count * 2)
                    {
                        bool* ptr2 = ptr + Vector128<byte>.Count;
                        Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                        Vector128<byte> sourceVector2 = Vector128.Load((byte*)ptr2);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        DoOperation(sourceVector2, type).Store((byte*)ptr2);
                    }
                    else
                    {
                        Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        ptr += (nuint)Vector128<byte>.Count;
                        length -= (nuint)Vector128<byte>.Count;
                    }
                }
            }

        VectorizedLoop:
            do
            {
                Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                DoOperation(sourceVector, type).StoreAligned((byte*)ptr);
                ptr += (nuint)Vector128<byte>.Count;
                length -= (nuint)Vector128<byte>.Count;
            } while (length >= (nuint)Vector128<byte>.Count);

        TailProcess_Special:
            if (length > 0)
            {
                ptr = ptr + length - (nuint)Vector128<byte>.Count;
                Vector128<byte> sourceVector = Vector128.Load((byte*)ptr);
                DoOperation(sourceVector, type).Store((byte*)ptr);
            }

            [Inline(InlineBehavior.Remove)]
            static Vector128<byte> Normalize(in Vector128<byte> sourceVector) => sourceVector & Vector128<byte>.One;

            [Inline(InlineBehavior.Remove)]
            static Vector128<byte> DoOperation(in Vector128<byte> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => Normalize(sourceVector),
                    UnaryOperatorType.Not => Vector128.Equals(sourceVector, Vector128<byte>.Zero),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedUnaryOperationCore_64(ref bool* ptr, ref nuint length, [InlineParameter] UnaryOperatorType type)
        {
            nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<byte>>();
            if (headRemainder == 0)
                goto VectorizedLoop;
            else
            {
                if (FastCore.IsIdempotence(type))
                {
                    Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                    DoOperation(sourceVector, type).Store((byte*)ptr);
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
                        ScalarizedUnaryOperationCore(ref ptr, ref headRemainder, type);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector64<byte>.Count * 2)
                    {
                        bool* ptr2 = ptr + Vector64<byte>.Count;
                        Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                        Vector64<byte> sourceVector2 = Vector64.Load((byte*)ptr2);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        DoOperation(sourceVector2, type).Store((byte*)ptr2);
                    }
                    else
                    {
                        Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                        DoOperation(sourceVector, type).Store((byte*)ptr);
                        ptr += (nuint)Vector64<byte>.Count;
                        length -= (nuint)Vector64<byte>.Count;
                    }
                }
            }

        VectorizedLoop:
            do
            {
                Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                DoOperation(sourceVector, type).StoreAligned((byte*)ptr);
                ptr += (nuint)Vector64<byte>.Count;
                length -= (nuint)Vector64<byte>.Count;
            } while (length >= (nuint)Vector64<byte>.Count);

        TailProcess_Special:
            if (length > 0)
            {
                ptr = ptr + length - (nuint)Vector64<byte>.Count;
                Vector64<byte> sourceVector = Vector64.Load((byte*)ptr);
                DoOperation(sourceVector, type).Store((byte*)ptr);
            }

            [Inline(InlineBehavior.Remove)]
            static Vector64<byte> Normalize(in Vector64<byte> sourceVector) => sourceVector & Vector64<byte>.One;

            [Inline(InlineBehavior.Remove)]
            static Vector64<byte> DoOperation(in Vector64<byte> sourceVector, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Identity => Normalize(sourceVector),
                    UnaryOperatorType.Not => Vector64.Equals(sourceVector, Vector64<byte>.Zero),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif
