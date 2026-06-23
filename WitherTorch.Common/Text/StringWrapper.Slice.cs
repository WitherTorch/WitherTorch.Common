using System;

namespace WitherTorch.Common.Text;

partial class StringWrapper
{
    public StringSlice Slice(int startIndex)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        int length = Length;
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, length);
        if (length == 0)
            return StringSlice.Empty;
        return unchecked(new StringSlice(this, startIndex, length - startIndex));
    }

    public StringSlice Slice(int startIndex, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        int length = Length;
        if (startIndex + count > length)
            throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
        if (count == 0)
            return StringSlice.Empty;
        return unchecked(new StringSlice(this, startIndex, count));
    }

    public StringSlice Slice(nuint startIndex)
    {
        int length = Length;
        if (length <= 0)
            return StringSlice.Empty;
        nuint castedLength = unchecked((nuint)length);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, castedLength);
        return unchecked(new StringSlice(this, (int)startIndex, (int)(castedLength - startIndex)));
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
        return unchecked(new StringSlice(this, (int)startIndex, (int)count));
    }
}
