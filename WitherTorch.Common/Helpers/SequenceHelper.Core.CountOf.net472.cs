#if NET472_OR_GREATER
using System.Numerics;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        unsafe partial class FastCore<T>
        {
            private static partial nuint VectorizedCountOfCore(ref T* ptr, ref nuint length, T value, CompareMethod method)
            {
                nuint counter = 0;

                Vector<T> valueVector = new Vector<T>(value);

                nuint headRemainder = (nuint)ptr % UnsafeHelper.SizeOf<Vector<T>>();
                if (headRemainder == 0)
                    goto VectorizedLoop;
                else
                {
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    if (length > (nuint)Vector<T>.Count * 2)
                    {
                        headRemainder = (UnsafeHelper.SizeOf<Vector<T>>() - headRemainder) / UnsafeHelper.SizeOf<T>(); // 取得數量
                        counter += (nuint)MathHelper.PopCount(resultVector.ExtractMostSignificantBits() & ((1UL << (int)headRemainder) - 1));
                        ptr += headRemainder;
                        length -= headRemainder;
                        goto VectorizedLoop;
                    }
                    else
                    {
                        counter += (nuint)MathHelper.PopCount(resultVector.ExtractMostSignificantBits());
                        ptr += (nuint)Vector<T>.Count;
                        length -= (nuint)Vector<T>.Count;
                        goto TailProcess;
                    }
                }

            VectorizedLoop:
                do
                {
                    Vector<T> sourceVector = UnsafeHelper.Read<Vector<T>>(ptr);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    counter += (nuint)MathHelper.PopCount(resultVector.ExtractMostSignificantBits());
                    ptr += (nuint)Vector<T>.Count;
                    length -= (nuint)Vector<T>.Count;
                } while (length >= (nuint)Vector<T>.Count);
                goto TailProcess;

            TailProcess:
                if (length > 0)
                {
                    nuint tailOverlapOffset = (nuint)Vector<T>.Count - length;
                    Vector<T> sourceVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr - tailOverlapOffset);
                    Vector<T> resultVector = VectorizedCompare(sourceVector, valueVector, method);
                    counter += (nuint)MathHelper.PopCount(resultVector.ExtractMostSignificantBits() & ~((1UL << (int)tailOverlapOffset) - 1));
                }
                return counter;
            }
        }
    }
}
#endif