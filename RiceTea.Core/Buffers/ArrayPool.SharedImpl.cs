#if NET472_OR_GREATER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Extensions;
using RiceTea.Core.Helpers;
using RiceTea.Core.Native;
using RiceTea.Core.Threading;

namespace RiceTea.Core.Buffers;

partial class ArrayPool<T>
{
    private sealed partial class SharedImpl : ArrayPool<T>
    {
        private const int LocalArrayQueuePreserveCount = 4;
        private const int GlobalArrayQueuePreserveCount = 1;

        private const int LocalArrayQueueCount = 4;
        private const int LocalArraySizeLimit = 1 << (LocalArrayQueueCount + 3);
        private const int GlobalArrayQueueCount = 20 - LocalArrayQueueCount - 3;
        private const int GlobalArraySizeLimit = 1 << 20;

        private readonly ProcessorLocal<ArrayQueue>[] _localArrayQueues;
        private readonly LazyTiny<ConcurrentArrayQueue>[] _globalArrayQueues;

        public SharedImpl()
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
            StrongBox<nuint> readerCountBox = new StrongBox<nuint>(0);
            Lock writerLock = new Lock();
            DelayedCall call = new DelayedCall(() =>
            {
                ref nuint readerCount = ref readerCountBox.Value;
                int generation = 0;

                SpinWait waiter = new SpinWait();
                while (true)
                {
                    while (InterlockedHelper.Read(ref readerCount) != 0)
                        waiter.SpinOnce();
                    lock (writerLock)
                    {
                        while (InterlockedHelper.Read(ref readerCount) != 0)
                        {
                            waiter.Reset();
                            continue;
                        }
                        int count = queue.Count;
                        if (count <= LocalArrayQueuePreserveCount)
                            return;
                        for (int i = LocalArrayQueuePreserveCount; i < count; i++)
                            generation = MathHelper.Max(generation, GC.GetGeneration(queue.Dequeue()));
                        break;
                    }
                }
                GC.Collect(generation, GCCollectionMode.Optimized, blocking: false, compacting: false);
            });
            return new ArrayQueue(call, queue, readerCountBox, writerLock);
        }

        private static ConcurrentArrayQueue CreateGlobalArrayQueue()
        {
            ConcurrentQueue<T[]> queue = new ConcurrentQueue<T[]>(); 
            StrongBox<nuint> readerCountBox = new StrongBox<nuint>(0);
            Lock writerLock = new Lock();
            DelayedCall call = new DelayedCall(() =>
            {
                ref nuint readerCount = ref readerCountBox.Value;
                int generation = 0;

                SpinWait waiter = new SpinWait();
                while (true)
                {
                    while (InterlockedHelper.Read(ref readerCount) != 0)
                        waiter.SpinOnce();
                    lock (writerLock)
                    {
                        while (InterlockedHelper.Read(ref readerCount) != 0)
                        {
                            waiter.Reset();
                            continue;
                        }
                        int count = queue.Count;
                        if (count <= GlobalArrayQueuePreserveCount)
                            return;
                        for (int i = GlobalArrayQueuePreserveCount; i < count && queue.TryDequeue(out T[] array); i++)
                            generation = MathHelper.Max(generation, GC.GetGeneration(array));
                        break;
                    }
                }
                GC.Collect(generation, GCCollectionMode.Optimized, blocking: false, compacting: false);
            });
            return new ConcurrentArrayQueue(call, queue, readerCountBox, writerLock);
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
                ArrayQueue queue = _localArrayQueues.AsUnsafeRef()[index].Value;
                ref nuint readerCount = ref queue.ReaderCountBox.Value;
                lock (queue.WriterLock)
                    InterlockedHelper.Increment(ref readerCount);
                try
                {
                    queue.Queue.TryDequeue(out array);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                call = queue.Call;
            }
            else
            {
                ConcurrentArrayQueue queue = _globalArrayQueues.AsUnsafeRef()[index - LocalArrayQueueCount].Value;
                ref nuint readerCount = ref queue.ReaderCountBox.Value;
                lock (queue.WriterLock)
                    InterlockedHelper.Increment(ref readerCount);
                try
                {
                    queue.Queue.TryDequeue(out array);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
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
                ArrayQueue queue = _localArrayQueues.AsUnsafeRef()[index].Value;
                ref nuint readerCount = ref queue.ReaderCountBox.Value;
                lock (queue.WriterLock)
                    InterlockedHelper.Increment(ref readerCount);
                try
                {
                    queue.Queue.Enqueue(array);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                call = queue.Call;
            }
            else
            {
                ConcurrentArrayQueue queue = _globalArrayQueues.AsUnsafeRef()[index - LocalArrayQueueCount].Value;
                ref nuint readerCount = ref queue.ReaderCountBox.Value;
                lock (queue.WriterLock)
                    InterlockedHelper.Increment(ref readerCount);
                try
                {
                    queue.Queue.Enqueue(array);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                call = queue.Call;
            }
            call.RemoveRef();
        }

        private sealed record class ArrayQueue(
            DelayedCall Call,
            Queue<T[]> Queue,
            StrongBox<nuint> ReaderCountBox,
            Lock WriterLock
            );

        private sealed record class ConcurrentArrayQueue(
            DelayedCall Call,
            ConcurrentQueue<T[]> Queue,
            StrongBox<nuint> ReaderCountBox,
            Lock WriterLock
            );
    }
}
#endif