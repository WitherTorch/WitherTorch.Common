#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.Intrinsics;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, T* ptrEnd, T value, IndexOfMethod method, bool accurateResult)
            {
                if (Limits.UseVector512())
                {
                    Vector512<T>* ptrLimit = ((Vector512<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector512<T> maskVector = Vector512.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector512<T> valueVector = Vector512.Load(ptr);
                            Vector512<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
                            if (!resultVector.Equals(default))
                                return accurateResult ? ptr + FindIndexForResultVector(resultVector) : (T*)Booleans.TrueNative;
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Limits.UseVector256())
                {
                    Vector256<T>* ptrLimit = ((Vector256<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            Vector256<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
                            if (!resultVector.Equals(default))
                                return accurateResult ? ptr + FindIndexForResultVector(resultVector) : (T*)Booleans.TrueNative;
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Limits.UseVector128())
                {
                    Vector128<T>* ptrLimit = ((Vector128<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            Vector128<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
                            if (!resultVector.Equals(default))
                                return accurateResult ? ptr + FindIndexForResultVector(resultVector) : (T*)Booleans.TrueNative;
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Limits.UseVector64())
                {
                    Vector64<T>* ptrLimit = ((Vector64<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            Vector64<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
                            if (!resultVector.Equals(default))
                                return accurateResult ? ptr + FindIndexForResultVector(resultVector) : (T*)Booleans.TrueNative;
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                return LegacyPointerIndexOfCore(ref ptr, ptrEnd, value, method, accurateResult);
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
            private static Vector512<T> VectorizedIndexOfCore(in Vector512<T> valueVector, in Vector512<T> maskVector, [InlineParameter] IndexOfMethod method)
            => method switch
            {
                IndexOfMethod.Include => Vector512.Equals(valueVector, maskVector),
                IndexOfMethod.Exclude => ~Vector512.Equals(valueVector, maskVector),
                IndexOfMethod.GreaterThan => Vector512.GreaterThan(valueVector, maskVector),
                IndexOfMethod.GreaterOrEqualsThan => Vector512.GreaterThanOrEqual(valueVector, maskVector),
                IndexOfMethod.LessThan => Vector512.LessThan(valueVector, maskVector),
                IndexOfMethod.LessOrEqualsThan => Vector512.LessThanOrEqual(valueVector, maskVector),
                _ => throw new InvalidOperationException(),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedIndexOfCore(in Vector256<T> valueVector, in Vector256<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector256.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector256.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector256.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector256.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector256.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector256.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedIndexOfCore(in Vector128<T> valueVector, in Vector128<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector128.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector128.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector128.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector128.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector128.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector128.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedIndexOfCore(in Vector64<T> valueVector, in Vector64<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector64.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector64.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector64.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector64.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector64.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector64.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector(in Vector512<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector(in Vector256<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector(in Vector128<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector(in Vector64<T> vector)
                => MathHelper.TrailingZeroCount(*(ulong*)UnsafeHelper.AsPointerIn(in vector)) / sizeof(T) / 8;
        }
    }
}
#endif