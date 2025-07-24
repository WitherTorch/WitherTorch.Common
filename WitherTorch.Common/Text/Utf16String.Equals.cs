using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Utf16String utf16 => PartiallyEqualsCore(utf16._value, startIndex, count),
                _ => PartiallyEqualsCoreForOther(other, startIndex, count),
            };

        private unsafe bool PartiallyEqualsCore(char* other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
                return SequenceHelper.Equals(ptr + startIndex, other, count);
        }

        private unsafe bool PartiallyEqualsCoreForOther(StringBase other, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                unchecked
                {
                    fixed (char* ptrBuffer = buffer)
                    {
                        other.CopyTo(ptrBuffer, (int)startIndex, (int)count);
                        fixed (char* ptrSource = _value)
                            return SequenceHelper.Equals(ptrSource + startIndex, ptrBuffer, count);
                    }
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        public override int CompareToCore(string other) => _value.CompareTo(other);

        public override int CompareToCore(StringBase other) => other switch
        {
            Utf16String utf16 => CompareToCore(utf16),
            Latin1String latin1 => CompareToCore(latin1),
            _ => base.CompareToCore(other),
        };

        public override bool EqualsCore(string other)
        {
            string value = _value;
            int length = value.Length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return EqualsCoreUtf16(value, other, unchecked((nuint)length));
        }

        public override bool EqualsCore(StringBase other)
        {
            string value = _value;
            int length = value.Length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return other switch
            {
                Utf16String utf16 => EqualsCoreUtf16(value, utf16._value, unchecked((nuint)length)),
                _ => EqualsCoreOther(value, other, unchecked((nuint)length)),
            };
        }

        private static unsafe bool EqualsCoreUtf16(string a, string b, nuint length)
        {
            fixed (char* ptrA = a, ptrB = b)
                return SequenceHelper.Equals(ptrA, ptrB, length);
        }

        private static unsafe bool EqualsCoreOther(string a, StringBase b, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            try
            {
                unchecked
                {
                    fixed (char* ptrBuffer = buffer)
                    {
                        b.CopyTo(ptrBuffer, 0, (int)length);
                        fixed (char* ptrA = a)
                            return SequenceHelper.Equals(ptrA, ptrBuffer, length);
                    }
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private int CompareToCore(Utf16String other) => _value.CompareTo(other._value);

        private unsafe int CompareToCore(Latin1String other)
        {
            int length = _value.Length;
            int comparison = other.Length.CompareTo(length);
            if (comparison != 0 || length <= 0)
                return comparison;
            fixed (char* ptrA = _value)
            fixed (byte* ptrB = other.GetInternalRepresentation())
                return -InternalStringHelper.CompareTo_LU(ptrB, ptrA, unchecked((nuint)length));
        }
    }
}
