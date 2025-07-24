using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Text
{
    public abstract partial class StringBase
    {
        private sealed class IndexEnumerator : IEnumerator<char>
        {
            private readonly StringBase _source;
            private readonly int _length;

            private int _index;

            public IndexEnumerator(StringBase str)
            {
                _source = str;
                _length = str.Length;
                _index = -1;
            }

            public char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    int index = _index;
                    if (index < 0 || index >= _length)
                        throw new InvalidOperationException("Enumerator is not positioned within the string.");
                    return _source.GetCharAt(unchecked((nuint)index));
                }
            }

            object System.Collections.IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                int index = _index;
                if (index < 0)
                {
                    _index = 0;
                    return true;
                }
                index++;
                if (index < _length)
                {
                    _index = index;
                    return true;
                }
                return false;
            }

            public void Reset() => _index = -1;
        }
    }
}
