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
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, T* ptrEnd, UnaryOperationMethod method)
            {
                if (Limits.UseVector512())
                {
                    Vector512<T>* ptrLimit = ((Vector512<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            Vector512<T> valueVector = Vector512.Load(ptr);
                            VectorizedUnaryOperationCore_512(valueVector, method).Store(ptr);
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
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            VectorizedUnaryOperationCore_256(valueVector, method).Store(ptr);
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
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            VectorizedUnaryOperationCore_128(valueVector, method).Store(ptr);
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
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            VectorizedUnaryOperationCore_64(valueVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyUnaryOperationCore(ref ptr, ptrEnd, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedUnaryOperationCore_512(in Vector512<T> valueVector,
            [InlineParameter] UnaryOperationMethod method)
            => method switch
            {
                UnaryOperationMethod.Not => ~valueVector,
                _ => throw new InvalidOperationException(),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedUnaryOperationCore_256(in Vector256<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedUnaryOperationCore_128(in Vector128<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedUnaryOperationCore_64(in Vector64<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };
        }
    }
}
#endif
