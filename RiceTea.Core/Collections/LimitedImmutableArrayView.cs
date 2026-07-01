using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Buffers;
using RiceTea.Core.Extensions;
using RiceTea.Core.Threading;

namespace RiceTea.Core.Collections;

public sealed class LimitedImmutableArrayView<T> : IReadOnlyList<T>, IEnumerable<T>, IReversibleEnumerable<T>, ILockable where T : class?
{
    public static readonly LimitedImmutableArrayView<T> Empty = new LimitedImmutableArrayView<T>(Array.Empty<T>(), 0);

    private readonly T[] _array;
    private readonly int _count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int startIndex) => Array.Copy(_array, 0, array, startIndex, _count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(scoped ref ArrayPool<T>.RentScope scope) => scope.CopyFrom(_array, 0, _count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<T> GetEnumerator() => new LimitedArrayEnumerator<T>(_array, _count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<T> GetReversedEnumerator() => new LimitedArrayReversedEnumerator<T>(_array, _count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Lock.Scope ILockable.EnterLockScope() => default;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
