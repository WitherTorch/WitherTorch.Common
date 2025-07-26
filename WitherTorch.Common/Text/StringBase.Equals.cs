using System;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        public bool StartsWith(char value)
        {
            if (Length <= 0)
                return false;
            return GetCharAt(0) == value;
        }

        public bool StartsWith(string value)
        {
            int length = Length;
            int valueLength = value.Length;
            if (length <= 0 || length <= valueLength)
                return false;
            if (valueLength <= 0)
                return true;
            return PartiallyEqualsCore(value, 0, unchecked((nuint)valueLength));
        }

        public bool StartsWith(StringBase value)
        {
            int length = Length;
            int valueLength = value.Length;
            if (length <= 0 || length <= valueLength)
                return false;
            if (valueLength <= 0)
                return true;
            return PartiallyEqualsCore(value, 0, unchecked((nuint)valueLength));
        }

        public bool EndsWith(char value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            return GetCharAt((nuint)(length - 1)) == value;
        }

        public bool EndsWith(string value)
        {
            int length = Length;
            int valueLength = value.Length;
            if (length <= 0 || length <= valueLength)
                return false;
            if (valueLength <= 0)
                return true;
            return unchecked(PartiallyEqualsCore(value, (nuint)(length - valueLength), (nuint)valueLength));
        }

        public bool EndsWith(StringBase value)
        {
            int length = Length;
            int valueLength = value.Length;
            if (length <= 0 || length <= valueLength)
                return false;
            if (valueLength <= 0)
                return true;
            return unchecked(PartiallyEqualsCore(value, (nuint)(length - valueLength), (nuint)valueLength));
        }

        public bool Equals(string? other)
        {
            if (other is null)
                return false;
            return EqualsCore(other);
        }

        public bool Equals(StringBase? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EqualsCore(other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj switch
            {
                string str => EqualsCore(str),
                StringBase str => EqualsCore(str),
                _ => false,
            };
        }

        public bool PartiallyEquals(string other, int startIndex)
        {
            int length = Length;
            int otherLength = other.Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (otherLength == 0)
                return true;
            if (otherLength > length)
                return false;
            if (startIndex + otherLength > length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            return unchecked(PartiallyEqualsCore(other, (nuint)startIndex, (nuint)otherLength));
        }

        public bool PartiallyEquals(string other, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > other.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return true;
            return unchecked(PartiallyEqualsCore(other, (nuint)startIndex, (nuint)count));
        }

        public bool PartiallyEquals(StringBase other, int startIndex)
        {
            int length = Length;
            int otherLength = other.Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (otherLength == 0)
                return true;
            if (otherLength > length)
                return false;
            if (startIndex + otherLength > length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            return unchecked(PartiallyEqualsCore(other, (nuint)startIndex, (nuint)otherLength));
        }

        public bool PartiallyEquals(StringBase other, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > other.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return true;
            return unchecked(PartiallyEqualsCore(other, (nuint)startIndex, (nuint)count));
        }

        public int CompareTo(string? other)
        {
            if (other is null)
                return 1;
            return CompareToCore(other);
        }

        public int CompareTo(StringBase? other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            return CompareToCore(other);
        }

        public virtual bool EqualsCore(string other) => ToString().Equals(other, StringComparison.Ordinal);

        public virtual bool EqualsCore(StringBase other) => ToString().Equals(other.ToString(), StringComparison.Ordinal);

        protected virtual unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
            {
                for (nuint i = 0; i < count; i++)
                {
                    if (GetCharAt(startIndex + i) != ptr[i])
                        return false;
                }
            }
            return true;
        }

        protected virtual bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if (GetCharAt(startIndex + i) != other.GetCharAt(i))
                    return false;
            }
            return true;
        }

        public virtual int CompareToCore(string other) => ToString().CompareTo(other);

        public virtual int CompareToCore(StringBase other) => ToString().CompareTo(other.ToString());
    }
}
