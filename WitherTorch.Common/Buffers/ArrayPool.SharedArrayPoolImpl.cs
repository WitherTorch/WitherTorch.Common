#if NET472_OR_GREATER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;
using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private sealed partial class SharedArrayPoolImpl : ArrayPool<T>
        {
            private const int LocalArrayQueueCount = 4;
            private const int LocalArraySizeLimit = 1 << (LocalArrayQueueCount + 3);
            private const int GlobalArrayQueueCount = 20 - LocalArrayQueueCount - 3;
            private const int GlobalArraySizeLimit = 1 << 20;

            private readonly ThreadLocal<Queue<DelayedArrayHolder>>[] _smallQueueLocal;
            private readonly LazyTiny<ConcurrentQueue<DelayedArrayHolder>>[] _largeQueueLazy;
            private readonly ThreadLocal<UnwrappableList<DelayedArrayHolder>> _restoreListLocal;

            public SharedArrayPoolImpl()
            {
                ThreadLocal<Queue<DelayedArrayHolder>>[] smallQueueLocal = new ThreadLocal<Queue<DelayedArrayHolder>>[LocalArrayQueueCount];
                for (int i = 0; i < LocalArrayQueueCount; i++)
                {
                    smallQueueLocal[i] = new ThreadLocal<Queue<DelayedArrayHolder>>(() => new Queue<DelayedArrayHolder>(), false);
                }
                LazyTiny<ConcurrentQueue<DelayedArrayHolder>>[] largeQueueLazy = new LazyTiny<ConcurrentQueue<DelayedArrayHolder>>[GlobalArrayQueueCount];
                for (int i = 0; i < GlobalArrayQueueCount; i++)
                {
                    largeQueueLazy[i] = new LazyTiny<ConcurrentQueue<DelayedArrayHolder>>(() => new ConcurrentQueue<DelayedArrayHolder>(), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                _smallQueueLocal = smallQueueLocal;
                _largeQueueLazy = largeQueueLazy;
                _restoreListLocal = new ThreadLocal<UnwrappableList<DelayedArrayHolder>>(() => new UnwrappableList<DelayedArrayHolder>(2), false);
            }

            public override T[] Rent(nuint capacity)
            {
                if (capacity == 0)
                    return Array.Empty<T>();
                if (capacity > GlobalArrayQueueCount)
                    return new T[capacity];

                capacity >>= 4;
                int index = MathHelper.Log2(capacity);
                index += MathHelper.BooleanToInt32(capacity > (1U << index));

                DelayedArrayHolder? holder;
                if (index < LocalArrayQueueCount)
                    _smallQueueLocal[index].Value!.TryDequeue(out holder);
                else
                    _largeQueueLazy[index - LocalArrayQueueCount].Value!.TryDequeue(out holder);
                holder ??= new DelayedArrayHolder(MinimumArraySize << index);
                holder.AddRef();
                _restoreListLocal.Value!.Add(holder);
                return holder.Array;
            }

            public override void Return(T[] array, bool clearArray)
            {
                int length = array.Length;
                if (length <= 0 || length > GlobalArraySizeLimit)
                    return;
                int index = MathHelper.Log2((uint)length) - 4;
                if (index < 0 || !TryRemoveHolderInRentList(array, out DelayedArrayHolder? holder))
                    return;
                if (clearArray)
                    SequenceHelper.Clear(array);
                if (index < LocalArrayQueueCount)
                    _smallQueueLocal[index].Value!.Enqueue(holder);
                else
                    _largeQueueLazy[index - LocalArrayQueueCount].Value.Enqueue(holder);
                holder.RemoveRef();
            }

            private bool TryRemoveHolderInRentList(T[] array, [NotNullWhen(true)] out DelayedArrayHolder? holder)
            {
                UnwrappableList<DelayedArrayHolder> restoreList = _restoreListLocal.Value!;
                int count = restoreList.Count;
                if (count <= 0)
                    goto Failed;

                ref DelayedArrayHolder restoreListRef = ref restoreList.Unwrap()[0];
                for (nuint i = 0, limit = (nuint)count; i < limit; i++)
                {
                    DelayedArrayHolder item = UnsafeHelper.AddByteOffset(ref restoreListRef, i * UnsafeHelper.SizeOf<DelayedArrayHolder>());
                    if (!ReferenceEquals(item.Array, array))
                        continue;
                    restoreList.Remove(item);
                    holder = item;
                    return true;
                }

            Failed:
                holder = null;
                return false;
            }
        }
    }
}
#endif