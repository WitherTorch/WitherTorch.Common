#if NET472_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Intrinsics;
using WitherTorch.Common.Intrinsics.X86;

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

            private static partial void VectorizedReplaceCore(ref T* ptr, T* ptrEnd, T filter, T replacement, IndexOfMethod method)
            {
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector<T> filterVector = new Vector<T>(filter); // 將要比對的項目擴充成向量
                        Vector<T> replaceVector = new Vector<T>(replacement);
                        do
                        {
                            Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            sourceVector = Vector.ConditionalSelect(
                                condition: VectorizedIndexOfCore(sourceVector, filterVector, method),
                                left: replaceVector,
                                right: sourceVector);
                            UnsafeHelper.WriteUnaligned(ptr, sourceVector);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                LegacyReplaceCore(ref ptr, ptrEnd, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedIndexOfCore(in Vector<T> valueVector, in Vector<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterThanOrEquals => Vector.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessThanOrEquals => Vector.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector(in Vector<T> vector)
                => sizeof(Vector<T>) switch
                {
                    M128.SizeInBytes => FindIndexForResultVector_128(vector),
                    M256.SizeInBytes => FindIndexForResultVector_256(vector),
                    M512.SizeInBytes => FindIndexForResultVector_512(vector),
                    _ => FindIndexForResultVectorFallback(vector)
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector_128(in Vector<T> vector)
            {
                ulong* ptrVector = (ulong*)UnsafeHelper.AsPointerIn(in vector);

                int result = MathHelper.TrailingZeroCount(ptrVector[0]);
                if (result < 64)
                    return result / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[1]);
                if (result < 64)
                    return (64 + result) / (sizeof(T) * 8);

                return Vector<T>.Count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector_256(in Vector<T> vector)
            {
                ulong* ptrVector = (ulong*)UnsafeHelper.AsPointerIn(in vector);

                int result = MathHelper.TrailingZeroCount(ptrVector[0]);
                if (result < 64)
                    return result / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[1]);
                if (result < 64)
                    return (64 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[2]);
                if (result < 64)
                    return (64 * 2 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[3]);
                if (result < 64)
                    return (64 * 3 + result) / (sizeof(T) * 8);

                return Vector<T>.Count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector_512(in Vector<T> vector)
            {
                ulong* ptrVector = (ulong*)UnsafeHelper.AsPointerIn(in vector);

                int result = MathHelper.TrailingZeroCount(ptrVector[0]);
                if (result < 64)
                    return result / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[1]);
                if (result < 64)
                    return (64 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[2]);
                if (result < 64)
                    return (64 * 2 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[3]);
                if (result < 64)
                    return (64 * 3 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[4]);
                if (result < 64)
                    return (64 * 4 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[5]);
                if (result < 64)
                    return (64 * 5 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[6]);
                if (result < 64)
                    return (64 * 6 + result) / (sizeof(T) * 8);

                result = MathHelper.TrailingZeroCount(ptrVector[7]);
                if (result < 64)
                    return (64 * 7 + result) / (sizeof(T) * 8);

                return Vector<T>.Count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int FindIndexForResultVectorFallback(in Vector<T> vector)
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