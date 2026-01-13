using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class CollectionHelper
    {
        private sealed class EmptyReadOnlyCollectionImpl<T> : ICollection, ICollection<T>, IReadOnlyCollection<T>
        {
            public static readonly EmptyReadOnlyCollectionImpl<T> Instance = new EmptyReadOnlyCollectionImpl<T>();

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

    }
}
