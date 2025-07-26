using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Latin1String
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
            => PartiallyEqualsCoreUtf16(_value, other, startIndex, count);

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Latin1String latin1 => PartiallyEqualsCoreLatin1(_value, latin1._value, startIndex, count),
                Utf16String utf16 => PartiallyEqualsCoreUtf16(_value, utf16.GetInternalRepresentation(), startIndex, count),
                Utf8String utf8 => PartiallyEqualsCoreUtf8(_value, utf8.GetInternalRepresentation(), startIndex, count),
                _ => base.PartiallyEqualsCore(other, startIndex, count),
            };

        public override int GetHashCode() => HashingHelper.GetHashCode(this);

        public override int CompareToCore(string other)
        {
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
                Utf8String utf8 => CompareToCoreUtf8(value, utf8.GetInternalRepresentation(), unchecked((nuint)length)),
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
                Utf8String utf8 => EqualsCoreUtf8(value, utf8.GetInternalRepresentation(), unchecked((nuint)length)),
                _ => base.EqualsCore(other),
            };
        }

        private static unsafe bool PartiallyEqualsCoreLatin1(byte[] a, byte[] b, nuint startIndex, nuint count)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return SequenceHelper.Equals(ptrA + startIndex, ptrB, count);
        }

        private static unsafe bool PartiallyEqualsCoreUtf8(byte[] a, byte[] b, nuint startIndex, nuint count)
        {
            fixed (byte* ptrA = a, ptrB = b)
            {
                byte* iteratorA = ptrA + startIndex, iteratorB = ptrB;
                byte* end = (byte*)UnsafeHelper.PointerMaxValue;
                for (nuint i = 0; i < count; i++)
                {
                    if ((iteratorB = Utf8StringHelper.TryReadUtf8Character(iteratorB, end, out uint unicodeValue)) == null ||
                        unicodeValue > Latin1StringHelper.Latin1StringLimit ||
                        iteratorA[i] != unicodeValue)
                        return false;
                }
            }
            return true;
        }

        private static unsafe bool PartiallyEqualsCoreUtf16(byte[] a, string b, nuint startIndex, nuint count)
        {
            fixed (char* ptrB = b)
            {
                if (SequenceHelper.ContainsGreaterThan(ptrB, count, Latin1StringHelper.Latin1StringLimit))
                    return false;
                ArrayPool<byte> pool = ArrayPool<byte>.Shared;
                byte[] buffer = pool.Rent(count);
                try
                {
                    fixed (byte* ptrA = a, ptrBuffer = buffer)
                    {
                        Latin1StringHelper.NarrowAndCopyTo(ptrB, count, ptrBuffer);
                        return SequenceHelper.Equals(ptrA + startIndex, ptrBuffer, count);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }

        private static unsafe int CompareToCoreLatin1(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return InternalSequenceHelper.CompareTo(ptrA, ptrB, length);
        }

        private static unsafe int CompareToCoreUtf16(byte[] a, string b, nuint length)
        {
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
            {
                return Latin1StringHelper.CompareTo_Utf16(ptrA, ptrB, length);
            }
        }

        private static unsafe int CompareToCoreUtf8(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return -Utf8StringHelper.CompareTo_Latin1(ptrB, ptrA, length);
        }

        private static unsafe bool EqualsCoreLatin1(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return SequenceHelper.Equals(ptrA, ptrB, length);
        }

        private static unsafe bool EqualsCoreUtf16(byte[] a, string b, nuint length)
        {
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
                return Latin1StringHelper.Equals_Utf16(ptrA, ptrB, length);
        }

        private static unsafe bool EqualsCoreUtf8(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return Utf8StringHelper.Equals_Latin1(ptrB, ptrA, length);
        }
    }
}
