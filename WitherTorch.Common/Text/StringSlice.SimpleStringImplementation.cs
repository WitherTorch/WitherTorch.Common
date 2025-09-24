using System;

namespace WitherTorch.Common.Text
{
    partial struct StringSlice : ISimpleString
    {
        public bool Contains(char value)
            => _original.Contains(value, _startIndex, _length);

        public bool Contains(char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            return _original.Contains(value, _startIndex + startIndex, count);
        }

        public bool Contains(string value)
            => _original.Contains(value, _startIndex, _length);

        public bool Contains(string value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            return _original.Contains(value, _startIndex + startIndex, count);
        }

        public bool Contains(StringBase value)
            => _original.Contains(value, _startIndex, _length);

        public bool Contains(StringBase value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            return _original.Contains(value, _startIndex + startIndex, count);
        }

        public bool EndsWith(char value)
        {
            if (value == '\0')
                return true;
            int length = _length;
            if (length <= 0)
                return false;
            return _original.GetCharAt((nuint)(_startIndex + length - 1)) == value;
        }

        public bool EndsWith(string value)
        {
            int length = _length;
            int valueLength = value.Length;
            if (length < valueLength)
                return false;
            return _original.PartiallyEquals(value, _startIndex + length - valueLength);
        }

        public bool EndsWith(StringBase value)
        {
            int length = _length;
            int valueLength = value.Length;
            if (length < valueLength)
                return false;
            return _original.PartiallyEquals(value, _startIndex + length - valueLength);
        }

        public int IndexOf(char value)
        {
            int startIndex = _startIndex;
            int result = _original.IndexOf(value, startIndex, _length);
            return result < 0 ? -1 : result - startIndex;
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            int offset = _startIndex;
            int result = _original.IndexOf(value, offset + startIndex, count);
            return result < 0 ? -1 : result - offset;
        }

        public int IndexOf(string value)
        {
            int startIndex = _startIndex;
            int result = _original.IndexOf(value, startIndex, _length);
            return result < 0 ? -1 : result - startIndex;
        }

        public int IndexOf(string value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            int offset = _startIndex;
            int result = _original.IndexOf(value, offset + startIndex, count);
            return result < 0 ? -1 : result - offset;
        }

        public int IndexOf(StringBase value)
        {
            int startIndex = _startIndex;
            int result = _original.IndexOf(value, startIndex, _length);
            return result < 0 ? -1 : result - startIndex;
        }

        public int IndexOf(StringBase value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            int offset = _startIndex;
            int result = _original.IndexOf(value, offset + startIndex, count);
            return result < 0 ? -1 : result - offset;
        }

        public bool PartiallyEquals(string other, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            return _original.PartiallyEquals(other, _startIndex + startIndex, _length);
        }

        public bool PartiallyEquals(string other, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            return _original.PartiallyEquals(other, _startIndex + startIndex, count);
        }

        public bool PartiallyEquals(StringBase other, int startIndex)
        {
            if (startIndex < 0 || startIndex >= _length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            return _original.PartiallyEquals(other, _startIndex + startIndex, _length);
        }

        public bool PartiallyEquals(StringBase other, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > _length)
                throw new ArgumentOutOfRangeException(startIndex >= _length ? nameof(startIndex) : nameof(count));
            return _original.PartiallyEquals(other, _startIndex + startIndex, count);
        }

        public StringSlice Slice(nuint startIndex)
        {
            if (startIndex == 0)
                return this;
            nuint length = (nuint)_length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return this;
            return new StringSlice(_original, _startIndex + (int)startIndex, (int)(length - startIndex));
        }

        public StringSlice Slice(nuint startIndex, nuint count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            nuint length = (nuint)_length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return Empty;
            if (startIndex == 0 && startIndex + count == length)
                return this;
            return new StringSlice(_original, _startIndex + (int)startIndex, (int)count);
        }

        public bool StartsWith(char value)
        {
            if (value == '\0')
                return true;
            int length = _length;
            if (length <= 0)
                return false;
            return _original.GetCharAt((nuint)_startIndex) == value;
        }

        public bool StartsWith(string value)
        {
            int length = _length;
            int valueLength = value.Length;
            if (length < valueLength)
                return false;
            return _original.PartiallyEquals(value, _startIndex);
        }

        public bool StartsWith(StringBase value)
        {
            int length = _length;
            int valueLength = value.Length;
            if (length < valueLength)
                return false;
            return _original.PartiallyEquals(value, _startIndex);
        }
    }
}
