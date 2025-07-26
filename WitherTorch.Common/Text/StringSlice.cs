using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    public readonly struct StringSlice : IEnumerable<char>
    {
        public static readonly StringSlice Empty = new StringSlice(StringBase.Empty, 0u, 0u);

        private readonly StringBase _original;
        private readonly nuint _startIndex, _length;

        public StringBase Original => _original ?? StringBase.Empty;
        public nuint StartIndex => _startIndex;
        public nuint Length => _length;

        internal StringSlice(StringBase original, nuint startIndex, nuint length)
        {
            _original = original;
            _startIndex = startIndex;
            _length = length;
        }

        public char this[int index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException();
                nuint castedIndex = unchecked((nuint)index);
                if (castedIndex >= _length)
                    throw new IndexOutOfRangeException();
                return _original.GetCharAt(_startIndex + castedIndex);
            }
        }

        public static StringSlice Create(StringBase original)
        {
            int length = original.Length;
            if (length <= 0)
                return Empty;
            return new StringSlice(original, 0, unchecked((nuint)length));
        }

        public static StringSlice Create(StringBase original, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > original.Length)
                throw new ArgumentOutOfRangeException(startIndex >= original.Length ? nameof(startIndex) : nameof(length));
            if (length == 0)
                return Empty;
            return new StringSlice(original, unchecked((nuint)startIndex), unchecked((nuint)length));
        }

        public StringSlice Slice(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return this;
            nuint castedStartIndex = unchecked((nuint)startIndex);
            nuint length = _length;
            if (castedStartIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return this;
            return new StringSlice(_original, _startIndex + castedStartIndex, length - castedStartIndex);
        }

        public StringSlice Slice(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            nuint castedStartIndex = unchecked((nuint)startIndex);
            nuint castedCount = unchecked((nuint)count);
            nuint length = _length;
            if (castedStartIndex + castedCount > length)
                throw new ArgumentOutOfRangeException(castedStartIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return Empty;
            if (castedStartIndex == 0 && castedStartIndex + castedCount == length)
                return this;
            return new StringSlice(_original, _startIndex + castedStartIndex, castedCount);
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public override int GetHashCode()
        {
#if NET472_OR_GREATER
            return _original.GetHashCode() ^ _startIndex.GetHashCode() ^ _length.GetHashCode();
#else
            return HashCode.Combine(_original, _startIndex, _length);
#endif
        }

        public bool Equals(StringSlice other)
            => ReferenceEquals(_original, other._original) && _startIndex == other._startIndex && _length == other._length;

        public override bool Equals(object? obj) => obj is StringSlice other && Equals(other);

        public StringBase ToStringBase() => _original.SubstringCore(_startIndex, _length);

        public override unsafe string ToString()
        {
            nuint length = _length;
            if (length == 0)
                return string.Empty;
            StringBase original = _original;
            nuint startIndex = _startIndex;
            if (startIndex == 0 && length == unchecked((nuint)original.Length))
                return original.ToString();

            string result = StringHelper.AllocateRawString((int)length);
            fixed (char* ptr = result)
                original.CopyToCore(ptr, startIndex, length);
            return result;
        }

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
