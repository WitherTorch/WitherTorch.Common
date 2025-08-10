using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Extensions
{
    public static class LinqExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IndexedValue<TSource>> WithIndex<TSource>(this IEnumerable<TSource> _this)
        {
            using IEnumerator<TSource> enumerator = _this.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
                yield return new IndexedValue<TSource>(i, enumerator.Current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<NativeIndexedValue<TSource>> WithNativeIndex<TSource>(this IEnumerable<TSource> _this)
        {
            using IEnumerator<TSource> enumerator = _this.GetEnumerator();
            for (nuint i = 0; enumerator.MoveNext(); i++)
                yield return new NativeIndexedValue<TSource>(i, enumerator.Current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> WhereEqualsTo<TSource>(this IEnumerable<TSource> _this, TSource value)
        {
            if (UnsafeHelper.IsPrimitiveType<TSource>())
                return WhereEqualsToIterator_Primitive(_this, value);

            return WhereEqualsToIterator(_this, value, EqualityComparer<TSource>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> WhereEqualsTo<TSource>(this IEnumerable<TSource> _this, 
            TSource filter, IEqualityComparer<TSource> comparer) 
            => WhereEqualsToIterator(_this, filter, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IndexedValue<TValue>> WhereEqualsTo<TValue>(this IEnumerable<IndexedValue<TValue>> _this, TValue value)
        {
            if (UnsafeHelper.IsPrimitiveType<TValue>())
                return WhereEqualsToIterator_Primitive(_this, value);

            return WhereEqualsToIterator(_this, value, EqualityComparer<TValue>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IndexedValue<TValue>> WhereEqualsTo<TValue>(this IEnumerable<IndexedValue<TValue>> _this, 
            TValue value, IEqualityComparer<TValue> comparer) 
            => WhereEqualsToIterator(_this, value, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<NativeIndexedValue<TValue>> WhereEqualsTo<TValue>(this IEnumerable<NativeIndexedValue<TValue>> _this, TValue value)
        {
            if (UnsafeHelper.IsPrimitiveType<TValue>())
                return WhereEqualsToIterator_Primitive(_this, value);

            return WhereEqualsToIterator(_this, value, EqualityComparer<TValue>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<NativeIndexedValue<TValue>> WhereEqualsTo<TValue>(this IEnumerable<NativeIndexedValue<TValue>> _this, 
            TValue filter, IEqualityComparer<TValue> comparer) 
            => WhereEqualsToIterator(_this, filter, comparer);

        private static IEnumerable<TSource> WhereEqualsToIterator_Primitive<TSource>(IEnumerable<TSource> source, TSource filter)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TSource current = enumerator.Current;
                if (UnsafeHelper.Equals(current, filter))
                    yield return current;
            }
        }

        private static IEnumerable<IndexedValue<TValue>> WhereEqualsToIterator_Primitive<TValue>(IEnumerable<IndexedValue<TValue>> source, TValue filter)
        {
            using IEnumerator<IndexedValue<TValue>> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IndexedValue<TValue> current = enumerator.Current;
                if (UnsafeHelper.Equals(current.Value, filter))
                    yield return current;
            }
        }

        private static IEnumerable<NativeIndexedValue<TValue>> WhereEqualsToIterator_Primitive<TValue>(IEnumerable<NativeIndexedValue<TValue>> source, TValue filter)
        {
            using IEnumerator<NativeIndexedValue<TValue>> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NativeIndexedValue<TValue> current = enumerator.Current;
                if (UnsafeHelper.Equals(current.Value, filter))
                    yield return current;
            }
        }

        private static IEnumerable<TSource> WhereEqualsToIterator<TSource>(IEnumerable<TSource> source,
            TSource filter, IEqualityComparer<TSource> comparer)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TSource current = enumerator.Current;
                if (comparer.Equals(current, filter))
                    yield return current;
            }
        }

        private static IEnumerable<IndexedValue<TValue>> WhereEqualsToIterator<TValue>(IEnumerable<IndexedValue<TValue>> source,
            TValue filter, IEqualityComparer<TValue> comparer)
        {
            using IEnumerator<IndexedValue<TValue>> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IndexedValue<TValue> current = enumerator.Current;
                if (comparer.Equals(current.Value, filter))
                    yield return current;
            }
        }

        private static IEnumerable<NativeIndexedValue<TValue>> WhereEqualsToIterator<TValue>(IEnumerable<NativeIndexedValue<TValue>> source,
            TValue filter, IEqualityComparer<TValue> comparer)
        {
            using IEnumerator<NativeIndexedValue<TValue>> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NativeIndexedValue<TValue> current = enumerator.Current;
                if (comparer.Equals(current.Value, filter))
                    yield return current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IndexedValue<TValue>> Where<TValue>(this IEnumerable<IndexedValue<TValue>> _this, Func<TValue, bool> predicate)
        {
            using IEnumerator<IndexedValue<TValue>> enumerator = _this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IndexedValue<TValue> current = enumerator.Current;
                if (predicate.Invoke(current.Value))
                    yield return current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<NativeIndexedValue<TValue>> Where<TValue>(this IEnumerable<NativeIndexedValue<TValue>> _this, Func<TValue, bool> predicate)
        {
            using IEnumerator<NativeIndexedValue<TValue>> enumerator = _this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NativeIndexedValue<TValue> current = enumerator.Current;
                if (predicate.Invoke(current.Value))
                    yield return current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> _this, nuint count)
        {
            if (count == 0)
                return _this;
            return SkipIterator(_this, count);
        }

        private static IEnumerable<TSource> SkipIterator<TSource>(IEnumerable<TSource> source, nuint count)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                    yield break;
            }
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> _this, nuint count)
        {
            if (count == 0)
                return Enumerable.Empty<TSource>();

            return TakeIterator(_this, count);
        }

        private static IEnumerable<TSource> TakeIterator<TSource>(IEnumerable<TSource> source, nuint count)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                    yield break;

                yield return enumerator.Current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> SkipAndTake<TSource>(this IEnumerable<TSource> _this, int skipCount, int takeCount)
        {
            if (skipCount <= 0)
                return _this;
            if (takeCount <= 0)
                return Enumerable.Empty<TSource>();

            return SkipAndTakeIterator(_this, unchecked((nuint)skipCount), unchecked((nuint)takeCount));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> SkipAndTake<TSource>(this IEnumerable<TSource> _this, nuint skipCount, nuint takeCount)
        {
            if (skipCount == 0)
                return _this;
            if (takeCount == 0)
                return Enumerable.Empty<TSource>();

            return SkipAndTakeIterator(_this, skipCount, takeCount);
        }

        private static IEnumerable<TSource> SkipAndTakeIterator<TSource>(IEnumerable<TSource> source, nuint skipCount, nuint takeCount)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < skipCount; i++)
            {
                if (!enumerator.MoveNext())
                    yield break;
            }
            for (nuint i = 0; i < takeCount; i++)
            {
                if (!enumerator.MoveNext())
                    yield break;

                yield return enumerator.Current;
            }
        }
    }
}
