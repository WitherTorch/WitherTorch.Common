#if NET472_OR_GREATER
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Helpers;
using RiceTea.Core.Native;

namespace RiceTea.Core.Buffers;

partial class ArrayPool<T>
{
    partial class SharedImpl
    {
        private sealed class GlobalArrayStack
        {
            private readonly Stack<T[]> _stack;
            private readonly Lock _lock;
            private readonly int _arraySize;

            private ulong _lastTrimTimestamp = 0;

            public GlobalArrayStack(int arraySize)
            {
                _stack = new Stack<T[]>(GlobalArrayStackPreserveCount);
                _lock = new Lock();
                _arraySize = arraySize;
                TrimCaller.Register(this);
            }

            public void Push(T[] array)
            {
                DebugHelper.ThrowIf(array.Length != _arraySize, "Array size mismatch.");
                lock (_lock)
                    _stack.Push(array);
            }

            public T[] Pop()
            {
                lock (_lock)
                {
                    Stack<T[]> stack = _stack;
                    if (stack.Count <= 0)
                        goto Create;
                    return stack.Pop();
                }

            Create:
                if (_isUnmanagedType)
                    return ArrayHelper.CreateUninitializedArray<T>(_arraySize);
                else
                    return new T[_arraySize];
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

                        Stack<T[]> stack = _stack;
                        int count = stack.Count;
                        if (count <= GlobalArrayStackPreserveCount)
                            return;
                        for (int i = GlobalArrayStackPreserveCount; i < count; i++)
                            stack.Pop();
                    }
                    finally
                    {
                        @lock.Exit();
                    }
                }
                finally
                {
                    TrimCaller.Register(this);
                }
            }

            private sealed class TrimCaller
            {
                private readonly GlobalArrayStack _owner;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private TrimCaller(GlobalArrayStack owner) => _owner = owner;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static void Register(GlobalArrayStack owner) => new TrimCaller(owner);

                ~TrimCaller() => _owner.Trim();
            }
        }
    }
}
#endif