#if NET472_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method, bool accurateResult)
            {
                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
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
                    Vector<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
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
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector<T>.Count;
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompareCore(sourceVector, valueVector, method);
                    if (Vector.EqualsAll(resultVector, Vector<T>.Zero))
                        return null;
                    return accurateResult ? ptr + MathHelper.TrailingZeroCount(resultVector.ExtractMostSignificantBits()) : (T*)Booleans.TrueNative;
                }
                else
                    return null;
            }

            private static partial void VectorizedReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, CompareMethod method)
            {
                Vector<T> filterVector = new Vector<T>(filter);
                Vector<T> replacementVector = new Vector<T>(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                    if (length > (nuint)Vector<T>.Count * 2)
                    {
                        ScalarizedReplaceCore(ref ptr, ref headRemainder, filter, replacement, method);
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else if (length == (nuint)Vector<T>.Count * 2)
                    {
                        T* ptr2 = ptr + Vector<T>.Count;
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        Vector<T> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr2);
                        UnsafeHelper.WriteUnaligned(ptr, Vector.ConditionalSelect(
                                                        condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector));
                        UnsafeHelper.WriteUnaligned(ptr2, Vector.ConditionalSelect(
                                                        condition: VectorizedCompareCore(sourceVector2, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector2));
                        return;
                    }
                    else
                    {
                        Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                        UnsafeHelper.WriteUnaligned(ptr, Vector.ConditionalSelect(
                                                        condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                        left: replacementVector,
                                                        right: sourceVector));
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    UnsafeHelper.WriteUnaligned(ptr, Vector.ConditionalSelect(
                                                    condition: VectorizedCompareCore(sourceVector, filterVector, method),
                                                    left: replacementVector,
                                                    right: sourceVector));
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedCompareCore(in Vector<T> sourceVector, in Vector<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif