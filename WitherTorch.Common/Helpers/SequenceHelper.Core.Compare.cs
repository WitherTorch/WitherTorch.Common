using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum CompareMethod
        {
            Include,
            Exclude,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
            _Last
        }

        partial class FastCore<T>
        {
            [Inline(InlineBehavior.Remove)]
            private static bool ScalarizedCompare(T item, T value, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => UnsafeHelper.Equals(item, value),
                    CompareMethod.Exclude => UnsafeHelper.NotEquals(item, value),
                    CompareMethod.GreaterThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanUnsigned(item, value) : UnsafeHelper.IsGreaterThan(item, value),
                    CompareMethod.GreaterThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsGreaterThanOrEquals(item, value),
                    CompareMethod.LessThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanUnsigned(item, value) : UnsafeHelper.IsLessThan(item, value),
                    CompareMethod.LessThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsLessThanOrEquals(item, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        partial class SlowCore<T>
        {
            [Inline(InlineBehavior.Remove)]
            private static bool Compare(EqualityComparer<T> comparer, T item, T value, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => comparer.Equals(item, value),
                    CompareMethod.Exclude => !comparer.Equals(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool Compare(Comparer<T> comparer, T item, T value, [InlineParameter] CompareMethod method)
            {
                int result = comparer.Compare(item, value);
                return method switch
                {
                    CompareMethod.GreaterThan => result > 0,
                    CompareMethod.GreaterThanOrEquals => result >= 0,
                    CompareMethod.LessThan => result < 0,
                    CompareMethod.LessThanOrEquals => result <= 0,
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
