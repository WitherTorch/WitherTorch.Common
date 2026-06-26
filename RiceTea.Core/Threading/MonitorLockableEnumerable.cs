using System.Collections;
using System.Collections.Generic;

namespace RiceTea.Core.Threading;

public interface IMonitorLockableEnumerable<T> : IEnumerable<T>, IMonitorLockable { }

public sealed class MonitorLockableEnumerable<T> : IMonitorLockableEnumerable<T>
{
    private readonly IEnumerable<T> _items;
    private readonly object _lockObj;

    public IEnumerable<T> Items => _items;

    public MonitorLockableEnumerable(IEnumerable<T> items, object lockObj)
    {
        _items = items;
        _lockObj = lockObj;
    }

    public MonitorLockScope EnterLockScope() => MonitorLockScope.Enter(_lockObj);

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
