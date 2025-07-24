using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Latin1String
    {

        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Latin1String latin1 => PartiallyEqualsCore(latin1._value, startIndex, count),
                Utf16String utf16 => PartiallyEqualsCore(utf16.GetInternalRepresentation(), startIndex, count),
                _ => base.PartiallyEqualsCore(other, startIndex, count),
            };

        private unsafe bool PartiallyEqualsCore(char* other, nuint startIndex, nuint count)
        {
            if (SequenceHelper.ContainsGreaterThan(other, count, InternalStringHelper.Latin1StringLimit))
                return false;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(count);
            try
            {
                fixed (byte* ptrSource = _value, ptrBuffer = buffer)
                {
                    InternalStringHelper.NarrowAndCopyTo(other, count, ptrBuffer);
                    return SequenceHelper.Equals(ptrSource + startIndex, ptrBuffer, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool PartiallyEqualsCore(byte[] other, nuint startIndex, nuint count)
        {
            fixed (byte* ptrSource = _value, ptrOther = other)
                return SequenceHelper.Equals(ptrSource + startIndex, ptrOther, count);
        }

        public override int GetHashCode() => HashingHelper.GetHashCode(this);

        public override int CompareToCore(string other)
        {
            byte[] value = _value;
            int length = _length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            return CompareToCoreUtf16(_value, other, unchecked((nuint)length));
        }

        public override int CompareToCore(StringBase other)
        {
            byte[] value = _value;
            int length = _length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            return other switch
            {
                Latin1String latin1 => CompareToCoreLatin1(value, latin1._value, unchecked((nuint)length)),
                Utf16String utf16 => CompareToCoreUtf16(value, utf16.GetInternalRepresentation(), unchecked((nuint)length)),
                _ => base.CompareToCore(other),
            };
        }

        public override bool EqualsCore(string other)
        {
            byte[] value = _value;
            int length = _length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return EqualsCoreUtf16(value, other, unchecked((nuint)length));
        }

        public override bool EqualsCore(StringBase other)
        {
            byte[] value = _value;
            int length = _length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return other switch
            {
                Latin1String latin1 => EqualsCoreLatin1(value, latin1._value, unchecked((nuint)length)),
                Utf16String utf16 => EqualsCoreUtf16(value, utf16.GetInternalRepresentation(), unchecked((nuint)length)),
                _ => base.EqualsCore(other),
            };
        }

        private unsafe int CompareToCoreLatin1(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return InternalStringHelper.CompareTo_LL(ptrA, ptrB, length);
        }

        private unsafe int CompareToCoreUtf16(byte[] a, string b, nuint length)
        {
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
            {
                return InternalStringHelper.CompareTo_LU(ptrA, ptrB, length);
            }
        }

        private unsafe bool EqualsCoreLatin1(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return SequenceHelper.Equals(ptrA, ptrB, length);
        }

        private unsafe bool EqualsCoreUtf16(byte[] a, string b, nuint length)
        {
            if (length <= 0)
                return true;
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
            {
                if (SequenceHelper.ContainsGreaterThan(ptrB, length, InternalStringHelper.Latin1StringLimit))
                    return false;
                ArrayPool<byte> pool = ArrayPool<byte>.Shared;
                byte[] buffer = pool.Rent(length);
                try
                {
                    fixed (byte* dest = buffer)
                    {
                        InternalStringHelper.NarrowAndCopyTo(ptrB, length, dest);
                        return SequenceHelper.Equals(ptrA, dest, length);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }
    }
}
