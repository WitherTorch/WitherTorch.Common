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
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, IndexOfMethod method, bool accurateResult)
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
                [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                Vector512<T> valueVector = Vector512.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector512<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector512<T>>(ptr);
                    Vector512<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector512<T> sourceVector = UnsafeHelper.Read<Vector512<T>>(ptr);
                    Vector512<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector512<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector512<T>>(ptr);
                    Vector512<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector512.EqualsAll(resultVector, Vector512<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_256(ref T* ptr, ref nuint length, T value,
                [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                Vector256<T> valueVector = Vector256.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector256<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector256<T>>(ptr);
                    Vector256<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector256<T> sourceVector = UnsafeHelper.Read<Vector256<T>>(ptr);
                    Vector256<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector256<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector256<T>>(ptr);
                    Vector256<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector256.EqualsAll(resultVector, Vector256<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_128(ref T* ptr, ref nuint length, T value,
                [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                Vector128<T> valueVector = Vector128.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector128<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector128<T>>(ptr);
                    Vector128<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector128<T> sourceVector = UnsafeHelper.Read<Vector128<T>>(ptr);
                    Vector128<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector128<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector128<T>>(ptr);
                    Vector128<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector128.EqualsAll(resultVector, Vector128<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore_64(ref T* ptr, ref nuint length, T value,
                [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                Vector64<T> valueVector = Vector64.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector64<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector64<T>>(ptr);
                    Vector64<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector64<T> sourceVector = UnsafeHelper.Read<Vector64<T>>(ptr);
                    Vector64<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
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
                    Vector64<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector64<T>>(ptr);
                    Vector64<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector64.EqualsAll(resultVector, Vector64<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }

            private static partial void VectorizedReplaceCore(ref T* ptr, T* ptrEnd, T filter, T replacement, IndexOfMethod method)
            {
                if (Limits.UseVector512())
                {
                    Vector512<T>* ptrLimit = ((Vector512<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector512<T> filterVector = Vector512.Create(filter); // 將要比對的項目擴充成向量
                        Vector512<T> replaceVector = Vector512.Create(replacement);
                        do
                        {
                            Vector512<T> sourceVector = Vector512.Load(ptr);
                            Vector512.ConditionalSelect(
                                condition: VectorizedIndexOfCore(sourceVector, filterVector, method),
                                left: replaceVector,
                                right: sourceVector).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector256())
                {
                    Vector256<T>* ptrLimit = ((Vector256<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector256<T> filterVector = Vector256.Create(filter); // 將要比對的項目擴充成向量
                        Vector256<T> replaceVector = Vector256.Create(replacement);
                        do
                        {
                            Vector256<T> sourceVector = Vector256.Load(ptr);
                            Vector256.ConditionalSelect(
                                condition: VectorizedIndexOfCore(sourceVector, filterVector, method),
                                left: replaceVector,
                                right: sourceVector).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector128())
                {
                    Vector128<T>* ptrLimit = ((Vector128<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector128<T> filterVector = Vector128.Create(filter); // 將要比對的項目擴充成向量
                        Vector128<T> replaceVector = Vector128.Create(replacement);
                        do
                        {
                            Vector128<T> sourceVector = Vector128.Load(ptr);
                            Vector128.ConditionalSelect(
                                condition: VectorizedIndexOfCore(sourceVector, filterVector, method),
                                left: replaceVector,
                                right: sourceVector).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector64())
                {
                    Vector64<T>* ptrLimit = ((Vector64<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector64<T> filterVector = Vector64.Create(filter); // 將要比對的項目擴充成向量
                        Vector64<T> replaceVector = Vector64.Create(replacement);
                        do
                        {
                            Vector64<T> sourceVector = Vector64.Load(ptr);
                            Vector64.ConditionalSelect(
                                condition: VectorizedIndexOfCore(sourceVector, filterVector, method),
                                left: replaceVector,
                                right: sourceVector).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyReplaceCore(ref ptr, ptrEnd, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedIndexOfCore(in Vector512<T> sourceVector, in Vector512<T> valueVector, [InlineParameter] IndexOfMethod method)
            => method switch
            {
                IndexOfMethod.Include => Vector512.Equals(sourceVector, valueVector),
                IndexOfMethod.Exclude => ~Vector512.Equals(sourceVector, valueVector),
                IndexOfMethod.GreaterThan => Vector512.GreaterThan(sourceVector, valueVector),
                IndexOfMethod.GreaterThanOrEquals => Vector512.GreaterThanOrEqual(sourceVector, valueVector),
                IndexOfMethod.LessThan => Vector512.LessThan(sourceVector, valueVector),
                IndexOfMethod.LessThanOrEquals => Vector512.LessThanOrEqual(sourceVector, valueVector),
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedIndexOfCore(in Vector256<T> sourceVector, in Vector256<T> valueVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector256.Equals(sourceVector, valueVector),
                    IndexOfMethod.Exclude => ~Vector256.Equals(sourceVector, valueVector),
                    IndexOfMethod.GreaterThan => Vector256.GreaterThan(sourceVector, valueVector),
                    IndexOfMethod.GreaterThanOrEquals => Vector256.GreaterThanOrEqual(sourceVector, valueVector),
                    IndexOfMethod.LessThan => Vector256.LessThan(sourceVector, valueVector),
                    IndexOfMethod.LessThanOrEquals => Vector256.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedIndexOfCore(in Vector128<T> sourceVector, in Vector128<T> valueVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector128.Equals(sourceVector, valueVector),
                    IndexOfMethod.Exclude => ~Vector128.Equals(sourceVector, valueVector),
                    IndexOfMethod.GreaterThan => Vector128.GreaterThan(sourceVector, valueVector),
                    IndexOfMethod.GreaterThanOrEquals => Vector128.GreaterThanOrEqual(sourceVector, valueVector),
                    IndexOfMethod.LessThan => Vector128.LessThan(sourceVector, valueVector),
                    IndexOfMethod.LessThanOrEquals => Vector128.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedIndexOfCore(in Vector64<T> sourceVector, in Vector64<T> valueVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector64.Equals(sourceVector, valueVector),
                    IndexOfMethod.Exclude => ~Vector64.Equals(sourceVector, valueVector),
                    IndexOfMethod.GreaterThan => Vector64.GreaterThan(sourceVector, valueVector),
                    IndexOfMethod.GreaterThanOrEquals => Vector64.GreaterThanOrEqual(sourceVector, valueVector),
                    IndexOfMethod.LessThan => Vector64.LessThan(sourceVector, valueVector),
                    IndexOfMethod.LessThanOrEquals => Vector64.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif