using InlineMethod;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;

namespace WitherTorch.Common.Native
{
    internal sealed class DelayedCollector : IDisposable
    {
        private const int DelayedCollectingPeriod = 5000;
        private const int DelayedCollectingNoRefTime = DelayedCollectingPeriod / 2;

        private static readonly DelayedCollector _instance = new DelayedCollector();

        private readonly Thread _thread;
        private readonly AutoResetEvent _waitingEvent = new AutoResetEvent(false);
        private readonly ConcurrentQueue<DelayedCollectingObject> _queue;
        private readonly HashSet<DelayedCollectingObject> _innerSet;

        private long _state;

        public static DelayedCollector Instance => _instance;

        private DelayedCollector()
        {
            _state = 0;
            _innerSet = new HashSet<DelayedCollectingObject>();
            _queue = new ConcurrentQueue<DelayedCollectingObject>();
            _thread = new Thread(DoTick)
            {
                IsBackground = true,
                Name = nameof(DelayedCollector) + " Thread",
                Priority = ThreadPriority.Lowest
            };
        }

        public void AddObject(DelayedCollectingObject obj)
        {
            if (obj is null || CheckDisposed())
                return;

            long state = Interlocked.CompareExchange(ref _state, 1L, 0L);
            if (state >= 2L)
                return;

            AddObjectReal(obj);

            _waitingEvent.Set();

            if (state == 0L)
            {
                Thread thread = _thread;
                if ((thread.ThreadState & System.Threading.ThreadState.Unstarted) > 0)
                {
                    thread.Start();
                }

                Interlocked.CompareExchange(ref _state, 0L, 1L);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private void AddObjectReal(DelayedCollectingObject obj)
        {
            _queue.Enqueue(obj);
        }

        private void DoTick()
        {
            HashSet<DelayedCollectingObject> innerSet = _innerSet;
            ConcurrentQueue<DelayedCollectingObject> bag = _queue;
            Stopwatch stopwatch = new Stopwatch();
            AutoResetEvent waitingEvent = _waitingEvent;
            waitingEvent.WaitOne();
            while (!CheckDisposed())
            {
                stopwatch.Restart();

                while (bag.TryDequeue(out DelayedCollectingObject? obj))
                    innerSet.Add(obj);

                DoLifeCheck(innerSet);

                if (innerSet.Count <= 0)
                {
                    Debug.WriteLine($"[{nameof(DelayedCollector)}] No object needs collecting, sleeping...");
                    stopwatch.Stop();
                    waitingEvent.WaitOne();
                    continue;
                }

                stopwatch.Stop();
                long time = stopwatch.ElapsedMilliseconds;


                if (time < 0L)
                    Thread.Sleep(DelayedCollectingPeriod);
                else if (time < DelayedCollectingPeriod)
                    Thread.Sleep(DelayedCollectingPeriod - unchecked((int)time));
                else
                    Thread.Yield();
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void DoLifeCheck(HashSet<DelayedCollectingObject> list)
        {
            if (list.Count <= 0)
                return;
            ulong now = unchecked((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
#if DEBUG
            int count =
#endif
            list.RemoveWhere(obj =>
            {
                if (obj.IsDisposed)
                    return true;
                if (obj.IsInReference)
                    return false;
                if ((now - obj.LastRefTime) > DelayedCollectingNoRefTime)
                {
                    obj.RemoveObject();
                    return true;
                }
                return false;
            });
#if DEBUG
            Debug.WriteLineIf(count > 0, $"[{nameof(DelayedCollector)}] Removed {count} object(s)");
#endif
        }

        [Inline(InlineBehavior.Remove)]
        private bool CheckDisposed()
        {
            return Interlocked.Read(ref _state) >= 2;
        }

        private void DisposeCore()
        {
            if (Interlocked.Exchange(ref _state, 2) >= 2)
                return;
            AutoResetEvent waitingEvent = _waitingEvent;

            waitingEvent.Set();
            waitingEvent.Dispose();
        }

        ~DelayedCollector()
        {
            DisposeCore();
        }

        public void Dispose()
        {
            DisposeCore();
            GC.SuppressFinalize(this);
        }
    }
}
