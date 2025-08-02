using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected internal unsafe override StringBase SubstringCore(nuint startIndex, nuint count)
        {
            if (count > Utf16CompressionLengthLimit)
                goto SlowRoute;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = count * sizeof(char);
            byte[] source = _value;
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* ptrSource = source, ptrBuffer = buffer)
                {
                    byte* sourceIterator = ptrSource, sourceEnd = ptrSource + source.Length, bufferEnd = ptrBuffer + bufferLength;
                    nuint offset = SkipCharacters(ref sourceIterator, sourceEnd, ptrBuffer, bufferEnd, startIndex);
                    if (offset == (nuint)UnsafeHelper.PointerMaxValue ||
                        TryCopyCharactersToBuffer(sourceIterator, sourceEnd, ptrBuffer + offset, ref bufferEnd, count) == null)
                        goto SlowRoute;

                    nuint length = unchecked((nuint)(bufferEnd - ptrBuffer));
                    if (length == 0)
                        return Empty;
                    if (length > MaxUtf8StringBufferSize)
                        goto SlowRoute;

                    bool isAsciiOnly = !SequenceHelper.ContainsGreaterThan(ptrBuffer, length, AsciiCharacterLimit);
                    if (isAsciiOnly)
                        return Latin1String.Create(ptrBuffer, length);

                    byte[] result = new byte[length + 1];
                    fixed (byte* ptrResult = result)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult, ptrBuffer, length * sizeof(byte));
                    return new Utf8String(result, unchecked((int)count));
                }
            }
            finally
            {
                pool.Return(buffer);
            }

        SlowRoute:
            return SubstringCoreSlow(startIndex, count);
        }

        protected unsafe override StringBase RemoveCore(nuint startIndex, nuint count)
        {
            nuint length = unchecked((nuint)_length);
            nuint endIndex = startIndex + count;
            if (endIndex >= length)
                return SubstringCore(0, startIndex);

            nuint resultLength = length - count;
            if (resultLength > Utf16CompressionLengthLimit)
                goto SlowRoute;

            byte[] source = _value;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = resultLength * sizeof(char);
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* ptrSource = source, ptrBuffer = buffer)
                {
                    byte* sourceEnd = ptrSource + source.Length, bufferEnd = ptrBuffer + bufferLength;
                    byte* bufferIterator = TryCopyCharactersToBuffer(ptrSource, sourceEnd, ptrBuffer, ref bufferEnd, startIndex);
                    if (bufferIterator == null)
                        goto SlowRoute;
                    byte* sourceIterator = ptrSource;
                    nuint offset = SkipCharacters(ref sourceIterator, sourceEnd, bufferIterator, bufferEnd, resultLength - endIndex);
                    if (offset == (nuint)UnsafeHelper.PointerMaxValue)
                        goto SlowRoute;

                    length = unchecked((nuint)(bufferIterator - ptrBuffer) + offset);
                    if (length == 0)
                        return Empty;
                    if (length > MaxUtf8StringBufferSize)
                        goto SlowRoute;

                    bool isAsciiOnly = !SequenceHelper.ContainsGreaterThan(ptrBuffer, length, AsciiCharacterLimit);
                    if (isAsciiOnly)
                        return Latin1String.Create(ptrBuffer, length);

                    byte[] result = new byte[length + 1];
                    fixed (byte* ptrResult = result)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult, ptrBuffer, length * sizeof(byte));
                    return new Utf8String(result, unchecked((int)resultLength));
                }
            }
            finally
            {
                pool.Return(buffer);
            }

        SlowRoute:
            return RemoveCoreSlow(startIndex, endIndex, resultLength);
        }

        private unsafe Utf16String SubstringCoreSlow(nuint startIndex, nuint count)
        {
            Utf16String result = Utf16String.Allocate(count, out string buffer);
            fixed (char* ptr = buffer)
                CopyToCore(ptr, startIndex, count);
            return result;
        }

        private unsafe Utf16String RemoveCoreSlow(nuint startIndex, nuint endIndex, nuint resultLength)
        {
            Utf16String result = Utf16String.Allocate(resultLength, out string buffer);
            fixed (char* ptr = buffer)
            {
                CopyToCore(ptr, 0, startIndex);
                CopyToCore(ptr + startIndex, endIndex, resultLength - startIndex);
            }
            return result;
        }

        private static unsafe byte* TryCopyCharactersToBuffer(byte* source, byte* sourceEnd, byte* buffer, ref byte* bufferEnd, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if ((source = Utf8EncodingHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                {
                    bufferEnd = buffer;
                    break;
                }
                if ((buffer = Utf8EncodingHelper.TryWriteUtf8Character(buffer, bufferEnd, unicodeValue)) == null)
                    return null;
            }
            return buffer;
        }

        private static unsafe nuint SkipCharacters(ref byte* source, byte* sourceEnd, byte* destination, byte* destinationEnd, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if ((source = Utf8EncodingHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                    return 0;
                if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out _, out char trailSurrogate))
                {
                    if (++i < count)
                        continue;
                    if ((destination = Utf8EncodingHelper.TryWriteUtf8Character(destination, destinationEnd, trailSurrogate)) == null)
                        return (nuint)UnsafeHelper.PointerMaxValue;
                }
            }
            return unchecked((nuint)(destinationEnd - destination));
        }

        private static unsafe nuint SkipCharacters(ref byte* source, byte* sourceEnd, char* destination, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if ((source = Utf8EncodingHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                    return 0;
                if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out _, out char trailSurrogate))
                {
                    if (++i < count)
                        continue;
                    *destination = trailSurrogate;
                    return 1;
                }
            }
            return 0;
        }
    }
}
