using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static class ReferenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>([NotNullIfNotNull(nameof(location2))] ref T? location1, [NotNullIfNotNull(nameof(location1))] ref T? location2)
            => (location1, location2) = (location2, location1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static T? Exchange<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value)
        {
            T? result = location;
            location = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static T? CompareExchange<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value, T? comparand)
        {
            T? result = location;

            if (UnsafeHelper.IsPrimitiveType<T>())
                goto Primitive;
            goto Fallback;

        Primitive:
            if (UnsafeHelper.Equals(result, comparand))
                goto Change;
            goto Result;

        Fallback:
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(value!, comparand!))
                goto Change;
            goto Result;

        Change:
            location = value;
            goto Result;

        Result:
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static T? CompareExchange<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value, T? comparand, IEqualityComparer<T> comparer)
        {
            T? result = location;
            if (comparer.Equals(value!, comparand!))
                location = value;
            return result;
        }
    }
}
