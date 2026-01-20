using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Collections
{
    partial class AppendOnlyCollection
    {
        public sealed class Enumerator<T> : IEnumerator<T>
        {
            private readonly IAppendOnlyCollection<T> _list;
            private int _index;

            public Enumerator(IAppendOnlyCollection<T> list)
            {
                _list = list;
                _index = -1;
            }

            public T Current
            {
                get
                {
                    int index = _index;
                    if (index < 0 || index >= _list.Count)
                        throw new InvalidOperationException();
                    return _list[index];
                }
            }

            object? IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                int index = _index + 1;
                int count = _list.Count;
                if (index < count)
                {
                    _index = index;
                    return index >= 0;
                }
                return false;
            }

            public void Reset() => _index = -1;
        }
    }
}