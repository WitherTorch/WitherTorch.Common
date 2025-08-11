using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class EnumeratorHelper
    {
        private sealed class OneItemEnumerator<T> : IEnumerator<T>
        {
            private readonly T _item;

            private byte _state;

            public OneItemEnumerator(T item)
            {
                _item = item;
                _state = 0;
            }

            public T Current => _state == 1 ? _item : throw new InvalidOperationException();

            object? IEnumerator.Current => _state == 1 ? _item : throw new InvalidOperationException();

            public void Dispose() { }

            public bool MoveNext()
            {
                switch (_state)
                {
                    case 0:
                        _state = 1;
                        return true;
                    case 1:
                        _state = 2;
                        return true;
                    default:
                        return false;
                }
            }

            public void Reset() => _state = 0;
        }
    }
}
