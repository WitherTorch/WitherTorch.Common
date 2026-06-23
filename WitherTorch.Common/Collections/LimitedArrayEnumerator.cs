using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Collections;

internal class LimitedArrayEnumerator<T> : IEnumerator<T>
{
    private readonly T[] _array;
    private readonly int _count;

    private int _index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LimitedArrayEnumerator(T[] array, int count)
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
        int index = _index + 1;
        int count = _count;
        if (index < count)
        {
            _index = index;
            return index >= 0;
        }
        return false;
    }

    public void Reset()
    {
        _index = 0;
    }
}