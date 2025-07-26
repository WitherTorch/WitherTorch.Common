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

        public override int CompareToCore(string other)
        {
            string value = _value;
            int length = value.Length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            return CompareToCoreUtf16(value, other, unchecked((nuint)length));
        }

        public override int CompareToCore(StringBase other)
        {
            string value = _value;
            int length = value.Length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            return other switch
            {
                Utf16String utf16 => CompareToCoreUtf16(value, utf16._value, unchecked((nuint)length)),
                Latin1String latin1 => CompareToCoreLatin1(value, latin1.GetInternalRepresentation(), unchecked((nuint)length)),
                Utf8String utf8 => CompareToCoreUtf8(value, utf8.GetInternalRepresentation(), unchecked((nuint)length), utf8.IsAsciiOnly),
                _ => CompareToCoreOther(value, other, unchecked((nuint)length)),
            };
        }

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
                Latin1String latin1 => EqualsCoreLatin1(value, latin1.GetInternalRepresentation(), unchecked((nuint)length)),
                Utf8String utf8 => EqualsCoreUtf8(value, utf8.GetInternalRepresentation(), unchecked((nuint)length), utf8.IsAsciiOnly),
                _ => EqualsCoreOther(value, other, unchecked((nuint)length)),
            };
        }

        private static unsafe int CompareToCoreLatin1(string a, byte[] b, nuint length)
        {
            fixed (char* ptrA = a)
            fixed (byte* ptrB = b)
                return -Latin1StringHelper.CompareTo_Utf16(ptrB, ptrA, length);
        }

        private static unsafe int CompareToCoreUtf8(string a, byte[] b, nuint length, bool isAsciiOnly)
        {
            fixed (char* ptrA = a)
            fixed (byte* ptrB = b)
            {
                if (isAsciiOnly)
                    return -Latin1StringHelper.CompareTo_Utf16(ptrB, ptrA, length);

                return -Utf8StringHelper.CompareTo_Utf16(ptrB, ptrA, length);
            }
        }

        private static unsafe int CompareToCoreUtf16(string a, string b, nuint length)
        {
            fixed (char* ptrA = a, ptrB = b)
                return InternalSequenceHelper.CompareTo(ptrA, ptrB, length);
        }

        private static unsafe int CompareToCoreOther(string a, StringBase b, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            fixed (char* ptrBuffer = buffer)
            {
                b.CopyToCore(ptrBuffer, 0, length);
                fixed (char* ptrA = a)
                    return InternalSequenceHelper.CompareTo(ptrA, ptrBuffer, length);
            }
        }

        private static unsafe bool EqualsCoreLatin1(string a, byte[] b, nuint length)
        {
            fixed (char* ptrA = a)
            fixed (byte* ptrB = b)
                return Latin1StringHelper.Equals_Utf16(ptrB, ptrA, length);
        }

        private static unsafe bool EqualsCoreUtf8(string a, byte[] b, nuint length, bool isAsciiOnly)
        {
            fixed (char* ptrA = a)
            fixed (byte* ptrB = b)
            {
                if (isAsciiOnly)
                    return Latin1StringHelper.Equals_Utf16(ptrB, ptrA, length);

                return Utf8StringHelper.Equals_Utf16(ptrB, ptrA, length);
            }
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
            fixed (char* ptrBuffer = buffer)
            {
                b.CopyToCore(ptrBuffer, 0, length);
                fixed (char* ptrA = a)
                    return SequenceHelper.Equals(ptrA, ptrBuffer, length);
            }
        }
    }
}
