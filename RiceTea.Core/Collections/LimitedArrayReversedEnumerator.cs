using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using RiceTea.Core.Extensions;

namespace RiceTea.Core.Collections;

internal class LimitedArrayReversedEnumerator<T> : IEnumerator<T>
{
    private readonly T[] _array;
    private readonly int _count;

    private int _index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LimitedArrayReversedEnumerator(T[] array, int count)
    {
        if (array.Length < count)
            Throw();

        _array = array;
        _count = count;
        _index = -1;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Throw() => ArgumentOutOfRangeException.Throw(nameof(count));
    }

    public T Current
    {
        get
        {
            int index = _index;
            if (index < 0 || index >= _count)
                throw new InvalidOperationException();
            return _array.AsUnsafeRef()[index];
        }
    }

    object? IEnumerator.Current => Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        int index = _index - 1;
        if (index >= 0)
        {
            _index = index;
            return index < _count;
        }
        return false;
    }

    public void Reset()
    {
        _index = _count;
    }
}