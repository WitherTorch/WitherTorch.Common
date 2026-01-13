using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class CollectionHelper
    {
        private sealed class EmptyReadOnlyDictionaryImpl<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        {
            public static readonly EmptyReadOnlyDictionaryImpl<TKey, TValue> Instance = new EmptyReadOnlyDictionaryImpl<TKey, TValue>();

            private EmptyReadOnlyDictionaryImpl() { }

            public TValue this[TKey key]
            {
                get => throw new KeyNotFoundException();
                set { }
            }

            public ICollection<TKey> Keys => EmptyCollection<TKey>();

            public ICollection<TValue> Values => EmptyCollection<TValue>();

            public int Count => 0;

            public bool IsReadOnly => true;

            public void Add(TKey key, TValue value) { }

            public void Add(KeyValuePair<TKey, TValue> item) { }

            public void Clear() { }

            public bool Contains(KeyValuePair<TKey, TValue> item) => false;

            public bool ContainsKey(TKey key) => false;

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => EmptyEnumerator<KeyValuePair<TKey, TValue>>();

            public bool Remove(TKey key) => false;

            public bool Remove(KeyValuePair<TKey, TValue> item) => false;

            public bool TryGetValue(TKey key, out TValue value)
            {
                value = default!;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() => EmptyEnumerator<KeyValuePair<TKey, TValue>>();

            IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => EmptyCollection<TKey>();

            IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => EmptyCollection<TValue>();
        }
    }
}
