using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Collections
{
    partial class AppendOnlyCollection
    {
        public sealed class ReversedEnumerator<T> : IEnumerator<T>
        {
            private readonly IAppendOnlyCollection<T> _list;
            private int _index;

            public ReversedEnumerator(IAppendOnlyCollection<T> list)
            {
                _list = list;
                _index = list.Count;
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
                int index = _index - 1;
                if (index >= 0)
                {
                    _index = index;
                    return index < _list.Count;
                }
                return false;
            }

            public void Reset() => _index = _list.Count;
        }
    }
}