#if NET8_0_OR_GREATER
using System;
using System.Runtime.Intrinsics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        partial class FastCore<T>
        {
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedCompare(in Vector512<T> sourceVector, in Vector512<T> valueVector, [InlineParameter] CompareMethod method)
            => method switch
            {
                CompareMethod.Include => Vector512.Equals(sourceVector, valueVector),
                CompareMethod.Exclude => ~Vector512.Equals(sourceVector, valueVector),
                CompareMethod.GreaterThan => Vector512.GreaterThan(sourceVector, valueVector),
                CompareMethod.GreaterThanOrEquals => Vector512.GreaterThanOrEqual(sourceVector, valueVector),
                CompareMethod.LessThan => Vector512.LessThan(sourceVector, valueVector),
                CompareMethod.LessThanOrEquals => Vector512.LessThanOrEqual(sourceVector, valueVector),
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedCompare(in Vector256<T> sourceVector, in Vector256<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector256.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector256.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector256.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector256.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector256.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector256.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedCompare(in Vector128<T> sourceVector, in Vector128<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector128.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector128.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector128.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector128.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector128.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector128.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedCompare(in Vector64<T> sourceVector, in Vector64<T> valueVector, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => Vector64.Equals(sourceVector, valueVector),
                    CompareMethod.Exclude => ~Vector64.Equals(sourceVector, valueVector),
                    CompareMethod.GreaterThan => Vector64.GreaterThan(sourceVector, valueVector),
                    CompareMethod.GreaterThanOrEquals => Vector64.GreaterThanOrEqual(sourceVector, valueVector),
                    CompareMethod.LessThan => Vector64.LessThan(sourceVector, valueVector),
                    CompareMethod.LessThanOrEquals => Vector64.LessThanOrEqual(sourceVector, valueVector),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }
    }
}
#endif