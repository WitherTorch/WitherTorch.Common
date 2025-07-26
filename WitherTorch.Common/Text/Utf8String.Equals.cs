using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
            => PartiallyEqualsCoreUtf16(_value, other, startIndex, count, _isAsciiOnly);

        protected override unsafe bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Utf8String utf8 => PartiallyEqualsCoreUtf8(_value, utf8._value, startIndex, count, _isAsciiOnly && utf8._isAsciiOnly),
                Utf16String utf16 => PartiallyEqualsCoreUtf16(_value, utf16.GetInternalRepresentation(), startIndex, count, _isAsciiOnly),
                Latin1String latin1 => PartiallyEqualsCoreLatin1(_value, latin1.GetInternalRepresentation(), startIndex, count, _isAsciiOnly),
                _ => PartiallyEqualsCoreOther(_value, other, startIndex, count)
            };

        public override int CompareToCore(string other)
        {
            int length = _length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            return CompareToCoreUtf16(_value, other, unchecked((nuint)length), _isAsciiOnly);
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
                Utf8String utf8 => CompareToCoreUtf8(value, utf8._value, unchecked((nuint)length)),
                Utf16String utf16 => CompareToCoreUtf16(value, utf16.GetInternalRepresentation(), unchecked((nuint)length), _isAsciiOnly),
                Latin1String latin1 => CompareToCoreLatin1(value, latin1.GetInternalRepresentation(), unchecked((nuint)length), _isAsciiOnly),
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
            return EqualsCoreUtf16(value, other, unchecked((nuint)length), _isAsciiOnly);
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
                Utf8String utf8 => EqualsCoreUtf8(value, utf8._value, unchecked((nuint)length)),
                Utf16String utf16 => EqualsCoreUtf16(value, utf16.GetInternalRepresentation(), unchecked((nuint)length), _isAsciiOnly),
                Latin1String latin1 => EqualsCoreLatin1(value, latin1.GetInternalRepresentation(), unchecked((nuint)length), _isAsciiOnly),
                _ => base.EqualsCore(other),
            };
        }

        private static unsafe bool PartiallyEqualsCoreLatin1(byte[] a, byte[] b, nuint startIndex, nuint count, bool isAsciiOnly)
        {
            fixed (byte* ptrB = b)
            {
                if (isAsciiOnly && !SequenceHelper.ContainsGreaterThan(ptrB, count, AsciiCharacterLimit))
                {
                    fixed (byte* ptrA = a)
                        return SequenceHelper.Equals(ptrA + startIndex, ptrB, count * sizeof(byte));
                }

                using CharEnumerator enumerator = new CharEnumerator(a);
                for (nuint i = 0; i < startIndex; i++)
                {
                    if (!enumerator.MoveNext())
                        return false;
                }
                for (nuint i = 0; i < count; i++)
                {
                    if (!enumerator.MoveNext() || enumerator.Current != ptrB[i])
                        return false;
                }
            }
            return true;
        }

        private static unsafe bool PartiallyEqualsCoreUtf8(byte[] a, byte[] b, nuint startIndex, nuint count, bool isAsciiOnly)
        {
            if (isAsciiOnly)
            {
                fixed (byte* ptrA = a, ptrB = b)
                    return SequenceHelper.Equals(ptrA + startIndex, ptrB, count * sizeof(byte));
            }

            using CharEnumerator enumeratorA = new CharEnumerator(a);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumeratorA.MoveNext())
                    return false;
            }

            using CharEnumerator enumeratorB = new CharEnumerator(a);
            for (nuint i = 0; i < count; i++)
            {
                if (!enumeratorA.MoveNext() || !enumeratorB.MoveNext() ||
                    enumeratorA.Current != enumeratorB.Current)
                    return false;
            }
            return true;
        }

        private static unsafe bool PartiallyEqualsCoreUtf16(byte[] a, string b, nuint startIndex, nuint count, bool isAsciiOnly)
        {
            fixed (char* ptrB = b)
            {
                if (!SequenceHelper.ContainsGreaterThan(ptrB, count, (char)AsciiCharacterLimit))
                    return PartiallyEqualsCoreUtf16_VeryFast(a, ptrB, startIndex, count);
                if (isAsciiOnly)
                    return PartiallyEqualsCoreUtf16_Fast(a, ptrB, startIndex, count);
                return PartiallyEqualsCoreUtf16_Slow(a, ptrB, startIndex, count);
            }
        }

        private static unsafe bool PartiallyEqualsCoreUtf16_VeryFast(byte[] a, char* b, nuint startIndex, nuint count)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(count);
            try
            {
                fixed (byte* ptrA = a, ptrBuffer = buffer)
                {
                    Latin1StringHelper.NarrowAndCopyTo(b, count, ptrBuffer);
                    return SequenceHelper.Equals(ptrA + startIndex, ptrBuffer, count * sizeof(byte));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private static unsafe bool PartiallyEqualsCoreUtf16_Fast(byte[] a, char* b, nuint startIndex, nuint count)
        {
            fixed (byte* ptrA = a)
            {
                byte* offsetedPtrA = ptrA + startIndex;
                for (nuint i = 0; i < count; i++)
                {
                    if (offsetedPtrA[i] != b[i])
                        return false;
                }
            }
            return true;
        }

        private static unsafe bool PartiallyEqualsCoreUtf16_Slow(byte[] a, char* b, nuint startIndex, nuint count)
        {
            using CharEnumerator enumerator = new CharEnumerator(a);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumerator.MoveNext())
                    return false;
            }
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext() || enumerator.Current != b[i])
                    return false;
            }
            return true;
        }

        private static unsafe bool PartiallyEqualsCoreOther(byte[] a, StringBase b, nuint startIndex, nuint count)
        {
            using CharEnumerator enumerator = new CharEnumerator(a);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumerator.MoveNext())
                    return false;
            }
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* ptr = buffer)
                {
                    b.CopyToCore(ptr, 0, count);
                    for (nuint i = 0; i < count; i++)
                    {
                        if (!enumerator.MoveNext() || enumerator.Current != ptr[i])
                            return false;
                    }
                }
            }
            finally
            {
                pool.Return(buffer);
            }
            return true;
        }

        private static unsafe int CompareToCoreLatin1(byte[] a, byte[] b, nuint length, bool isAsciiOnly)
        {
            fixed (byte* ptrA = a, ptrB = b)
            {
                if (isAsciiOnly && !SequenceHelper.ContainsGreaterThan(ptrB, length, AsciiCharacterLimit))
                    return InternalSequenceHelper.CompareTo(ptrA, ptrB, length);

                return Utf8StringHelper.CompareTo_Latin1(ptrA, ptrB, length);
            }
        }

        private static unsafe int CompareToCoreUtf8(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return InternalSequenceHelper.CompareTo(ptrA, ptrB, length);
        }

        private static unsafe int CompareToCoreUtf16(byte[] a, string b, nuint length, bool isAsciiOnly)
        {
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
            {
                if (isAsciiOnly)
                    return Latin1StringHelper.CompareTo_Utf16(ptrA, ptrB, length);
                return Utf8StringHelper.CompareTo_Utf16(ptrA, ptrB, length);
            }
        }

        private static unsafe bool EqualsCoreLatin1(byte[] a, byte[] b, nuint length, bool isAsciiOnly)
        {
            fixed (byte* ptrA = a, ptrB = b)
            {
                if (isAsciiOnly)
                    return SequenceHelper.Equals(ptrA, ptrB, length);
                return Utf8StringHelper.Equals_Latin1(ptrA, ptrB, length);
            }
        }

        private static unsafe bool EqualsCoreUtf8(byte[] a, byte[] b, nuint length)
        {
            fixed (byte* ptrA = a, ptrB = b)
                return SequenceHelper.Equals(ptrA, ptrB, length);
        }

        private static unsafe bool EqualsCoreUtf16(byte[] a, string b, nuint length, bool isAsciiOnly)
        {
            fixed (byte* ptrA = a)
            fixed (char* ptrB = b)
            {
                if (isAsciiOnly)
                    return Latin1StringHelper.Equals_Utf16(ptrA, ptrB, length);

                return Utf8StringHelper.Equals_Utf16(ptrA, ptrB, length);
            }
        }
    }
}
