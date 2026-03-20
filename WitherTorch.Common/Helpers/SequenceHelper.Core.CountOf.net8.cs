#if NET8_0_OR_GREATER
using System;
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial nuint VectorizedCountOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method)
            {
                if (Limits.UseVector512() && length >= (nuint)Vector512<T>.Count)
                    return VectorizedCountOfCore_512(ref ptr, ref length, value, method);
                if (Limits.UseVector256() && length >= (nuint)Vector256<T>.Count)
                    return VectorizedCountOfCore_256(ref ptr, ref length, value, method);
                if (Limits.UseVector128() && length >= (nuint)Vector128<T>.Count)
                    return VectorizedCountOfCore_128(ref ptr, ref length, value, method);
                if (Limits.UseVector64() && length >= (nuint)Vector64<T>.Count)
                    return VectorizedCountOfCore_64(ref ptr, ref length, value, method);
                throw new InvalidOperationException("Unreachable branch!");
            }

            [Inline(InlineBehavior.Remove)]
            private static nuint VectorizedCountOfCore_512(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;

                Vector512<T> valueVector512 = Vector512.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector512<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector512<T> sourceVector512 = Vector512.Load(ptr);
                    Vector512<T> resultVector512 = VectorizedCompare(sourceVector512, valueVector512, method);
                    if (length > (nuint)Vector512<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector512<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        counter += (nuint)MathHelper.PopCount(resultVector512.ExtractMostSignificantBits() & ((1UL << (int)headRemainder) - 1));
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        counter += (nuint)MathHelper.PopCount(resultVector512.ExtractMostSignificantBits());
                        ptr += (nuint)Vector512<T>.Count;
                        length -= (nuint)Vector512<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector512<T> sourceVector512 = Vector512.LoadAligned(ptr);
                    Vector512<T> resultVector512 = VectorizedCompare(sourceVector512, valueVector512, method);
                    counter += (nuint)MathHelper.PopCount(resultVector512.ExtractMostSignificantBits());
                    ptr += (nuint)Vector512<T>.Count;
                    length -= (nuint)Vector512<T>.Count;
                } while (length >= (nuint)Vector512<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    nuint tailOverlapOffset = (nuint)Vector512<T>.Count - length;
                    Vector512<T> sourceVector512 = Vector512.Load(ptr - tailOverlapOffset);
                    Vector512<T> resultVector512 = VectorizedCompare(sourceVector512, valueVector512, method);
                    counter += (nuint)MathHelper.PopCount(resultVector512.ExtractMostSignificantBits() & ~((1UL << (int)tailOverlapOffset) - 1));
                }
                return counter;
            }

            [Inline(InlineBehavior.Remove)]
            private static nuint VectorizedCountOfCore_256(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;

                Vector256<T> valueVector256 = Vector256.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector256<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector256<T> sourceVector256 = Vector256.Load(ptr);
                    Vector256<T> resultVector256 = VectorizedCompare(sourceVector256, valueVector256, method);
                    if (length > (nuint)Vector256<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector256<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        counter += (nuint)MathHelper.PopCount(resultVector256.ExtractMostSignificantBits() & ((1UL << (int)headRemainder) - 1));
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        counter += (nuint)MathHelper.PopCount(resultVector256.ExtractMostSignificantBits());
                        ptr += (nuint)Vector256<T>.Count;
                        length -= (nuint)Vector256<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector256<T> sourceVector256 = Vector256.LoadAligned(ptr);
                    Vector256<T> resultVector256 = VectorizedCompare(sourceVector256, valueVector256, method);
                    counter += (nuint)MathHelper.PopCount(resultVector256.ExtractMostSignificantBits());
                    ptr += (nuint)Vector256<T>.Count;
                    length -= (nuint)Vector256<T>.Count;
                } while (length >= (nuint)Vector256<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    nuint tailOverlapOffset = (nuint)Vector256<T>.Count - length;
                    Vector256<T> sourceVector256 = Vector256.Load(ptr - tailOverlapOffset);
                    Vector256<T> resultVector256 = VectorizedCompare(sourceVector256, valueVector256, method);
                    counter += (nuint)MathHelper.PopCount(resultVector256.ExtractMostSignificantBits() & ~((1UL << (int)tailOverlapOffset) - 1));
                }
                return counter;
            }

            [Inline(InlineBehavior.Remove)]
            private static nuint VectorizedCountOfCore_128(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;

                Vector128<T> valueVector128 = Vector128.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector128<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector128<T> sourceVector128 = Vector128.Load(ptr);
                    Vector128<T> resultVector128 = VectorizedCompare(sourceVector128, valueVector128, method);
                    if (length > (nuint)Vector128<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector128<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        counter += (nuint)MathHelper.PopCount(resultVector128.ExtractMostSignificantBits() & ((1UL << (int)headRemainder) - 1));
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        counter += (nuint)MathHelper.PopCount(resultVector128.ExtractMostSignificantBits());
                        ptr += (nuint)Vector128<T>.Count;
                        length -= (nuint)Vector128<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector128<T> sourceVector128 = Vector128.LoadAligned(ptr);
                    Vector128<T> resultVector128 = VectorizedCompare(sourceVector128, valueVector128, method);
                    counter += (nuint)MathHelper.PopCount(resultVector128.ExtractMostSignificantBits());
                    ptr += (nuint)Vector128<T>.Count;
                    length -= (nuint)Vector128<T>.Count;
                } while (length >= (nuint)Vector128<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    nuint tailOverlapOffset = (nuint)Vector128<T>.Count - length;
                    Vector128<T> sourceVector128 = Vector128.Load(ptr - tailOverlapOffset);
                    Vector128<T> resultVector128 = VectorizedCompare(sourceVector128, valueVector128, method);
                    counter += (nuint)MathHelper.PopCount(resultVector128.ExtractMostSignificantBits() & ~((1UL << (int)tailOverlapOffset) - 1));
                }
                return counter;
            }

            [Inline(InlineBehavior.Remove)]
            private static nuint VectorizedCountOfCore_64(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;

                Vector64<T> valueVector64 = Vector64.Create(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector64<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector64<T> sourceVector64 = Vector64.Load(ptr);
                    Vector64<T> resultVector64 = VectorizedCompare(sourceVector64, valueVector64, method);
                    if (length > (nuint)Vector64<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector64<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        counter += (nuint)MathHelper.PopCount(resultVector64.ExtractMostSignificantBits() & ((1UL << (int)headRemainder) - 1));
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        counter += (nuint)MathHelper.PopCount(resultVector64.ExtractMostSignificantBits());
                        ptr += (nuint)Vector64<T>.Count;
                        length -= (nuint)Vector64<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector64<T> sourceVector64 = Vector64.LoadAligned(ptr);
                    Vector64<T> resultVector64 = VectorizedCompare(sourceVector64, valueVector64, method);
                    counter += (nuint)MathHelper.PopCount(resultVector64.ExtractMostSignificantBits());
                    ptr += (nuint)Vector64<T>.Count;
                    length -= (nuint)Vector64<T>.Count;
                } while (length >= (nuint)Vector64<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    nuint tailOverlapOffset = (nuint)Vector64<T>.Count - length;
                    Vector64<T> sourceVector64 = Vector64.Load(ptr - tailOverlapOffset);
                    Vector64<T> resultVector64 = VectorizedCompare(sourceVector64, valueVector64, method);
                    counter += (nuint)MathHelper.PopCount(resultVector64.ExtractMostSignificantBits() & ~((1UL << (int)tailOverlapOffset) - 1));
                }
                return counter;
            }
        }
    }
}
#endif