#if NET472_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            Vector<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
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

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedIndexOfCore(in Vector<T> valueVector, in Vector<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector(in Vector<T> vector)
            {
                ulong* ptrVector = (ulong*)UnsafeHelper.AsPointerIn(in vector);
                for (int i = 0; i < Vector<ulong>.Count; i++)
                {
                    int result = MathHelper.TrailingZeroCount(ptrVector[i]);
                    if (result == sizeof(ulong) * 8)
                        continue;
                    return i * (sizeof(ulong) / sizeof(T)) + result / sizeof(T) / 8;
                }
                return Vector<T>.Count;
            }
        }
    }
}
#endif