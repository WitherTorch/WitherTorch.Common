using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;
using WitherTorch.Common.Text;

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

        public static unsafe bool SequenceEqual<TSource>(this IEnumerable<TSource> _this, TSource* compare, int count) where TSource : unmanaged
        {
            if (count <= 0)
            {
                using IEnumerator<TSource> enumerator = _this.GetEnumerator();
                return !enumerator.MoveNext();
            }

            return SequenceEqualCore(_this, compare, unchecked((nuint)count));
        }

        public static unsafe bool SequenceEqual<TSource>(this IEnumerable<TSource> _this, TSource* compare, nuint count) where TSource : unmanaged
        {
            if (count == 0)
            {
                using IEnumerator<TSource> enumerator = _this.GetEnumerator();
                return !enumerator.MoveNext();
            }

            return SequenceEqualCore(_this, compare, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool SequenceEqualCore<TSource>(IEnumerable<TSource> source, TSource* compare, nuint count) where TSource : unmanaged
        {
            switch (source)
            {
                case TSource[] array:
                    {
                        if (array.Length < MathHelper.MakeSigned(count))
                            return false;
                        fixed (TSource* ptr = array)
                            return SequenceHelper.Equals(ptr, compare, count);
                    }
                case UnwrappableList<TSource> list:
                    {
                        if (list.Count < MathHelper.MakeSigned(count))
                            return false;
                        fixed (TSource* ptr = list.Unwrap())
                            return SequenceHelper.Equals(ptr, compare, count);
                    }
                default:
                    break;
            }

            if (UnsafeHelper.IsPrimitiveType<TSource>())
                return SequenceEqualCore_Primitive(source, compare, count);
            return SequenceEqualCore_NonPrimitive(source, compare, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool SequenceEqualCore_Primitive<TSource>(IEnumerable<TSource> source, TSource* compare, nuint count) where TSource : unmanaged
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext() || !UnsafeHelper.Equals(enumerator.Current, compare[i]))
                    return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool SequenceEqualCore_NonPrimitive<TSource>(IEnumerable<TSource> source, TSource* compare, nuint count) where TSource : unmanaged
        {
            EqualityComparer<TSource> comparer = EqualityComparer<TSource>.Default;

            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext() || !comparer.Equals(enumerator.Current, compare[i]))
                    return false;
            }
            return true;
        }

        public static unsafe int SequenceCompare<TSource>(this IEnumerable<TSource> _this, IEnumerable<TSource> other)
        {
            Comparer<TSource> comparer = Comparer<TSource>.Default;
            using IEnumerator<TSource> enumeratorThis = _this.GetEnumerator();
            using IEnumerator<TSource> enumeratorOther = other.GetEnumerator();
            while (enumeratorThis.MoveNext())
            {
                if (!enumeratorOther.MoveNext())
                    return 1;
                int comparison = comparer.Compare(enumeratorThis.Current, enumeratorOther.Current);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return enumeratorOther.MoveNext() ? -1 : 0;
        }

        public static unsafe int SequenceCompare<TSource>(this IEnumerable<TSource> _this, TSource* compare, int count) where TSource : unmanaged
        {
            if (count <= 0)
            {
                using IEnumerator<TSource> enumerator = _this.GetEnumerator();
                return enumerator.MoveNext() ? 1 : 0;
            }

            return SequenceCompareCore(_this, compare, unchecked((nuint)count));
        }

        public static unsafe int SequenceCompare<TSource>(this IEnumerable<TSource> _this, TSource* compare, nuint count) where TSource : unmanaged
        {
            if (count == 0)
            {
                using IEnumerator<TSource> enumerator = _this.GetEnumerator();
                return enumerator.MoveNext() ? 1 : 0;
            }

            return SequenceCompareCore(_this, compare, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int SequenceCompareCore<TSource>(IEnumerable<TSource> source, TSource* compare, nuint count) where TSource : unmanaged
        {
            switch (source)
            {
                case TSource[] array:
                    {
                        int comparison = array.Length.CompareTo(MathHelper.MakeSigned(count));
                        if (comparison != 0)
                            return MathHelper.Sign(comparison);
                        fixed (TSource* ptr = array)
                            return InternalSequenceHelper.CompareTo(ptr, compare, count);
                    }
                case UnwrappableList<TSource> list:
                    {
                        int comparison = list.Count.CompareTo(MathHelper.MakeSigned(count));
                        if (comparison != 0)
                            return MathHelper.Sign(comparison);
                        fixed (TSource* ptr = list.Unwrap())
                            return InternalSequenceHelper.CompareTo(ptr, compare, count);
                    }
                default:
                    break;
            }

            Comparer<TSource> comparer = Comparer<TSource>.Default;
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                    return -1;
                int comparison = comparer.Compare(enumerator.Current, compare[i]);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return enumerator.MoveNext() ? 1 : 0;
        }

        public static nuint NativeCount<TSource>(this IEnumerable<TSource> _this)
        {
            nuint i = 0;
            using IEnumerator<TSource> enumerator = _this.GetEnumerator();
            while (enumerator.MoveNext() && ++i != 0) ;
            return i;
        }
    }
}
