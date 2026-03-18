#if NET472_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Intrinsics;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, IndexOfMethod method, bool accurateResult)
            {
                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector.EqualsAll(resultVector, Vector<T>.Zero))
                    {
                        if (length > (nuint)Vector<T>.Count * 2)
                        {
                            ptr += headRemainder;
                            length -= headRemainder;
                            goto VectorizedLoop;
                        }
                        else
                        {
                            ptr += (nuint)Vector<T>.Count;
                            length -= (nuint)Vector<T>.Count;
                            goto TailProcess;
                        }
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector.EqualsAll(resultVector, Vector<T>.Zero))
                    {
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        continue;
                    }
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length >= (nuint)Vector<T>.Count / 2)
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedIndexOfCore(sourceVector, valueVector, method);
                    if (Vector.EqualsAll(resultVector, Vector<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                else
                    return LegacyPointerIndexOfCore(ref ptr, length, value, method, accurateResult);
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
            private static Vector<T> VectorizedIndexOfCore(in Vector<T> sourceVector, in Vector<T> valueVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector.Equals(sourceVector, valueVector),
                    IndexOfMethod.Exclude => ~Vector.Equals(sourceVector, valueVector),
                    IndexOfMethod.GreaterThan => Vector.GreaterThan(sourceVector, valueVector),
                    IndexOfMethod.GreaterThanOrEquals => Vector.GreaterThanOrEqual(sourceVector, valueVector),
                    IndexOfMethod.LessThan => Vector.LessThan(sourceVector, valueVector),
                    IndexOfMethod.LessThanOrEquals => Vector.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new InvalidOperationException(),
                };
        }
    }
}
#endif