using System;
using System.Linq;

using InlineMethod;

using WitherTorch.Common.Extensions;

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
            int length = Length;
            int comparison = length.CompareTo(other.Length);
            if (comparison != 0)
                return comparison;
            if (length <= 0)
                return 0;
            return CompareToCore(other, unchecked((nuint)length));
        }

        public int CompareTo(StringBase? other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            int length = Length;
            int comparison = length.CompareTo(other.Length);
            if (comparison != 0)
                return comparison;
            if (length <= 0)
                return 0;
            return CompareToCore(other, unchecked((nuint)length));
        }

        public bool Equals(string? other)
        {
            if (other is null)
                return false;
            return Equals_Inline(other);
        }

        public bool Equals(StringBase? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals_Inline(other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj switch
            {
                string other => Equals_Inline(other),
                StringBase other => Equals_Inline(other),
                _ => false
            };
        }

        [Inline(InlineBehavior.Remove)]
        private bool Equals_Inline(string other)
        {
            int length = Length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return EqualsCore(other, unchecked((nuint)length));
        }

        [Inline(InlineBehavior.Remove)]
        private bool Equals_Inline(StringBase other)
        {
            int length = Length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return EqualsCore(other, unchecked((nuint)length));
        }

        protected virtual unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
            {
                if (count == 1)
                    return GetCharAt(startIndex) == ptr[startIndex];

                return PartiallyEqualsCoreFallback(ptr, startIndex, count);
            }
        }

        protected virtual unsafe bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
        {
            if (count == 1)
                return GetCharAt(startIndex) == other.GetCharAt(startIndex);

            if (other is Utf16String utf16)
            {
                fixed (char* ptr = utf16.GetInternalRepresentation())
                    return PartiallyEqualsCoreFallback(ptr, startIndex, count);
            }
            return PartiallyEqualsCoreFallback(other, startIndex, count);
        }

        protected virtual unsafe int CompareToCore(string other, nuint length)
        {
            fixed (char* ptr = other)
            {
                if (length == 1)
                    return GetCharAt(0).CompareTo(*ptr);

                return CompareToCoreFallback(ptr, length);
            }
        }

        protected virtual unsafe int CompareToCore(StringBase other, nuint length)
        {
            if (length == 1)
                return GetCharAt(0).CompareTo(other.GetCharAt(0));

            if (other is Utf16String utf16)
            {
                fixed (char* ptr = utf16.GetInternalRepresentation())
                    return CompareToCoreFallback(ptr, length);
            }

            return CompareToCoreFallback(other);
        }

        protected virtual unsafe bool EqualsCore(string other, nuint length)
        {
            fixed (char* ptr = other)
            {
                if (length == 1)
                    return GetCharAt(0) == *ptr;

                return EqualsCoreFallback(ptr, length);
            }
        }

        protected virtual unsafe bool EqualsCore(StringBase other, nuint length)
        {
            if (length == 1)
                return GetCharAt(0) == other.GetCharAt(0);

            if (other is Utf16String utf16)
            {
                fixed (char* ptr = utf16.GetInternalRepresentation())
                    return EqualsCoreFallback(ptr, length);
            }

            return EqualsCoreFallback(other);
        }

        [Inline(InlineBehavior.Remove)]
        private unsafe bool PartiallyEqualsCoreFallback(char* other, nuint startIndex, nuint count)
            => this.SkipAndTake(startIndex, count).SequenceEqual(other, count);

        [Inline(InlineBehavior.Remove)]
        private unsafe bool PartiallyEqualsCoreFallback(StringBase other, nuint startIndex, nuint count)
            => this.SkipAndTake(startIndex, count).SequenceEqual(other);

        [Inline(InlineBehavior.Remove)]
        private unsafe int CompareToCoreFallback(char* other, nuint length)
            => this.SequenceCompare(other, length);

        [Inline(InlineBehavior.Remove)]
        private unsafe int CompareToCoreFallback(StringBase other)
            => this.SequenceCompare(other);

        [Inline(InlineBehavior.Remove)]
        private unsafe bool EqualsCoreFallback(char* other, nuint length)
            => this.SequenceEqual(other, length);

        [Inline(InlineBehavior.Remove)]
        private unsafe bool EqualsCoreFallback(StringBase other)
            => this.SequenceEqual(other);
    }
}
