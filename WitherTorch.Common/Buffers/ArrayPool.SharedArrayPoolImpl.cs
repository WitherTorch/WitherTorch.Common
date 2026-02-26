#if NET472_OR_GREATER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private sealed partial class SharedArrayPoolImpl : ArrayPool<T>
        {
            private const int LocalArrayQueuePreserveCount = 1;
            private const int GlobalArrayQueuePreserveCount = 1;

            private const int LocalArrayQueueCount = 4;
            private const int LocalArraySizeLimit = 1 << (LocalArrayQueueCount + 3);
            private const int GlobalArrayQueueCount = 20 - LocalArrayQueueCount - 3;
            private const int GlobalArraySizeLimit = 1 << 20;

            private readonly ProcessorLocal<ArrayQueue>[] _localArrayQueues;
            private readonly LazyTiny<ConcurrentArrayQueue>[] _globalArrayQueues;

            public SharedArrayPoolImpl()
            {
                ProcessorLocal<ArrayQueue>[] localArrayQueues = new ProcessorLocal<ArrayQueue>[LocalArrayQueueCount];
                localArrayQueues[0] = new(static () => CreateLocalArrayQueue(16));
                localArrayQueues[1] = new(static () => CreateLocalArrayQueue(32));
                localArrayQueues[2] = new(static () => CreateLocalArrayQueue(64));
                localArrayQueues[3] = new(static () => CreateLocalArrayQueue(128));
                LazyTiny<ConcurrentArrayQueue>[] globalArrayQueues = new LazyTiny<ConcurrentArrayQueue>[GlobalArrayQueueCount];
                for (int i = 0; i < GlobalArrayQueueCount; i++)
                {
                    globalArrayQueues[i] = new LazyTiny<ConcurrentArrayQueue>(CreateGlobalArrayQueue, LazyThreadSafetyMode.ExecutionAndPublication);
                }
                _localArrayQueues = localArrayQueues;
                _globalArrayQueues = globalArrayQueues;
            }

            private static ArrayQueue CreateLocalArrayQueue(int arraySize)
            {
                Queue<T[]> queue = new Queue<T[]>(LocalArrayQueuePreserveCount);
                for (int i = 0; i < LocalArrayQueuePreserveCount; i++)
                    queue.Enqueue(new T[arraySize]);
                DelayedCall call = new DelayedCall(() =>
                {
                    int count = queue.Count;
                    for (int i = LocalArrayQueuePreserveCount; i < count; i++)
                        queue.Dequeue();
                });
                return new ArrayQueue(call, queue);
            }

            private static ConcurrentArrayQueue CreateGlobalArrayQueue()
            {
                ConcurrentQueue<T[]> queue = new ConcurrentQueue<T[]>();
                DelayedCall call = new DelayedCall(() =>
                {
                    int count = queue.Count;
                    for (int i = GlobalArrayQueuePreserveCount; i < count; i++)
                        queue.TryDequeue(out _);
                });
                return new ConcurrentArrayQueue(call, queue);
            }

            protected override T[] RentCore(nuint capacity)
            {
                if (capacity > GlobalArraySizeLimit)
                    return new T[capacity];

                capacity >>= 4;
                int index = MathHelper.Log2(capacity);
                index += MathHelper.BooleanToInt32(capacity >= (1U << index));

                DelayedCall call;
                T[]? array;
                if (index < LocalArrayQueueCount)
                {
                    ArrayQueue queue = UnsafeHelper.AddTypedOffset(ref _localArrayQueues[0], index).Value;
                    queue.Queue.TryDequeue(out array);
                    call = queue.Call;
                }
                else
                {
                    ConcurrentArrayQueue queue = UnsafeHelper.AddTypedOffset(ref _globalArrayQueues[0], index - LocalArrayQueueCount).Value;
                    queue.Queue.TryDequeue(out array);
                    call = queue.Call;
                }
                call.AddRef();
                return array ?? new T[MinimumArraySize << index];
            }

            protected override void ReturnCore(T[] array)
            {
                int length = array.Length;
                if (length < 16 || length > GlobalArraySizeLimit || !MathHelper.IsPow2(length))
                    return;
                int index = MathHelper.Log2((uint)length) - 4;
                DelayedCall call;
                if (index < LocalArrayQueueCount)
                {
                    ArrayQueue queue = UnsafeHelper.AddTypedOffset(ref _localArrayQueues[0], index).Value;
                    queue.Queue.Enqueue(array);
                    call = queue.Call;
                }
                else
                {
                    ConcurrentArrayQueue queue = UnsafeHelper.AddTypedOffset(ref _globalArrayQueues[0], index - LocalArrayQueueCount).Value;
                    queue.Queue.Enqueue(array);
                    call = queue.Call;
                }
                call.RemoveRef();
            }

            private sealed record class ArrayQueue(
                DelayedCall Call,
                Queue<T[]> Queue
                );

            private sealed record class ConcurrentArrayQueue(
                DelayedCall Call,
                ConcurrentQueue<T[]> Queue
                );
        }
    }
}
#endif