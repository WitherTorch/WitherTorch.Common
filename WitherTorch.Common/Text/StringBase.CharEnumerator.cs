using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Text
{
    public abstract partial class StringBase
    {
        private sealed class CharEnumerator : IEnumerator<char>
        {
            private readonly StringBase _source;
            private readonly int _length;

            private int _index;
            private char _current;
            private bool _isValidState;

            public CharEnumerator(StringBase str)
            {
                _source = str;
                _length = str.Length;
                Reset();
            }

            public char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!_isValidState)
                        throw new InvalidOperationException("Enumerator is not positioned within the string.");
                    return _current;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                bool state = MoveNextCore();
                _isValidState = state;
                return state;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool MoveNextCore()
            {
                int index = _index;
                if (index < 0)
                {
                    _index = 0;
                    if (_length > 0)
                    {
                        _current = _source.GetCharAt(0);
                        return true;
                    }
                    return false;
                }
                index++;
                if (index < _length)
                {
                    _index = index;
                    _current = _source.GetCharAt(unchecked((char)_index));
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = -1;
                _current = '\0';
                _isValidState = false;
            }
        }
    }
}
