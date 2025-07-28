using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        private struct CharEnumerator : IEnumerator<char>
        {
            private readonly byte[] _buffer;
            private readonly nuint _length;

            private nuint _cursor;
            private char _current, _current2;
            private bool _isValidState;

            public readonly nuint NextCursor => _cursor;

            public CharEnumerator(byte[] buffer)
            {
                _buffer = buffer;
                _length = MathHelper.MakeUnsigned(buffer.Length - 1);
                Reset();
            }

            public CharEnumerator(byte[] buffer, nuint cursor) : this(buffer)
            {
                _cursor = cursor;
            }

            public CharEnumerator(in CharEnumerator original)
            {
                _buffer = original._buffer;
                _length = original._length;
                _cursor = original._cursor;
                _current = original._current;
                _current2 = original._current2;
                _isValidState = original._isValidState;
            }

            public readonly char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!_isValidState)
                        throw new InvalidOperationException("Enumerator is not positioned within the string.");
                    return _current;
                }
            }

            readonly object IEnumerator.Current => Current;

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (_isValidState)
                {
                    char current = _current, current2 = _current2;
                    if (char.IsHighSurrogate(current) && char.IsLowSurrogate(current2))
                    {
                        _current = current2;
                        return true;
                    }
                }
                bool state = MoveNextCore();
                _isValidState = state;
                return state;
            }

            public void Reset()
            {
                _cursor = 0;
                _current = '\0';
                _current2 = '\0';
                _isValidState = false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private unsafe bool MoveNextCore()
            {
                nuint cursor = _cursor;
                nuint length = _length;
                if (cursor >= length)
                    return false;
                fixed (byte* ptr = _buffer)
                {
                    byte* ptrNext = Utf8EncodingHelper.TryReadUtf8Character(ptr + cursor, ptr + length, out uint codePoint);
                    if (ptrNext < ptr)
                    {
                        _cursor = length;
                        return false;
                    }
                    Utf8EncodingHelper.ToUtf16Characters(codePoint, out _current, out _current2);
                    _cursor = unchecked((nuint)(ptrNext - ptr));
                    return true;
                }
            }
        }
    }
}
