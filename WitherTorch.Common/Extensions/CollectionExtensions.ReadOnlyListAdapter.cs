using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Extensions
{
    public static partial class CollectionExtensions
    {
        private sealed class ReadOnlyListAdapter<T> : IList<T>, IReadOnlyList<T>
        {
            private readonly IList<T> _list;

            public ReadOnlyListAdapter(IList<T> list)
            {
                _list = list;
            }

            public T this[int index] { get => _list[index]; set => _list[index] = value; }

            public int Count => _list.Count;

            public bool IsReadOnly => _list.IsReadOnly;

            public void Add(T item) => _list.Add(item);

            public void Clear() => _list.Clear();

            public bool Contains(T item) => _list.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

            public int IndexOf(T item) => _list.IndexOf(item);

            public void Insert(int index, T item) => _list.Insert(index, item);

            public bool Remove(T item) => _list.Remove(item);

            public void RemoveAt(int index) => _list.RemoveAt(index);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();
        }
    }
}
