#if NET8_0_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, BinaryOperationMethod method)
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
                            VectorizedBinaryOperationCore_512(valueVector, maskVector, method).Store(ptr);
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
                        Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            VectorizedBinaryOperationCore_256(valueVector, maskVector, method).Store(ptr);
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
                        Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            VectorizedBinaryOperationCore_128(valueVector, maskVector, method).Store(ptr);
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
                        Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            VectorizedBinaryOperationCore_64(valueVector, maskVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyBinaryOperationCore(ref ptr, ptrEnd, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedBinaryOperationCore_512(in Vector512<T> valueVector, in Vector512<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedBinaryOperationCore_256(in Vector256<T> valueVector, in Vector256<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedBinaryOperationCore_128(in Vector128<T> valueVector, in Vector128<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedBinaryOperationCore_64(in Vector64<T> valueVector, in Vector64<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };
        }
    }
}
#endif