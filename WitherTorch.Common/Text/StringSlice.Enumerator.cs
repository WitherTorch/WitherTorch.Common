using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Text
{
    partial struct StringSlice
    {
        public struct Enumerator : IEnumerator<char>
        {
            private readonly StringBase _original;
            private readonly nuint _startIndex, _length;

            private bool _initialized;
            private nuint _index;

            public Enumerator(StringSlice slice)
            {
                _original = slice._original;
                _startIndex = slice._startIndex;
                _length = slice._length;

                _initialized = false;
                _index = 0;
            }

            public readonly char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!_initialized)
                        throw new InvalidOperationException("Enumerator not initialized. Call MoveNext() first.");
                    nuint index = _index;
                    if (index >= _length)
                        throw new InvalidOperationException("Enumerator has reached the end of the collection.");
                    return _original.GetCharAt(_startIndex + index);
                }
            }

            readonly object IEnumerator.Current => Current;

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (!_initialized)
                {
                    _initialized = true;
                    _index = 0;
                    return _length > 0;
                }

                nuint nextIndex = _index + 1;
                if (nextIndex >= _length)
                    return false;

                _index = nextIndex;
                return true;
            }

            public void Reset() => _initialized = false;
        }
    }
}
