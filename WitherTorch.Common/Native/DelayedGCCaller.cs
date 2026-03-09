using System;
using System.Threading;

namespace WitherTorch.Common.Native
{
    public sealed class DelayedGCCaller : DelayedCollectingObject
    {
        private static readonly object _syncRoot = new object();
        private static readonly DelayedGCCaller _secretGCCaller = new DelayedGCCaller();

        private static ulong _lastCollectingTime = 0UL;
        private static long _activeInstanceCount = 0L;

        public DelayedGCCaller() { }

        protected override void GenerateObject()
        {
            Interlocked.Increment(ref _activeInstanceCount);
        }

        protected override void DestroyObject()
        {
            const ulong DelayedGCCallerCooldown = 15000 * TimeSpan.TicksPerMillisecond;

            long instanceCount = Interlocked.Decrement(ref _activeInstanceCount);
            if (IsDisposed)
                return;
            bool shouldCollecting;
            lock (_syncRoot)
            {
                ulong now = NativeMethods.GetTicksForSystem();
                ulong lastCollectingTime = _lastCollectingTime;
                if (now <= lastCollectingTime || now - lastCollectingTime < DelayedGCCallerCooldown)
                    shouldCollecting = false;
                else
                {
                    shouldCollecting = true;
                    _lastCollectingTime = now;
                }
            }
            if (shouldCollecting)
            {
                if (instanceCount <= 0L)
                    GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
                else
                    GC.Collect(2, GCCollectionMode.Optimized, blocking: false, compacting: true);
            }
            else
            {
                DelayedGCCaller secretGCCaller = _secretGCCaller;
                secretGCCaller.AddRef();
                secretGCCaller.RemoveRef();
            }
        }
    }
}
