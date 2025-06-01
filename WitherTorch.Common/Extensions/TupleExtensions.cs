using System.Collections.Generic;

using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class TupleExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> _this, out TKey key, out TValue value)
        {
            key = _this.Key;
            value = _this.Value;
        }
    }
}
