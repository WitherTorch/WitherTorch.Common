#if NET472_OR_GREATER
using System;
using System.Numerics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, BinaryOperationMethod method)
            {
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperationCore(valueVector, maskVector, method));
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyBinaryOperationCore(ref ptr, ptrEnd, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedBinaryOperationCore(in Vector<T> valueVector, in Vector<T> maskVector,
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

