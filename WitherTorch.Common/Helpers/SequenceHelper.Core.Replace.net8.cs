#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial void VectorizedReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, CompareMethod method)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    VectorizedReplaceCore_512(ref ptr, ref length, filter, replacement, method);
                else if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    VectorizedReplaceCore_256(ref ptr, ref length, filter, replacement, method);
                else if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    VectorizedReplaceCore_128(ref ptr, ref length, filter, replacement, method);
                else
                    VectorizedReplaceCore_64(ref ptr, ref length, filter, replacement, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_512(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector512<T> filterVector = Vector512.Create(filter), replacementVector = Vector512.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector512<T> sourceVector = Vector512.Load(ptr);
                    Vector512.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
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

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector = Vector512.LoadAligned(ptr);
                    Vector512.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                    continue;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector512<T>.Count;
                    Vector512<T> sourceVector = Vector512.Load(ptr);
                    Vector512.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_256(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector256<T> filterVector = Vector256.Create(filter), replacementVector = Vector256.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector256<T> sourceVector = Vector256.Load(ptr);
                    Vector256.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
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

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector = Vector256.LoadAligned(ptr);
                    Vector256.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                    continue;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector256<T>.Count;
                    Vector256<T> sourceVector = Vector256.Load(ptr);
                    Vector256.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_128(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector128<T> filterVector = Vector128.Create(filter), replacementVector = Vector128.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector128<T> sourceVector = Vector128.Load(ptr);
                    Vector128.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
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

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector = Vector128.LoadAligned(ptr);
                    Vector128.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                    continue;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector128<T>.Count;
                    Vector128<T> sourceVector = Vector128.Load(ptr);
                    Vector128.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedReplaceCore_64(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                Vector64<T> filterVector = Vector64.Create(filter), replacementVector = Vector64.Create(replacement);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector64<T> sourceVector = Vector64.Load(ptr);
                    Vector64.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
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

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector = Vector64.LoadAligned(ptr);
                    Vector64.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).StoreAligned(ptr);
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                    continue;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    ptr = ptr + length - (nuint)Vector64<T>.Count;
                    Vector64<T> sourceVector = Vector64.Load(ptr);
                    Vector64.ConditionalSelect(
                        condition: VectorizedCompare(sourceVector, filterVector, method),
                        left: replacementVector,
                        right: sourceVector).Store(ptr);
                }
            }
        }
    }
}
#endif