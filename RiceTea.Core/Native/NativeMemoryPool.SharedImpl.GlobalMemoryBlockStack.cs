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

            private bool _hasTrimCaller = false;
            private ulong _lastTrimTimestamp = 0;

            public GlobalMemoryBlockStack(nuint blockSize)
            {
                _stack = new Stack<nuint>(GlobalMemoryBlockStackPreserveCount);
                _lock = new Lock();
                _blockSize = blockSize;

                TrimCaller.Register(this);
            }

            public void Push(void* ptr)
            {
                lock (_lock)
                {
                    Stack<nuint> stack = _stack;
                    stack.Push((nuint)ptr);
                    if (!_hasTrimCaller && stack.Count > GlobalMemoryBlockStackPreserveCount)
                    {
                        TrimCaller.Register(this);
                        _hasTrimCaller = true;
                    }
                }
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
                if (!@lock.TryEnter())
                {
                    TrimCaller.Register(this);
                    return;
                }
                try
                {
                    _hasTrimCaller = false;

                    ulong now = NativeMethods.GetTicksForSystem();
                    if (now - _lastTrimTimestamp < MinimumTrimPeriod)
                        return;
                    _lastTrimTimestamp = now;

                    Stack<nuint> stack = _stack;
                    int count = stack.Count;
                    if (count <= GlobalMemoryBlockStackPreserveCount)
                        return;
                    nuint size = _blockSize;
                    for (int i = GlobalMemoryBlockStackPreserveCount; i < count; i++)
                        NativeMethods.FreeMemoryBlock(new NativeMemoryBlock((void*)stack.Pop(), size));
                }
                finally
                {
                    @lock.Exit();
                }
            }

            private sealed class TrimCaller
            {
                private readonly GlobalMemoryBlockStack _owner;

                public TrimCaller(GlobalMemoryBlockStack owner) => _owner = owner;

                public static void Register(GlobalMemoryBlockStack owner) => new TrimCaller(owner);

                ~TrimCaller() => _owner.Trim();
            }
        }
    }
}