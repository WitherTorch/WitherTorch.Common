using System;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        public int IndexOf(char value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            return IndexOfCore(value, 0, unchecked((nuint)length));
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)startIndex, (nuint)count));
        }

        public int IndexOf(string value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return 0;
            if (valueLength > length)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public int IndexOf(string value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            length = value.Length;
            if (length <= 0)
                return 0;
            if (length > count)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)length, 0, (nuint)count));
        }

        public int IndexOf(StringBase value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return 0;
            if (valueLength > length)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public int IndexOf(StringBase value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            length = value.Length;
            if (length <= 0)
                return 0;
            if (length > count)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)length, 0, (nuint)count));
        }

        public bool Contains(char value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            return ContainsCore(value, 0, unchecked((nuint)length));
        }

        public bool Contains(char value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            return unchecked(ContainsCore(value, (nuint)startIndex, (nuint)length));
        }

        public bool Contains(string value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return true;
            if (valueLength > length)
                return false;
            return unchecked(ContainsCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public bool Contains(string value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            length = value.Length;
            if (length <= 0)
                return true;
            if (length > count)
                return false;
            return unchecked(ContainsCore(value, (nuint)length, 0, (nuint)count));
        }

        public bool Contains(StringBase value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return true;
            if (valueLength > length)
                return false;
            return unchecked(ContainsCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public bool Contains(StringBase value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            length = value.Length;
            if (length <= 0)
                return true;
            if (length > count)
                return false;
            return unchecked(ContainsCore(value, (nuint)length, 0, (nuint)count));
        }

        protected abstract int IndexOfCore(char value, nuint startIndex, nuint count);

        protected abstract int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count);

        protected virtual int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => IndexOfCore(value.ToString(), valueLength, startIndex, count);

        protected abstract bool ContainsCore(char value, nuint startIndex, nuint count);

        protected abstract bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count);

        protected virtual bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => ContainsCore(value.ToString(), valueLength, startIndex, count);
    }
}
