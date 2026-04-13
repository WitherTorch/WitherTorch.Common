using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Native
{
    unsafe partial class NativeMemoryPool
    {
        private static partial NativeMemoryPool CreateSharedPool() => new SharedImpl();

        private sealed partial class SharedImpl : NativeMemoryPool
        {
            private const int LocalArrayQueuePreserveCount = 1;
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
                StrongBox<nuint> barrierBox = new StrongBox<nuint>(0);
                DelayedCall call = new DelayedCall(() =>
                {
                    InterlockedHelper.Write(ref barrierBox.Value, UnsafeHelper.GetMaxValue<nuint>());
                    try
                    {
                        int count = queue.Count;
                        for (int i = LocalArrayQueuePreserveCount; i < count; i++)
                            NativeMethods.FreeMemoryBlock(queue.Dequeue());
                    }
                    finally
                    {
                        InterlockedHelper.Write(ref barrierBox.Value, 0);
                    }
                });
                return new ArrayQueue(call, queue, barrierBox);
            }

            private static ConcurrentArrayQueue CreateGlobalArrayQueue()
            {
                ConcurrentQueue<NativeMemoryBlock> queue = new ConcurrentQueue<NativeMemoryBlock>();
                DelayedCall call = new DelayedCall(() =>
                {
                    int count = queue.Count;
                    for (int i = GlobalArrayQueuePreserveCount; i < count; i++)
                    {
                        if (queue.TryDequeue(out NativeMemoryBlock block))
                            NativeMethods.FreeMemoryBlock(block);
                    }
                });
                return new ConcurrentArrayQueue(call, queue);
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
                    ArrayQueue queue = UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_localArrayQueues), index).Value;
                    if (InterlockedHelper.Read(ref queue.BarrierBox.Value) == 0)
                    {
                        queue.Queue.TryDequeue(out memoryBlock);
                        capacity = memoryBlock.Length;
                    }
                    else
                    {
                        memoryBlock = default;
                        capacity = 0;
                    }
                    call = queue.Call;
                }
                else
                {
                    ConcurrentArrayQueue queue = UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_globalArrayQueues), index - LocalArrayQueueCount).Value;
                    queue.Queue.TryDequeue(out memoryBlock);
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
                    ArrayQueue queue = UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_localArrayQueues), index).Value;
                    queue.Queue.Enqueue(new NativeMemoryBlock(ptr, length));
                    call = queue.Call;
                }
                else
                {
                    ConcurrentArrayQueue queue = UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_globalArrayQueues), index - LocalArrayQueueCount).Value;
                    queue.Queue.Enqueue(new NativeMemoryBlock(ptr, length));
                    call = queue.Call;
                }
                call.RemoveRef();
            }

            private sealed record class ArrayQueue(
                DelayedCall Call,
                Queue<NativeMemoryBlock> Queue,
                StrongBox<nuint> BarrierBox
                );

            private sealed record class ConcurrentArrayQueue(
                DelayedCall Call,
                ConcurrentQueue<NativeMemoryBlock> Queue
                );
        }
    }
}