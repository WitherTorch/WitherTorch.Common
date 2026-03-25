using System;

namespace WitherTorch.Common.Text
{
    partial class StringWrapper
    {
        public StringWrapper Substring(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return this;

            int length = Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return this;

            return unchecked(SubstringCore((nuint)startIndex, (nuint)(length - startIndex)));
        }

        public StringWrapper Substring(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int length = Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (startIndex == 0 && count == length)
                return this;
            if (count == 0)
                return Empty;

            return unchecked(SubstringCore((nuint)startIndex, (nuint)count));
        }

        protected internal abstract StringWrapper SubstringCore(nuint startIndex, nuint count);

        public StringWrapper Remove(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return Empty;

            int length = Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            return RemoveCore((nuint)startIndex, (nuint)(length - startIndex));
        }

        public StringWrapper Remove(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int length = Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (startIndex == 0 && count == length)
                return Empty;
            if (count == 0)
                return this;

            return RemoveCore((nuint)startIndex, (nuint)count);
        }

        protected abstract StringWrapper RemoveCore(nuint startIndex, nuint count);
    }
}
