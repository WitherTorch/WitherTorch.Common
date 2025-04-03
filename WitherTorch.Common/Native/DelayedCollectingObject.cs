using InlineMethod;

using System;
using System.Threading;

namespace WitherTorch.Common.Native
{
    public abstract class DelayedCollectingObject : IDisposable
    {
        private long disposed, created, refCount, lastDerefTime;

        public bool IsDisposed => CheckDisposed();

        public bool IsCreated => Interlocked.Read(ref created) > 0;

        public bool IsInReference => Interlocked.Read(ref refCount) > 0;

        public long LastRefTime => Interlocked.Read(ref lastDerefTime);

        protected DelayedCollectingObject()
        {
            disposed = 0;
            created = 0;
            refCount = 0;
            lastDerefTime = 0;
        }

        public void AddRef()
        {
            if (CheckDisposed())
                return;
            if (Interlocked.Increment(ref refCount) == 1)
            {
                DelayedCollector.Instance.AddObject(this);
                TryGenerateObject();
            }
        }

        public void RemoveRef()
        {
            long refCount = Interlocked.Decrement(ref this.refCount);
            if (refCount == 0)
            {
                Interlocked.Exchange(ref lastDerefTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                if (CheckDisposed())
                {
                    TryDestroyObject();
                }
            }
            else if (refCount < 0)
                Interlocked.Exchange(ref refCount, 0);
        }

        internal void RemoveObject()
        {
            if (CheckDisposed())
                return;
            TryDestroyObject();
        }

        [Inline(InlineBehavior.Remove)]
        private void TryGenerateObject()
        {
            if (Interlocked.CompareExchange(ref created, 1, 0) == 0)
            {
                GenerateObject();
            }
        }

        [Inline(InlineBehavior.Remove)]
        private void TryDestroyObject()
        {
            if (Interlocked.CompareExchange(ref created, 0, 1) == 1)
            {
                DestroyObject();
            }
        }

        protected abstract void GenerateObject();

        protected abstract void DestroyObject();

        [Inline(InlineBehavior.Remove)]
        private bool CheckDisposed()
        {
            return Interlocked.Read(ref disposed) > 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsInReference && disposing)
                return;
            if (Interlocked.CompareExchange(ref disposed, 1, 0) == 0)
            {
                TryDestroyObject();
            }
        }

        // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~DelayedCollectingObject()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
