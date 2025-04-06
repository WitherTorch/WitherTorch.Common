using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    public static class CollectionHelper
    {
        public static ICollection EmptyCollection() => EmptyReadOnlyCollectionImpl<object>.Instance;

        public static ICollection<T> EmptyCollection<T>() => EmptyReadOnlyCollectionImpl<T>.Instance;

        public static IReadOnlyCollection<T> EmptyReadOnlyCollection<T>() => EmptyReadOnlyCollectionImpl<T>.Instance;

        public static IEnumerator EmptyEnumerator() => EmptyEnumeratorImpl<object>.Instance;

        public static IEnumerator<T> EmptyEnumerator<T>() => EmptyEnumeratorImpl<T>.Instance;

        private sealed class EmptyReadOnlyCollectionImpl<T> : ICollection, ICollection<T>, IReadOnlyCollection<T>
        {
            private static readonly EmptyReadOnlyCollectionImpl<T> _instance = new EmptyReadOnlyCollectionImpl<T>();

            public static EmptyReadOnlyCollectionImpl<T> Instance => _instance;

            public int Count => 0;

            public bool IsReadOnly => true;

            public object SyncRoot => this;

            public bool IsSynchronized => true;

            public void Add(T item) { }

            public void Clear() { }

            public bool Contains(T item) => false;

            public void CopyTo(Array array, int arrayIndex) { }

            public void CopyTo(T[] array, int arrayIndex) { }

            public bool Remove(T item) => false;

            public IEnumerator<T> GetEnumerator() => EmptyEnumeratorImpl<T>.Instance;

            IEnumerator IEnumerable.GetEnumerator() => EmptyEnumeratorImpl<T>.Instance;
        }

        private sealed class EmptyEnumeratorImpl<T> : IEnumerator<T>
        {
            private static readonly EmptyEnumeratorImpl<T> _instance = new EmptyEnumeratorImpl<T>();

            public static EmptyEnumeratorImpl<T> Instance => _instance;

            public T Current => throw new InvalidOperationException();

            object IEnumerator.Current => throw new InvalidOperationException();

            public void Dispose() { }

            public bool MoveNext() => false;

            public void Reset() { }
        }

    }
}
