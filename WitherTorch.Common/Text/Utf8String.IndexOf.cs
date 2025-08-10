using System.Linq;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override unsafe bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
            {
                return this.SkipAndTake(startIndex, count)
                    .WhereEqualsTo(value)
                    .Any();
            }

            if (value > AsciiEncodingHelper.AsciiEncodingLimit)
                return ContainsCoreFallback(value);

            return ContainsCore(unchecked((byte)value));
        }

        protected override unsafe bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
                return base.ContainsCore(value, valueLength, startIndex, count);

            fixed (char* ptr = value)
                return ContainsCore(ptr, valueLength);
        }

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                AsciiString ascii => ContainsCore(ascii, valueLength, startIndex, count),
                Latin1String latin1 => ContainsCore(latin1, valueLength, startIndex, count),
                Utf16String utf16 => ContainsCore(utf16, valueLength, startIndex, count),
                _ => ContainsCore_Other(value, valueLength, startIndex, count)
            };

        protected override unsafe int IndexOfCore(char value, nuint startIndex, nuint count)
            => this.WithIndex()
            .SkipAndTake(startIndex, count)
            .WhereEqualsTo(value)
            .Select(static item => item.Index)
            .DefaultIfEmpty(-1)
            .First();

        private unsafe bool ContainsCore(byte value)
        {
            byte[] source = _value;
            fixed (byte* ptr = source)
                return SequenceHelper.Contains(ptr, MathHelper.MakeUnsigned(source.Length - 1), value);
        }

        private unsafe bool ContainsCore(AsciiString value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
                return base.ContainsCore(value, valueLength, startIndex, count);

            fixed (byte* ptr = value.GetInternalRepresentation())
                return ContainsCore(ptr, valueLength);
        }

        private unsafe bool ContainsCore(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
                return base.ContainsCore(value, valueLength, startIndex, count);

            fixed (byte* ptr = value.GetInternalRepresentation())
            {
                if (SequenceHelper.ContainsGreaterThan(ptr, valueLength, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
                    return ContainsCoreFallback(ptr, valueLength);
                return ContainsCore(ptr, valueLength);
            }
        }

        private unsafe bool ContainsCore(Utf16String value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
                return base.ContainsCore(value, valueLength, startIndex, count);

            fixed (char* ptr = value.GetInternalRepresentation())
                return ContainsCore(ptr, valueLength);
        }

        private unsafe bool ContainsCore_Other(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length))
                return base.ContainsCore(value, valueLength, startIndex, count);

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0, valueLength);
                    return ContainsCore(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCore(byte* value, nuint valueLength)
        {
            byte[] source = _value;
            fixed (byte* ptr = source)
                return InternalSequenceHelper.Contains(ptr, MathHelper.MakeUnsigned(source.Length - 1), value, valueLength);
        }

        private unsafe bool ContainsCore(char* value, nuint valueLength)
        {
            if (SequenceHelper.ContainsGreaterThan(value, valueLength, AsciiEncodingHelper.AsciiEncodingLimit))
                return ContainsCoreFallback(value, valueLength);

            return ContainsCoreFast(value, valueLength);
        }

        private unsafe bool ContainsCoreFast(char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(value, temp, valueLength);
                    return ContainsCore(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCoreFallback(char value)
        {
            if (Utf16StringHelper.IsSurrogateCharacter(value))
                return ContainsCoreFallbackSlow(value);
            return ContainsCoreFallbackFast(value);
        }

        private unsafe bool ContainsCoreFallback(byte* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = valueLength * 2;
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    byte* iterator = temp, tempEnd = temp + bufferLength;
                    for (nuint i = 0; i < valueLength; i++)
                        iterator = Utf8EncodingHelper.TryWriteUtf8Character(iterator, tempEnd, value[i]);
                    return ContainsCore(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCoreFallback(char* value, nuint valueLength)
        {
            if (Utf16StringHelper.HasSurrogateCharacters(value, valueLength))
                return ContainsCoreFallbackSlow(value, valueLength);
            return ContainsCoreFallbackFast(value, valueLength);
        }

        [LocalsInit(false)]
        private unsafe bool ContainsCoreFallbackFast(char value)
        {
            byte* buffer = stackalloc byte[3];
            byte* bufferEnd = Utf8EncodingHelper.TryWriteUtf8Character(buffer, buffer + 3, value);
            return ContainsCore(buffer, unchecked((nuint)(bufferEnd - buffer)));
        }

        private unsafe bool ContainsCoreFallbackSlow(char value)
            => this.WhereEqualsTo(value).Any();

        private unsafe bool ContainsCoreFallbackFast(char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = Utf8EncodingHelper.GetWorstCaseForEncodeLength(valueLength);
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    byte* iterator = temp, tempEnd = temp + bufferLength;
                    for (nuint i = 0; i < valueLength; i++)
                        iterator = Utf8EncodingHelper.TryWriteUtf8Character(iterator, tempEnd, value[i]);
                    return ContainsCore(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCoreFallbackSlow(char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = Utf8EncodingHelper.GetWorstCaseForEncodeLength(valueLength);
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    byte* iterator = temp, tempEnd = temp + bufferLength;
                    for (nuint i = 0; i < valueLength; i++)
                        iterator = Utf8EncodingHelper.TryWriteUtf8Character(iterator, tempEnd, value[i]);
                    return ContainsCore(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
