using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class AsciiLikeString
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                AsciiLikeString ascii => PartiallyEqualsCore(ascii, startIndex, count),
                Utf16String utf16 => PartiallyEqualsCore(utf16, startIndex, count),
                Utf8String utf8 => PartiallyEqualsCore(utf8, startIndex, count),
                _ => PartiallyEqualsCore_Other(other, startIndex, count),
            };

        public override int GetHashCode() => HashingHelper.GetHashCode(this);

        public override unsafe int CompareToCore(string other)
        {
            int length = _length;
            int result = other.Length.CompareTo(length);
            if (result != 0 || length <= 0)
                return result;
            fixed (char* ptr = other)
                return CompareToCore(ptr, unchecked((nuint)length));
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
                AsciiLikeString ascii => CompareToCore(ascii, unchecked((nuint)length)),
                Utf16String utf16 => CompareToCore(utf16, unchecked((nuint)length)),
                Utf8String utf8 => CompareToCore(utf8, unchecked((nuint)length)),
                _ => CompareToCore(other, unchecked((nuint)length)),
            };
        }

        public override unsafe bool EqualsCore(string other)
        {
            int length = _length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            fixed (char* ptr = other)
                return EqualsCore(ptr, unchecked((nuint)length));
        }

        public override bool EqualsCore(StringBase other)
        {
            int length = _length;
            if (length != other.Length)
                return false;
            if (length <= 0)
                return true;
            return other switch
            {
                AsciiLikeString ascii => EqualsCore(ascii, unchecked((nuint)length)),
                Utf16String utf16 => EqualsCore(utf16, unchecked((nuint)length)),
                Utf8String utf8 => EqualsCore(utf8, unchecked((nuint)length)),
                _ => EqualsCore(other, unchecked((nuint)length)),
            };
        }

        private unsafe bool PartiallyEqualsCore(AsciiLikeString compare, nuint startIndex, nuint count)
        {
            fixed (byte* source = _value, sourceB = compare._value)
                return SequenceHelper.Equals(source + startIndex, sourceB, count);
        }

        private unsafe bool PartiallyEqualsCore(Utf8String compare, nuint startIndex, nuint count)
        {
            byte characterLimit = GetCharacterLimit();
            if (characterLimit > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return PartiallyEqualsCoreSlow(compare, startIndex, count, characterLimit);

            fixed (byte* source = _value, sourceB = compare.GetInternalRepresentation())
                return SequenceHelper.Equals(source + startIndex, sourceB, count);
        }

        private unsafe bool PartiallyEqualsCore(Utf16String compare, nuint startIndex, nuint count)
            => PartiallyEqualsCore(compare.GetInternalRepresentation(), startIndex, count);

        private unsafe bool PartiallyEqualsCore_Other(StringBase compare, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    compare.CopyToCore(temp, 0, count);
                    return PartiallyEqualsCore(temp, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool PartiallyEqualsCore(char* compare, nuint startIndex, nuint count)
        {
            if (SequenceHelper.ContainsGreaterThan(compare, count, unchecked((char)GetCharacterLimit())))
                return false;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(count);
            try
            {
                fixed (byte* temp = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(compare, temp, count);
                    fixed (byte* source = _value)
                        return SequenceHelper.Equals(source + startIndex, temp, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool PartiallyEqualsCoreSlow(Utf8String compare, nuint startIndex, nuint count, byte characterLimit)
        {
            byte[] compareArray = compare.GetInternalRepresentation();
            fixed (byte* source = _value, sourceB = compareArray)
            {
                byte* iteratorA = source + startIndex, iteratorB = sourceB, endB = sourceB + compareArray.Length - 1;
                for (nuint i = 0; i < count; i++)
                {
                    if ((iteratorB = Utf8EncodingHelper.TryReadUtf8Character(iteratorB, endB, out uint unicodeValue)) == null ||
                        unicodeValue > characterLimit ||
                        iteratorA[i] != unicodeValue)
                        return false;
                }
            }
            return true;
        }

        private unsafe int CompareToCore(AsciiLikeString compare, nuint length)
        {
            fixed (byte* source = _value, sourceB = compare._value)
                return InternalSequenceHelper.CompareTo(source, sourceB, length);
        }

        private unsafe int CompareToCore(Utf8String compare, nuint length)
        {
            if (GetCharacterLimit() > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return CompareToCoreSlow(compare, length);

            fixed (byte* source = _value, sourceB = compare.GetInternalRepresentation())
                return InternalSequenceHelper.CompareTo(source, sourceB, length);
        }

        private unsafe int CompareToCore(Utf16String compare, nuint length)
        {
            fixed (char* ptr = compare.GetInternalRepresentation())
                return CompareToCore(ptr, length);
        }

        private unsafe int CompareToCore(StringBase compare, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            try
            {
                fixed (char* temp = buffer)
                {
                    compare.CopyToCore(temp, 0, length);
                    return CompareToCore(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int CompareToCore(char* compare, nuint length)
        {
            fixed (byte* source = _value)
                return AsciiLikeStringHelper.CompareTo_Utf16(source, compare, length);
        }

        private unsafe int CompareToCoreSlow(Utf8String compare, nuint length)
        {
            fixed (byte* source = _value, sourceB = compare.GetInternalRepresentation())
                return -Utf8StringHelper.CompareTo_Latin1(sourceB, source, length);
        }

        private unsafe bool EqualsCore(AsciiLikeString compare, nuint length)
        {
            fixed (byte* source = _value, sourceB = compare._value)
                return SequenceHelper.Equals(source, sourceB, length);
        }

        private unsafe bool EqualsCore(Utf8String compare, nuint length)
        {
            if (GetCharacterLimit() > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return EqualsCoreSlow(compare, length);

            fixed (byte* source = _value, sourceB = compare.GetInternalRepresentation())
                return SequenceHelper.Equals(source, sourceB, length);
        }

        private unsafe bool EqualsCore(Utf16String compare, nuint length)
        {
            fixed (char* ptr = compare.GetInternalRepresentation())
                return EqualsCore(ptr, length);
        }

        private unsafe bool EqualsCore(char* compare, nuint length)
        {
            if (SequenceHelper.ContainsGreaterThan(compare, length, unchecked((char)GetCharacterLimit())))
                return false;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(length);
            try
            {
                fixed (byte* temp = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(compare, temp, length);
                    fixed (byte* source = _value)
                        return SequenceHelper.Equals(source, temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool EqualsCore(StringBase compare, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    compare.CopyToCore(temp, 0, count);
                    return EqualsCore(temp, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool EqualsCoreSlow(Utf8String compare, nuint length)
        {
            fixed (byte* source = _value, sourceB = compare.GetInternalRepresentation())
                return Utf8StringHelper.Equals_Latin1(source, sourceB, length);
        }
    }
}
