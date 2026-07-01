using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Extensions;
using RiceTea.Core.Helpers;
using RiceTea.Core.Threading;

namespace RiceTea.Core.Native;

unsafe partial class NativeMemoryPool
{
    private static partial NativeMemoryPool CreateSharedPool() => new SharedImpl();

    private sealed partial class SharedImpl : NativeMemoryPool
    {
        private const int LocalArrayQueuePreserveCount = 4;
        private const int GlobalArrayQueuePreserveCount = 1;

        private const int LocalArrayQueueCount = 6;
        private const int LocalArraySizeLimit = 1 << (LocalArrayQueueCount + 3);
        private const int GlobalArrayQueueCount = 20 - LocalArrayQueueCount - 3;
        private const int GlobalArraySizeLimit = 1 << 20;

        private readonly ProcessorLocal<ArrayQueue>[] _localArrayQueues;
        private readonly LazyTiny<ConcurrentArrayQueue>[] _globalArrayQueues;

        public SharedImpl()
        {
            ProcessorLocal<ArrayQueue>[] localArrayQueues = new ProcessorLocal<ArrayQueue>[LocalArrayQueueCount];
            localArrayQueues[0] = new(static () => CreateLocalMemoryBlockQueue(16));
            localArrayQueues[1] = new(static () => CreateLocalMemoryBlockQueue(32));
            localArrayQueues[2] = new(static () => CreateLocalMemoryBlockQueue(64));
            localArrayQueues[3] = new(static () => CreateLocalMemoryBlockQueue(128));
            localArrayQueues[4] = new(static () => CreateLocalMemoryBlockQueue(256));
            localArrayQueues[5] = new(static () => CreateLocalMemoryBlockQueue(512));
            LazyTiny<ConcurrentArrayQueue>[] globalArrayQueues = new LazyTiny<ConcurrentArrayQueue>[GlobalArrayQueueCount];
            for (int i = 0; i < GlobalArrayQueueCount; i++)
            {
                globalArrayQueues[i] = new LazyTiny<ConcurrentArrayQueue>(CreateGlobalArrayQueue, LazyThreadSafetyMode.ExecutionAndPublication);
            }
            _localArrayQueues = localArrayQueues;
            _globalArrayQueues = globalArrayQueues;
        }

        private static ArrayQueue CreateLocalMemoryBlockQueue(int blockSize)
        {
            Queue<NativeMemoryBlock> queue = new Queue<NativeMemoryBlock>(LocalArrayQueuePreserveCount);
            for (int i = 0; i < LocalArrayQueuePreserveCount; i++)
                queue.Enqueue(NativeMethods.AllocMemoryBlock(blockSize));
            StrongBox<nuint> readerCountBox = new StrongBox<nuint>(0);
            Lock writerLock = new Lock();
            DelayedCall call = new DelayedCall(() =>
            {
                ref nuint readerCount = ref readerCountBox.Value;

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
                        for (int i = LocalArrayQueuePreserveCount; i < count; i++)
                            NativeMethods.FreeMemoryBlock(queue.Dequeue());
                        break;
                    }
                }
            });
            return new ArrayQueue(call, queue, readerCountBox, writerLock);
        }

        private static ConcurrentArrayQueue CreateGlobalArrayQueue()
        {
            ConcurrentQueue<NativeMemoryBlock> queue = new ConcurrentQueue<NativeMemoryBlock>();
            StrongBox<nuint> readerCountBox = new StrongBox<nuint>(0);
            Lock writerLock = new Lock();
            DelayedCall call = new DelayedCall(() =>
            {
                ref nuint readerCount = ref readerCountBox.Value;

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
                        for (int i = GlobalArrayQueuePreserveCount; i < count && queue.TryDequeue(out NativeMemoryBlock block); i++)
                            NativeMethods.FreeMemoryBlock(block);
                        break;
                    }
                }
            });
            return new ConcurrentArrayQueue(call, queue, readerCountBox, writerLock);
        }

        protected override void* RentCore(ref nuint capacity)
        {
            if (capacity > GlobalArraySizeLimit)
                return NativeMethods.AllocMemory(capacity);

            capacity >>= 4;
            int index = MathHelper.Log2(capacity);
            index += MathHelper.BooleanToInt32(capacity >= (1U << index));

            DelayedCall call;
            NativeMemoryBlock memoryBlock;
            if (index < LocalArrayQueueCount)
            {
                ArrayQueue queue = _localArrayQueues.AsUnsafeRef()[index].Value;
                ref nuint readerCount = ref queue.ReaderCountBox.Value;
                lock (queue.WriterLock)
                    InterlockedHelper.Increment(ref readerCount);
                try
                {
                    queue.Queue.TryDequeue(out memoryBlock);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                capacity = memoryBlock.Length;
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
                    queue.Queue.TryDequeue(out memoryBlock);
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                capacity = memoryBlock.Length;
                call = queue.Call;
            }
            call.AddRef();
            void* result = memoryBlock.NativePointer;
            if (result != null)
                return result;
            capacity = MinimumMemoryBlockSize << index;
            return NativeMethods.AllocMemory(capacity);
        }

        protected override void ReturnCore(void* ptr, nuint length)
        {
            if (length < 16 || length > GlobalArraySizeLimit || !MathHelper.IsPow2(length))
            {
                NativeMethods.FreeMemory(ptr);
                return;
            }
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
                    queue.Queue.Enqueue(new NativeMemoryBlock(ptr, length));
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
                    queue.Queue.Enqueue(new NativeMemoryBlock(ptr, length));
                }
                finally
                {
                    InterlockedHelper.Decrement(ref readerCount);
                }
                call = queue.Call;
            }
            call.RemoveRef();
        }
    }

    private sealed record class ArrayQueue(
        DelayedCall Call,
            Queue<NativeMemoryBlock> Queue,
            StrongBox<nuint> ReaderCountBox,
            Lock WriterLock
        );

    private sealed record class ConcurrentArrayQueue(
        DelayedCall Call,
        ConcurrentQueue<NativeMemoryBlock> Queue,
        StrongBox<nuint> ReaderCountBox,
        Lock WriterLock
        );
}