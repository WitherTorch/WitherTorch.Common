using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using InlineMethod;

namespace WitherTorch.Common.Native
{
    internal sealed class DelayedCollector
    {
        private const ulong DelayedCollectingPeriod = 5000 * TimeSpan.TicksPerMillisecond;
        private const ulong DelayedCollectingNoRefTime = DelayedCollectingPeriod / 2;

        private static readonly DelayedCollector _instance = new DelayedCollector();

        private readonly Thread _thread;
        private readonly ConcurrentQueue<DelayedCollectingObject> _queue;
        private readonly HashSet<DelayedCollectingObject> _innerSet;
        private readonly IntPtr _waitingEventHandle;

        public static DelayedCollector Instance => _instance;

        private DelayedCollector()
        {
            _innerSet = new HashSet<DelayedCollectingObject>();
            _queue = new ConcurrentQueue<DelayedCollectingObject>();
            _waitingEventHandle = NativeMethods.CreateWaitingHandle(autoReset: true);
            _thread = new Thread(DoTick)
            {
                IsBackground = true,
                Name = nameof(DelayedCollector) + " Thread",
                Priority = ThreadPriority.Lowest
            };
            _thread.Start();
        }

        public void AddObject(DelayedCollectingObject obj)
        {
            _queue.Enqueue(obj);
            NativeMethods.SetWaitingHandle(_waitingEventHandle);
        }

        private void DoTick()
        {
            HashSet<DelayedCollectingObject> innerSet = _innerSet;
            ConcurrentQueue<DelayedCollectingObject> bag = _queue;
            IntPtr waitingEventHandle = _waitingEventHandle;
            NativeMethods.WaitForWaitingHandle(waitingEventHandle);
            while (true)
            {
                ulong predictedTicks = NativeMethods.GetTicksForSystem() + DelayedCollectingPeriod;

                while (bag.TryDequeue(out DelayedCollectingObject? obj))
                    innerSet.Add(obj);

                DoLifeCheck(innerSet);

                if (innerSet.Count <= 0)
                {
                    Debug.WriteLine($"[{nameof(DelayedCollector)}] No object needs collecting, sleeping...");
                    NativeMethods.WaitForWaitingHandle(waitingEventHandle);
                    continue;
                }

                if (!NativeMethods.SleepInAbsoluteTicks(predictedTicks))
                    Thread.Yield();
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void DoLifeCheck(HashSet<DelayedCollectingObject> list)
        {
            if (list.Count <= 0)
                return;
            ulong now = NativeMethods.GetTicksForSystem();
#if DEBUG
            int count =
#endif
            list.RemoveWhere(obj =>
            {
                if (obj.IsDisposed)
                    return true;
                if (obj.IsInReference)
                    return false;
                if ((now - obj.LastDereferenceTime) > DelayedCollectingNoRefTime)
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
    }
}
