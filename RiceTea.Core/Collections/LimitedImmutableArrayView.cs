using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Extensions;
using RiceTea.Core.Threading;

namespace RiceTea.Core.Collections;

public sealed class LimitedImmutableArrayView<T> : IReadOnlyList<T>, IEnumerable<T>, IReversibleEnumerable<T>, ILockable where T : class?
{
    public static readonly LimitedImmutableArrayView<T> Empty = new LimitedImmutableArrayView<T>(Array.Empty<T>(), 0);

    private readonly T[] _array;
    private readonly int _count;

    public LimitedImmutableArrayView(T[] array, int count)
    {
        if (array.Length < count)
            ArgumentOutOfRangeException.Throw(nameof(count));
        _array = array;
        _count = count;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= _count)
                ArgumentOutOfRangeException.Throw(nameof(index));
            return _array.AsUnsafeRef()[index];
        }
    }

    public int Count => _count;

    public Lock.Scope EnterLockScope() => default;

    public IEnumerator<T> GetEnumerator() => new LimitedArrayEnumerator<T>(_array, _count);

    public IEnumerator<T> GetReversedEnumerator() => new LimitedArrayReversedEnumerator<T>(_array, _count);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
