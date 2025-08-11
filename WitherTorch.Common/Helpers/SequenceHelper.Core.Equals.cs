using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
#pragma warning disable CS8500
    partial class SequenceHelper
    {
        unsafe partial class FastCore
        {
            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial bool Equals(byte* ptr, byte* ptr2, nuint length);
        }

        unsafe partial class FastCore<T>
        {
            public static bool RangedAddAndEquals(T* ptr, T* ptr2, nuint length, T lowerBound, T higherBound, T valueToAddInRange)
            {
                T* ptrEnd = ptr + length;
                if (CheckTypeCanBeVectorized())
                    return VectorizedRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
                return LegacyRangedAddAndEquals(ref ptr, ref ptr2, ptrEnd, lowerBound, higherBound, valueToAddInRange);
            }

            [Inline(InlineBehavior.Remove)]
            private static partial bool VectorizedRangedAddAndEquals(ref T* ptr, ref T* ptr2, T* ptrEnd, T lowerBound, T higherBound, T valueToAddInRange);

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyRangedAddAndEquals(ref T* ptr, ref T* ptr2, T* ptrEnd, T lowerBound, T higherBound, T valueToAddInRange)
            {
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (UnsafeHelper.NotEquals(RangedAddFast(*ptr, valueToAddInRange, lowerBound, higherBound),
                        RangedAddFast(*ptr2, valueToAddInRange, lowerBound, higherBound)))
                        return false;
                }
                return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static T RangedAddFast(T source, T valueToAdd, T lowerBound, T higherBound)
            {
                if (IsGreaterOrEqualsFast(source, lowerBound) && IsLessOrEqualsFast(source, higherBound))
                    return UnsafeHelper.Add(source, valueToAdd);
                return source;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsGreaterOrEqualsFast(T a, T b)
            {
                if (UnsafeHelper.IsUnsigned<T>())
                    return UnsafeHelper.IsGreaterThanOrEqualsUnsigned(a, b);
                return UnsafeHelper.IsGreaterThanOrEquals(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsGreaterOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) >= 0;

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsFast(T a, T b)
            {
                if (UnsafeHelper.IsUnsigned<T>())
                    return UnsafeHelper.IsLessThanOrEqualsUnsigned(a, b);
                return UnsafeHelper.IsLessThanOrEquals(a, b);
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IsLessOrEqualsSlow(T a, T b, IComparer<T> comparer)
                => comparer.Compare(a, b) <= 0;
        }
    }

    unsafe partial class SlowCore<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(T* ptr, T* ptr2, nuint length)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (nuint i = 0; i < length; i++)
            {
                if (comparer.Equals(ptr[i], ptr2[i]))
                    continue;
                return false;
            }
            return true;
        }
    }
}
