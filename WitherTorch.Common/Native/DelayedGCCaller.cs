using System;
using System.Threading;

namespace WitherTorch.Common.Native
{
    public sealed class DelayedGCCaller : DelayedCollectingObject
    {
        private const int DelayedGCCallerCooldown = 15000;

        private static readonly object _syncRoot = new object();

        private static readonly Lazy<DelayedGCCaller> secretGCCallerLazy = new Lazy<DelayedGCCaller>(() => new DelayedGCCaller(), LazyThreadSafetyMode.ExecutionAndPublication);

        private static long activeInstanceCount = 0L;

        private static long lastCollectingTime = 0L;

        public DelayedGCCaller() { }

        protected override void GenerateObject()
        {
            Interlocked.Increment(ref activeInstanceCount);
        }

        protected override void DestroyObject()
        {
            long instCount = Interlocked.Decrement(ref activeInstanceCount);
            if (IsDisposed)
                return;
            bool shouldCollecting;
            lock (_syncRoot)
            {
                Thread.MemoryBarrier();
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                long lastCollectingTime = DelayedGCCaller.lastCollectingTime;
                if (now - lastCollectingTime < DelayedGCCallerCooldown)
                    shouldCollecting = false;
                else
                {
                    shouldCollecting = true;
                    DelayedGCCaller.lastCollectingTime = now;
                }
            }
            if (shouldCollecting)
            {
                if (instCount <= 0L)
                    GC.Collect(2, GCCollectionMode.Forced, true, true);
                else
                    GC.Collect(2, GCCollectionMode.Optimized, false, true);
            }
            else
            {
                DelayedGCCaller secretGCCaller = secretGCCallerLazy.Value;
                secretGCCaller.AddRef();
                secretGCCaller.RemoveRef();
            }
        }
    }
}
