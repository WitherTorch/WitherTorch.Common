using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RiceTea.Core.Threading;

public interface ILockableEnumerable<T> : IEnumerable<T>, ILockable { }

public sealed class LockableEnumerable<T> : ILockableEnumerable<T>
{
    private readonly IEnumerable<T> _items;
    private readonly Lock _lock;

    public IEnumerable<T> Items => _items;

    public LockableEnumerable(IEnumerable<T> items, Lock @lock)
    {
        _items = items;
        _lock = @lock;
    }

    public Lock.Scope EnterLockScope() => _lock.EnterScope();

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
