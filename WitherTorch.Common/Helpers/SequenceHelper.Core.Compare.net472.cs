#if NET472_OR_GREATER
using System;
using System.Numerics;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        partial class FastCore<T>
        {
            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedCompare(in Vector<T> sourceVector, in Vector<T> valueVector, [InlineParameter] CompareMethod method)
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