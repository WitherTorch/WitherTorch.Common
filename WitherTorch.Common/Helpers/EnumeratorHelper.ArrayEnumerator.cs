using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class EnumeratorHelper
    {
        private sealed class ArrayEnumerator<T> : IEnumerator<T>
        {
            private readonly T[] _array;

            private int _index;

            public ArrayEnumerator(T[] array)
            {
                _array = array;
                _index = -1;
            }

            public T Current
            {
                get
                {
                    int index = _index;
                    if (index < 0 || index >= _array.Length)
                        throw new InvalidOperationException();
                    return _array[index];
                }
            }

            object? IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                int index = _index + 1;
                int length = _array.Length;
                if (index >= length)
                {
                    _index = length;
                    return false;
                }
                return true;
            }

            public void Reset() => _index = -1;
        }
    }
}
