using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    public static class ReferenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>([NotNullIfNotNull(nameof(location2))] ref T? location1, [NotNullIfNotNull(nameof(location1))] ref T? location2)
            => (location1, location2) = (location2, location1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Swap<T>(ref T* location1, ref T* location2)
        {
            T* temp = location1;
            location1 = location2;
            location2 = temp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static T? Exchange<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value)
        {
            T? result = location;
            location = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* Exchange(ref void* location, void* value)
        {
            void* result = location;
            location = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T* Exchange<T>(ref T* location, T* value)
        {
            T* result = location;
            location = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static T? CompareExchange<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value, T? comparand)
            => UnsafeHelper.IsPrimitiveType<T>() ? CompareExchange_Primitive(ref location, value, comparand) : CompareExchange_Fallback(ref location, value, comparand);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        private static T? CompareExchange_Primitive<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value, T? comparand)
        {
            T? oldValue = location;
            T mask = UnsafeHelper.Negate(UnsafeHelper.As<bool, T>(UnsafeHelper.Equals(location, comparand)));
            location = UnsafeHelper.Or(UnsafeHelper.And(oldValue, UnsafeHelper.Not(mask)), UnsafeHelper.And(value, mask));
            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        private static T? CompareExchange_Fallback<T>([NotNullIfNotNull(nameof(value))] ref T location, T value, T comparand)
        {
            T oldValue = location;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(oldValue, comparand))
                location = value;
            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* CompareExchange(ref void* location, void* value, void* comparand)
        {
            void* oldValue = location;
            nuint mask = UnsafeHelper.Negate(MathHelper.BooleanToNativeUnsigned(location == comparand));
            location = (void*)((nuint)oldValue & ~mask | (nuint)value & mask);
            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T* CompareExchange<T>(ref T* location, T* value, T* comparand)
        {
            T* oldValue = location;
            nuint mask = UnsafeHelper.Negate(MathHelper.BooleanToNativeUnsigned(location == comparand));
            location = (T*)((nuint)oldValue & ~mask | (nuint)value & mask);
            return oldValue;
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
