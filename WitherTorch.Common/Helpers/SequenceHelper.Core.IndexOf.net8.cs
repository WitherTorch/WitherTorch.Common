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
                    Vector512<T> sourceVector = Vector512.Load(ptr);
                    Vector512<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector512<T>.Zero)
                    {
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
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector512<T>.Zero)
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
                    Vector512<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector512<T>.Zero)
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
                    Vector256<T> sourceVector = Vector256.Load(ptr);
                    Vector256<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector256<T>.Zero)
                    {
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
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector256<T>.Zero)
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
                    Vector256<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector256<T>.Zero)
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
                    Vector128<T> sourceVector = Vector128.Load(ptr);
                    Vector128<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector128<T>.Zero)
                    {
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
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector128<T>.Zero)
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
                    Vector128<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector128<T>.Zero)
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
                    Vector64<T> sourceVector = Vector64.Load(ptr);
                    Vector64<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector64<T>.Zero)
                    {
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
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector64<T>.Zero)
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
                    Vector64<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (resultVector == Vector64<T>.Zero)
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                return null;
            }
        }
    }
}
#endif