using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    public static partial class CollectionHelper
    {
        public static ICollection EmptyCollection() => EmptyReadOnlyCollectionImpl<object>.Instance;

        public static ICollection<T> EmptyCollection<T>() => EmptyReadOnlyCollectionImpl<T>.Instance;

        public static IReadOnlyCollection<T> EmptyReadOnlyCollection<T>() => EmptyReadOnlyCollectionImpl<T>.Instance;

        public static IEnumerator EmptyEnumerator() => EmptyEnumeratorImpl<object>.Instance;

        public static IEnumerator<T> EmptyEnumerator<T>() => EmptyEnumeratorImpl<T>.Instance;

        public static IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>() => EmptyReadOnlyDictionaryImpl<TKey, TValue>.Instance;

        public static IReadOnlyDictionary<TKey, TValue> EmptyReadOnlyDictionary<TKey, TValue>() => EmptyReadOnlyDictionaryImpl<TKey, TValue>.Instance;
    }
}
