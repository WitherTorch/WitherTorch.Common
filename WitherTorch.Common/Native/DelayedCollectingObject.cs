using System;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    public abstract class DelayedCollectingObject : ICheckableDisposable
    {
        private ulong _disposed, _created, _refCount, _lastDerefTime;

        public bool IsDisposed => CheckDisposed();

        public bool IsCreated => InterlockedHelper.Read(ref _created) > 0;

        public bool IsInReference => InterlockedHelper.Read(ref _refCount) > 0;

        public ulong LastRefTime => InterlockedHelper.Read(ref _lastDerefTime);

        protected DelayedCollectingObject()
        {
            _disposed = 0;
            _created = 0;
            _refCount = 0;
            _lastDerefTime = 0;
        }

        public void AddRef()
        {
            if (CheckDisposed())
                return;
            switch (InterlockedHelper.Increment(ref _refCount))
            {
                case 0:
                    InterlockedHelper.CompareExchange(ref _refCount, ulong.MaxValue, 0);
                    break;
                case 1:
                    DelayedCollector.Instance.AddObject(this);
                    TryGenerateObject();
                    break;
                default:
                    break;
            }
        }

        public void RemoveRef()
        {
            if (CheckDisposed())
                return;
            switch (InterlockedHelper.Add(ref _refCount, ulong.MaxValue))
            {
                case ulong.MaxValue:
                    InterlockedHelper.CompareExchange(ref _refCount, 0, ulong.MaxValue);
                    break;
                case 0:
                    InterlockedHelper.Exchange(ref _lastDerefTime, (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    break;
                default:
                    break;
            }
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
            if (InterlockedHelper.Exchange(ref _created, ulong.MaxValue) != 0)
                return;
            GenerateObject();
        }

        [Inline(InlineBehavior.Remove)]
        private void TryDestroyObject()
        {
            if (InterlockedHelper.Exchange(ref _created, 0) == 0)
                return;
            DestroyObject();
        }

        protected abstract void GenerateObject();

        protected abstract void DestroyObject();

        [Inline(InlineBehavior.Remove)]
        private bool CheckDisposed() => InterlockedHelper.Read(ref _disposed) != 0;

        protected virtual void Dispose(bool disposing)
        {
            if (IsInReference && disposing)
                return;
            if (InterlockedHelper.CompareExchange(ref _disposed, 1, 0) == 0)
                TryDestroyObject();
        }

        ~DelayedCollectingObject() => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
