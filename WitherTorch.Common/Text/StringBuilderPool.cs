using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Text
{
    internal sealed class StringBuilderPool : IPool<DelayedCollectingStringBuilder>
    {
        private static readonly Lazy<StringBuilderPool> _sharedPoolLazy = new Lazy<StringBuilderPool>(() => new StringBuilderPool(1),
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static StringBuilderPool Shared => _sharedPoolLazy.Value;

        private readonly ConcurrentBag<DelayedCollectingStringBuilder> _bag;

        public StringBuilderPool(int initialLength)
        {
            if (initialLength > 0)
            {
                DelayedCollectingStringBuilder[] builders = new DelayedCollectingStringBuilder[initialLength];
                for (int i = 0; i < initialLength; i++)
                    builders[i] = new DelayedCollectingStringBuilder();
                _bag = new ConcurrentBag<DelayedCollectingStringBuilder>(builders);
            }
            else
            {
                _bag = new ConcurrentBag<DelayedCollectingStringBuilder>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DelayedCollectingStringBuilder Rent()
        {
            DelayedCollectingStringBuilder result = RentCore();
            result.AddRef();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private DelayedCollectingStringBuilder RentCore()
        {
            if (_bag.TryTake(out DelayedCollectingStringBuilder? result))
            {
                result.GetObject()?.Clear();
                return result;
            }
            return new DelayedCollectingStringBuilder();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(DelayedCollectingStringBuilder obj)
        {
            obj.RemoveRef();
            _bag.Add(obj);
        }
    }
}
