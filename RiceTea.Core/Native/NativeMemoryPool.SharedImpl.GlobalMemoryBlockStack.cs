using System;
using System.Collections.Generic;
using System.Threading;

namespace RiceTea.Core.Native;

unsafe partial class NativeMemoryPool
{

    partial class SharedImpl
    {
        private sealed class GlobalMemoryBlockStack
        {
            private readonly Stack<nuint> _stack;
            private readonly Lock _lock;
            private readonly nuint _blockSize;

            private ulong _lastTrimTimestamp = 0;

            public GlobalMemoryBlockStack(nuint blockSize)
            {
                _stack = new Stack<nuint>(GlobalArrayStackPreserveCount);
                _lock = new Lock();
                _blockSize = blockSize;

                DelayedTrimCaller.Register(this);
            }

            public void Push(void* ptr)
            {
                lock (_lock)
                    _stack.Push((nuint)ptr);
            }

            public void* Pop()
            {
                lock (_lock)
                {
                    Stack<nuint> stack = _stack;
                    if (stack.Count <= 0)
                        goto Create;
                    return (void*)stack.Pop();
                }

            Create:
                return NativeMethods.AllocMemoryBlock(_blockSize).NativePointer;
            }

            public void Trim()
            {
                const ulong MinimumTrimPeriod = 60 * TimeSpan.TicksPerSecond;

                Lock @lock = _lock;
                try
                {
                    if (!@lock.TryEnter())
                        return;
                    try
                    {
                        ulong now = NativeMethods.GetTicksForSystem();
                        if (now - _lastTrimTimestamp < MinimumTrimPeriod)
                            return;
                        _lastTrimTimestamp = now;

                        Stack<nuint> stack = _stack;
                        int count = stack.Count;
                        if (count <= GlobalArrayStackPreserveCount)
                            return;
                        nuint size = _blockSize;
                        for (int i = GlobalArrayStackPreserveCount; i < count; i++)
                            NativeMethods.FreeMemoryBlock(new NativeMemoryBlock((void*)stack.Pop(), size));
                    }
                    finally
                    {
                        @lock.Exit();
                    }
                }
                finally
                {
                    DelayedTrimCaller.Register(this);
                }
            }

            private sealed class DelayedTrimCaller
            {
                private readonly GlobalMemoryBlockStack _owner;

                public DelayedTrimCaller(GlobalMemoryBlockStack owner) => _owner = owner;

                public static void Register(GlobalMemoryBlockStack owner) => new DelayedTrimCaller(owner);

                ~DelayedTrimCaller() => _owner.Trim();
            }
        }
    }
}