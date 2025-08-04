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
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, T* ptrEnd, UnaryOperationMethod method)
            {
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedUnaryOperationCore(valueVector, method));
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyUnaryOperationCore(ref ptr, ptrEnd, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedUnaryOperationCore(in Vector<T> valueVector,
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
