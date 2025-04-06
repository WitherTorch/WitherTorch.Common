using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Extensions
{
    partial class CollectionExtensions
    {
        private sealed class ReadOnlyCollectionAdapter<T> : ICollection<T>, IReadOnlyCollection<T>
        {
            private readonly ICollection<T> _collection;

            public ReadOnlyCollectionAdapter(ICollection<T> collection)
            {
                _collection = collection;
            }

            public int Count => _collection.Count;

            public bool IsReadOnly => _collection.IsReadOnly;

            public void Add(T item) => _collection.Add(item);

            public void Clear() => _collection.Clear();

            public bool Contains(T item) => _collection.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

            public bool Remove(T item) => _collection.Remove(item);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();
        }
    }
}
