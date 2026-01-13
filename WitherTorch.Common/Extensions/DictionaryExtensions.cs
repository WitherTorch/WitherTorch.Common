using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> _this, TKey key, TValue? defaultValue)
            => _this.TryGetValue(key, out TValue? result) ? result : defaultValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveAndDispose<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> _this, TKey key) where TKey : notnull where TValue : IDisposable
        {
            bool result = _this.TryRemove(key, out TValue? val);
            if (!result)
                return false;
            val?.Dispose();
            return true;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> _this) => _this.Count <= 0;

#if NET472_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> _this, TKey key, TValue value)
        {
            if (_this.ContainsKey(key))
                return false;
            _this.Add(key, value);
            return true;
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> _this)
        {
            if (_this.Count <= 0)
                return CollectionHelper.EmptyReadOnlyDictionary<TKey, TValue>();

            if (_this is IReadOnlyDictionary<TKey, TValue> result)
                return result;

            return new ReadOnlyDictionaryWrapper<TKey, TValue>(_this);
        }

        private sealed class ReadOnlyDictionaryWrapper<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> _dictionary;

            public ReadOnlyDictionaryWrapper(IDictionary<TKey, TValue> dictionary) => _dictionary = dictionary;

            public TValue this[TKey key] => _dictionary[key];

            public IEnumerable<TKey> Keys => _dictionary.Keys;

            public IEnumerable<TValue> Values => _dictionary.Values;

            public int Count => _dictionary.Count;

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

#nullable disable
            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
#nullable restore

            IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
        }
    }
}
