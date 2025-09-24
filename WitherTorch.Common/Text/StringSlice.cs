using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    public readonly partial struct StringSlice
    {
        public static readonly StringSlice Empty = new StringSlice(StringBase.Empty, 0, 0);

        private readonly StringBase _original;
        private readonly int _startIndex, _length;

        public StringBase Original => _original ?? StringBase.Empty;
        public int StartIndex => _startIndex;
        public int Length => _length;

        internal StringSlice(StringBase original, int startIndex, int length)
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
                if (index >= _length)
                    throw new IndexOutOfRangeException();
                return _original.GetCharAt(unchecked((nuint)(_startIndex + index)));
            }
        }

        public static StringSlice Create(StringBase original)
        {
            int length = original.Length;
            if (length <= 0)
                return Empty;
            return new StringSlice(original, 0, length);
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
            return new StringSlice(original, startIndex, length);
        }

        public StringSlice Slice(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return this;
            int length = _length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return this;
            return new StringSlice(_original, _startIndex + startIndex, length - startIndex);
        }

        public StringSlice Slice(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = _length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return Empty;
            if (startIndex == 0 && startIndex + count == length)
                return this;
            return new StringSlice(_original, _startIndex + startIndex, count);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in StringSlice left, in StringSlice right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in StringSlice left, in StringSlice right) => !left.Equals(right);

        public bool Equals(StringSlice other)
            => ReferenceEquals(_original, other._original) && _startIndex == other._startIndex && _length == other._length;

        public override bool Equals(object? obj) => obj is StringSlice other && Equals(other);

        public StringBase ToStringBase() => _original.SubstringCore((nuint)_startIndex, (nuint)_length);

        public unsafe char[] ToCharArray()
        {
            int length = _length;
            if (length <= 0)
                return Array.Empty<char>();
            StringBase original = _original;
            int startIndex = _startIndex;
            if (startIndex == 0 && length == original.Length)
                return original.ToCharArray();

            char[] result = new char[length];
            fixed (char* ptr = result)
                original.CopyToCore(ptr, (nuint)startIndex, (nuint)length);
            return result;
        }

        public override unsafe string ToString()
        {
            int length = _length;
            if (length <= 0)
                return string.Empty;
            StringBase original = _original;
            int startIndex = _startIndex;
            if (startIndex == 0 && length == original.Length)
                return original.ToString();

            string result = StringHelper.AllocateRawString(length);
            fixed (char* ptr = result)
                original.CopyToCore(ptr, (nuint)startIndex, (nuint)length);
            return result;
        }
    }
}
