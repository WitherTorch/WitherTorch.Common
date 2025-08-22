using System;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        public StringSlice Slice(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            int length = Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return StringSlice.Empty;
            return unchecked(new StringSlice(this, (nuint)startIndex, (nuint)(length - startIndex)));
        }

        public StringSlice Slice(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return StringSlice.Empty;
            return unchecked(new StringSlice(this, (nuint)startIndex, (nuint)count));
        }

        public StringSlice Slice(nuint startIndex)
        {
            int length = Length;
            if (length <= 0)
                return StringSlice.Empty;
            nuint castedLength = unchecked((nuint)length);
            if (startIndex >= castedLength)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            return unchecked(new StringSlice(this, startIndex, castedLength - startIndex));
        }

        public StringSlice Slice(nuint startIndex, nuint count)
        {
            int length = Length;
            if (length <= 0)
                return StringSlice.Empty;
            nuint castedLength = unchecked((nuint)length);
            if (startIndex + count > castedLength)
                throw new ArgumentOutOfRangeException(startIndex >= castedLength ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return StringSlice.Empty;
            return unchecked(new StringSlice(this, startIndex, count));
        }
    }
}
