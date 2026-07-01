using System;
using System.Threading;

using InlineMethod;

using RiceTea.Core.Helpers;

namespace RiceTea.Core.Native;

public abstract class DelayedCollectingObject : ICheckableDisposable
{
    private ulong _disposed, _created, _refCount, _lastDerefTime;

    public bool IsDisposed => CheckDisposed();

    public bool IsCreated => Volatile.Read(ref _created) > 0;

    public bool IsInReference => InterlockedHelper.Read(ref _refCount) > 0;

    public ulong LastDereferenceTime => InterlockedHelper.Read(ref _lastDerefTime);

    protected DelayedCollectingObject()
    {
        _disposed = 0;
        _created = 0;
        _refCount = 0;
        _lastDerefTime = 0;
    }

    public void AddRef()
    {
        if (CheckDisposed() || InterlockedHelper.LimitedIncrement(ref _refCount, ulong.MaxValue) != 1 || !TryGenerateObject())
            return;
        DelayedCollector.Instance.AddObject(this);
    }

    public void RemoveRef()
    {
        if (CheckDisposed() || InterlockedHelper.LimitedDecrement(ref _refCount, 0) != 0)
            return;
        InterlockedHelper.Write(ref _lastDerefTime, NativeMethods.GetTicksForSystem());
    }

    internal void RemoveObject()
    {
        if (CheckDisposed())
            return;
        TryDestroyObject();
    }

    [Inline(InlineBehavior.Remove)]
    private bool TryGenerateObject()
    {
        if (InterlockedHelper.Exchange(ref _created, ulong.MaxValue) != 0)
            return false;
        GenerateObject();
        return true;
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
    private bool CheckDisposed() => Volatile.Read(ref _disposed) != 0;

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
