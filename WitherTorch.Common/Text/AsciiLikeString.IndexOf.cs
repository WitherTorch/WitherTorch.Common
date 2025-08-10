using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class AsciiLikeString
    {
        protected override bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            if (value > GetCharacterLimit())
                return false;

            fixed (byte* source = _value)
                return SequenceHelper.Contains(source + startIndex, count, unchecked((byte)value));
        }

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value)
                return ContainsCore(ptr, valueLength, startIndex, count);
        }

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                AsciiLikeString ascii => ContainsCore(ascii, valueLength, startIndex, count),
                Utf16String utf16 => ContainsCore(utf16, valueLength, startIndex, count),
                Utf8String utf8 => ContainsCore(utf8, valueLength, startIndex, count),
                _ => ContainsCore_Other(value, valueLength, startIndex, count)
            };

        protected override int IndexOfCore(char value, nuint startIndex, nuint count)
        {
            if (value > GetCharacterLimit())
                return -1;

            fixed (byte* source = _value)
                return InternalSequenceHelper.ConvertToIndex32(
                    SequenceHelper.PointerIndexOf(source + startIndex, count, unchecked((byte)value)), source);
        }

        protected override int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value)
                return IndexOfCore(ptr, valueLength, startIndex, count);
        }

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                AsciiLikeString ascii => IndexOfCore(ascii, valueLength, startIndex, count),
                Utf16String utf16 => IndexOfCore(utf16, valueLength, startIndex, count),
                Utf8String utf8 => IndexOfCore(utf8, valueLength, startIndex, count),
                _ => IndexOfCore_Other(value, valueLength, startIndex, count)
            };

        private bool ContainsCore(AsciiLikeString value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* source = _value, sourceB = value._value)
                return InternalSequenceHelper.Contains(source + startIndex, count, sourceB, valueLength);
        }

        private bool ContainsCore(Utf8String value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (GetCharacterLimit() > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return ContainsCore_Other(value, valueLength, startIndex, count);

            fixed (byte* source = _value, sourceB = value.GetInternalRepresentation())
                return InternalSequenceHelper.Contains(source + startIndex, count, sourceB, valueLength);
        }

        private bool ContainsCore(Utf16String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value.GetInternalRepresentation())
                return ContainsCore(ptr, valueLength, startIndex, count);
        }

        private bool ContainsCore(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (SequenceHelper.ContainsGreaterThan(value, valueLength, unchecked((char)GetCharacterLimit())))
                return false;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(value, temp, valueLength);
                    fixed (byte* source = _value)
                        return InternalSequenceHelper.Contains(source + startIndex, count, temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private bool ContainsCore_Other(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0, valueLength);
                    return ContainsCore(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private int IndexOfCore(AsciiLikeString value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* source = _value, sourceB = value._value)
                return InternalSequenceHelper.ConvertToIndex32(
                    InternalSequenceHelper.PointerIndexOf(source + startIndex, count, sourceB, valueLength), source);
        }

        private int IndexOfCore(Utf8String value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (GetCharacterLimit() > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return IndexOfCore_Other(value, valueLength, startIndex, count);

            fixed (byte* source = _value, sourceB = value.GetInternalRepresentation())
                return InternalSequenceHelper.ConvertToIndex32(
                    InternalSequenceHelper.PointerIndexOf(source + startIndex, count, sourceB, valueLength), source);
        }

        private int IndexOfCore(Utf16String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value.GetInternalRepresentation())
                return IndexOfCore(ptr, valueLength, startIndex, count);
        }

        private int IndexOfCore(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (SequenceHelper.ContainsGreaterThan(value, valueLength, unchecked((char)GetCharacterLimit())))
                return -1;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(value, temp, valueLength);
                    fixed (byte* source = _value)
                        return InternalSequenceHelper.ConvertToIndex32(
                            InternalSequenceHelper.PointerIndexOf(source + startIndex, count, temp, valueLength), source);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private int IndexOfCore_Other(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0, valueLength);
                    return IndexOfCore(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
