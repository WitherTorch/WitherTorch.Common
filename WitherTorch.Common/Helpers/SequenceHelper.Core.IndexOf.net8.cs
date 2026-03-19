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
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method, bool accurateResult)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    return VectorizedPointerIndexOfCore_512(ref ptr, ref length, value, method, accurateResult);
                if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    return VectorizedPointerIndexOfCore_256(ref ptr, ref length, value, method, accurateResult);
                if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    return VectorizedPointerIndexOfCore_128(ref ptr, ref length, value, method, accurateResult);
                if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    return VectorizedPointerIndexOfCore_64(ref ptr, ref length, value, method, accurateResult);
                throw new InvalidOperationException("Unreachable branch!");
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_512(ref T* ptr, ref nuint length, T value,
                           [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                Vector512<T> valueVector = Vector512.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector512<T> sourceVector = Vector512.Load(ptr);
                    Vector512<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector512.EqualsAll(resultVector, Vector512<T>.Zero))
                    {
                        if (length > (nuint)Vector512<T>.Count * 2)
                        {
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector512<T>.Count;
                            length -= (nuint)Vector512<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector512.EqualsAll(resultVector, Vector512<T>.Zero))
                    {
                        ptr += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        continue;
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<T>.Count;
                    Vector512<T> sourceVector = Vector512.Load(ptr);
                    Vector512<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector512.EqualsAll(resultVector, Vector512<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_256(ref T* ptr, ref nuint length, T value,
                           [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                Vector256<T> valueVector = Vector256.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector256<T> sourceVector = Vector256.Load(ptr);
                    Vector256<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector256.EqualsAll(resultVector, Vector256<T>.Zero))
                    {
                        if (length > (nuint)Vector256<T>.Count * 2)
                        {
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector256<T>.Count;
                            length -= (nuint)Vector256<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector256.EqualsAll(resultVector, Vector256<T>.Zero))
                    {
                        ptr += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        continue;
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<T>.Count;
                    Vector256<T> sourceVector = Vector256.Load(ptr);
                    Vector256<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector256.EqualsAll(resultVector, Vector256<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_128(ref T* ptr, ref nuint length, T value,
                           [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                Vector128<T> valueVector = Vector128.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector128<T> sourceVector = Vector128.Load(ptr);
                    Vector128<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector128.EqualsAll(resultVector, Vector128<T>.Zero))
                    {
                        if (length > (nuint)Vector128<T>.Count * 2)
                        {
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector128<T>.Count;
                            length -= (nuint)Vector128<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector128.EqualsAll(resultVector, Vector128<T>.Zero))
                    {
                        ptr += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        continue;
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<T>.Count;
                    Vector128<T> sourceVector = Vector128.Load(ptr);
                    Vector128<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector128.EqualsAll(resultVector, Vector128<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_64(ref T* ptr, ref nuint length, T value,
                [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                Vector64<T> valueVector = Vector64.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector64<T> sourceVector = Vector64.Load(ptr);
                    Vector64<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector64.EqualsAll(resultVector, Vector64<T>.Zero))
                    {
                        if (length > (nuint)Vector64<T>.Count * 2)
                        {
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector64<T>.Count;
                            length -= (nuint)Vector64<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector64.EqualsAll(resultVector, Vector64<T>.Zero))
                    {
                        ptr += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        continue;
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<T>.Count;
                    Vector64<T> sourceVector = Vector64.Load(ptr);
                    Vector64<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector64.EqualsAll(resultVector, Vector64<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            private static partial void VectorizedReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, CompareMethod method)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedReplaceCore_512(ref ptr, ref length, filter, replacement, method);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedReplaceCore_256(ref ptr, ref length, filter, replacement, method);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedReplaceCore_128(ref ptr, ref length, filter, replacement, method);
                else if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    VectorizedReplaceCore_64(ref ptr, ref length, filter, replacement, method);
                else
                    throw new InvalidOperationException("Unreachable branch!");
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_512(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector512<T> filterVector = Vector512.Create(filter);
                Vector512<T> replacementVector = Vector512.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    if (length > (nuint)Vector512<T>.Count * 2)
                    {
                        ScalarizedReplaceCore(ref ptr, ref headRemainder, filter, replacement, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector512<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector512<T>.Count;
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        Vector512<T> sourceVector2 = Vector512.Load(ptr2);
                        Vector512.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        Vector512.ConditionalSelect(condition: VectorizedCompareCore(sourceVector2, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector2).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector512<T> sourceVector = Vector512.Load(ptr);
                        Vector512.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        ptr += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                    left: replacementVector,
                                                    right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_256(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector256<T> filterVector = Vector256.Create(filter);
                Vector256<T> replacementVector = Vector256.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    if (length > (nuint)Vector256<T>.Count * 2)
                    {
                        ScalarizedReplaceCore(ref ptr, ref headRemainder, filter, replacement, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector256<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector256<T>.Count;
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        Vector256<T> sourceVector2 = Vector256.Load(ptr2);
                        Vector256.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        Vector256.ConditionalSelect(condition: VectorizedCompareCore(sourceVector2, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector2).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector256<T> sourceVector = Vector256.Load(ptr);
                        Vector256.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        ptr += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                    left: replacementVector,
                                                    right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_128(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector128<T> filterVector = Vector128.Create(filter);
                Vector128<T> replacementVector = Vector128.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    if (length > (nuint)Vector128<T>.Count * 2)
                    {
                        ScalarizedReplaceCore(ref ptr, ref headRemainder, filter, replacement, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector128<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector128<T>.Count;
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        Vector128<T> sourceVector2 = Vector128.Load(ptr2);
                        Vector128.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        Vector128.ConditionalSelect(condition: VectorizedCompareCore(sourceVector2, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector2).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector128<T> sourceVector = Vector128.Load(ptr);
                        Vector128.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        ptr += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                    left: replacementVector,
                                                    right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_64(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector64<T> filterVector = Vector64.Create(filter);
                Vector64<T> replacementVector = Vector64.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    if (length > (nuint)Vector64<T>.Count * 2)
                    {
                        ScalarizedReplaceCore(ref ptr, ref headRemainder, filter, replacement, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector64<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector64<T>.Count;
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        Vector64<T> sourceVector2 = Vector64.Load(ptr2);
                        Vector64.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        Vector64.ConditionalSelect(condition: VectorizedCompareCore(sourceVector2, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector2).Store(ptr2);
                        return;
                    }
                    else
                    {
                        Vector64<T> sourceVector = Vector64.Load(ptr);
                        Vector64.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector).Store(ptr);
                        ptr += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64.ConditionalSelect(condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                    left: replacementVector,
                                                    right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedCompareCore(in Vector512<T> sourceVector, in Vector512<T> valueVector, [InlineParameter] CompareMethod method)
            => method switch
            {
                CompareMethod.Include => Vector512.Equals(sourceVector, valueVector),
                CompareMethod.Exclude => ~Vector512.Equals(sourceVector, valueVector),
                CompareMethod.GreaterThan => Vector512.GreaterThan(sourceVector, valueVector),
                CompareMethod.GreaterThanOrEquals => Vector512.GreaterThanOrEqual(sourceVector, valueVector),
                CompareMethod.LessThan => Vector512.LessThan(sourceVector, valueVector),
                CompareMethod.LessThanOrEquals => Vector512.LessThanOrEqual(sourceVector, valueVector),
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedCompareCore(in Vector256<T> sourceVector, in Vector256<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector256.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector256.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector256.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector256.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector256.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector256.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedCompareCore(in Vector128<T> sourceVector, in Vector128<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector128.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector128.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector128.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector128.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector128.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector128.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedCompareCore(in Vector64<T> sourceVector, in Vector64<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector64.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector64.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector64.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector64.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector64.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector64.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif