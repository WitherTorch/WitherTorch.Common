using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> _this, TKey key, TValue defaultValue)
        {
            return _this.TryGetValue(key, out TValue? result) ? result : defaultValue;
        }

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
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> _this)
        {
            return _this.Count <= 0;
        }
    }
}
