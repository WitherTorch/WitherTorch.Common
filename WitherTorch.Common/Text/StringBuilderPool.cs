using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Native;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Text
{
    internal sealed class StringBuilderPool : IPool<StringBuilder>
    {
        private static readonly Lazy<StringBuilderPool> _sharedPoolLazy = new Lazy<StringBuilderPool>(() => new StringBuilderPool(1),
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static StringBuilderPool Shared => _sharedPoolLazy.Value;

        private readonly ProcessorLocal<StringBuilderLocal> _localGroups;

        public StringBuilderPool(int initialLength)
        {
            _localGroups = new ProcessorLocal<StringBuilderLocal>(() =>
            {
                Queue<StringBuilder> queue = new Queue<StringBuilder>(initialLength);
                for (int i = 0; i < initialLength; i++)
                    queue.Enqueue(new StringBuilder());
                DelayedCall call = new DelayedCall(() =>
                {
                    int count = queue.Count;
                    for (int i = initialLength; i < count; i++)
                        queue.Dequeue();
                });
                return new StringBuilderLocal(call, queue);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilder Rent()
        {
            StringBuilderLocal local = _localGroups.Value;
            if (!local.Queue.TryDequeue(out StringBuilder? result) || result is null)
                result = new StringBuilder();
            local.Call.AddRef();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(StringBuilder obj)
        {
            StringBuilderLocal local = _localGroups.Value;
            local.Queue.Enqueue(obj.Clear());
            local.Call.RemoveRef();
        }

        private sealed record class StringBuilderLocal(
            DelayedCall Call,
            Queue<StringBuilder> Queue
            );
    }
}
