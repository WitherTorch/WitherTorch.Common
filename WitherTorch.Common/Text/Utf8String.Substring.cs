using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected internal unsafe override StringBase SubstringCore(nuint startIndex, nuint count)
        {
            if (_isAsciiOnly)
                return SubstringCoreFast(startIndex, count);

            return SubstringCoreSlow(startIndex, count);
        }

        protected unsafe override StringBase RemoveCore(nuint startIndex, nuint count)
        {
            if (_isAsciiOnly)
                return RemoveCoreFast(startIndex, count);

            return RemoveCoreSlow(startIndex, count);
        }

        private unsafe StringBase SubstringCoreFast(nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
            {
                if (WTCommon.AllowLatin1StringCompression)
                    return Latin1String.Create(ptr + startIndex, count);

                byte[] buffer = new byte[count + 1];
                fixed (byte* ptrBuffer = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptr + startIndex, unchecked((uint)count * sizeof(byte)));
                return new Utf8String(buffer, unchecked((int)count), isAsciiOnly: true);
            }
        }

        private unsafe StringBase SubstringCoreSlow(nuint startIndex, nuint count)
        {
            if (count > Utf16CompressionLengthLimit)
                goto VerySlowRoute;

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
                        goto VerySlowRoute;

                    nuint length = unchecked((nuint)(bufferEnd - ptrBuffer));
                    if (length == 0)
                        return Empty;
                    if (length > MaxUtf8StringBufferSize)
                        goto VerySlowRoute;

                    bool isAsciiOnly = !SequenceHelper.ContainsGreaterThan(ptrBuffer, length, AsciiCharacterLimit);
                    if (isAsciiOnly && WTCommon.AllowLatin1StringCompression)
                        return Latin1String.Create(ptrBuffer, length);

                    byte[] result = new byte[length + 1];
                    fixed (byte* ptrResult = result)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult, ptrBuffer, unchecked((uint)length));
                    return new Utf8String(result, unchecked((int)count), isAsciiOnly);
                }
            }
            finally
            {
                pool.Return(buffer);
            }

        VerySlowRoute:
            return SubstringCoreVerySlow(startIndex, count);
        }

        private unsafe Utf16String SubstringCoreVerySlow(nuint startIndex, nuint count)
        {
            Utf16String result = Utf16String.Allocate(count, out string buffer);
            fixed (char* ptr = buffer)
                CopyToCore(ptr, startIndex, count);
            return result;
        }

        private unsafe StringBase RemoveCoreFast(nuint startIndex, nuint count)
        {
            nuint length = unchecked((nuint)_length);
            nuint endIndex = startIndex + count;
            if (endIndex >= length)
                return SubstringCoreFast(0, startIndex);

            nuint resultLength = length - count;
            byte[] buffer = new byte[resultLength + 1];
            fixed (byte* ptrSource = _value, ptrBuffer = buffer)
            {
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource, unchecked((uint)startIndex * sizeof(byte)));
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptrSource + endIndex, unchecked((uint)(length - endIndex) * sizeof(byte)));
            }
            return new Utf8String(buffer, unchecked((int)resultLength), isAsciiOnly: true);
        }

        private unsafe StringBase RemoveCoreSlow(nuint startIndex, nuint count)
        {
            nuint length = unchecked((nuint)_length);
            nuint endIndex = startIndex + count;
            if (endIndex >= length)
                return SubstringCore(0, startIndex);

            nuint resultLength = length - count;
            if (resultLength > Utf16CompressionLengthLimit)
                goto VerySlowRoute;

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
                        goto VerySlowRoute;
                    byte* sourceIterator = ptrSource;
                    nuint offset = SkipCharacters(ref sourceIterator, sourceEnd, bufferIterator, bufferEnd, resultLength - endIndex);
                    if (offset == (nuint)UnsafeHelper.PointerMaxValue)
                        goto VerySlowRoute;

                    length = unchecked((nuint)(bufferIterator - ptrBuffer) + offset);
                    if (length == 0)
                        return Empty;
                    if (length > MaxUtf8StringBufferSize)
                        goto VerySlowRoute;

                    bool isAsciiOnly = !SequenceHelper.ContainsGreaterThan(ptrBuffer, length, AsciiCharacterLimit);
                    if (isAsciiOnly && WTCommon.AllowLatin1StringCompression)
                        return Latin1String.Create(ptrBuffer, length);

                    byte[] result = new byte[length + 1];
                    fixed (byte* ptrResult = result)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult, ptrBuffer, unchecked((uint)length));
                    return new Utf8String(result, unchecked((int)resultLength), isAsciiOnly);
                }
            }
            finally
            {
                pool.Return(buffer);
            }

        VerySlowRoute:
            return RemoveCoreVerySlow(startIndex, endIndex, resultLength);
        }

        private unsafe Utf16String RemoveCoreVerySlow(nuint startIndex, nuint endIndex, nuint resultLength)
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
                if ((source = Utf8StringHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                {
                    bufferEnd = buffer;
                    break;
                }
                if ((buffer = Utf8StringHelper.TryWriteUtf8Character(buffer, bufferEnd, unicodeValue)) == null)
                    return null;
            }
            return buffer;
        }

        private static unsafe nuint SkipCharacters(ref byte* source, byte* sourceEnd, byte* destination, byte* destinationEnd, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if ((source = Utf8StringHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                    return 0;
                if (Utf8StringHelper.ToUtf16Characters(unicodeValue, out _, out char trailSurrogate))
                {
                    if (++i < count)
                        continue;
                    if ((destination = Utf8StringHelper.TryWriteUtf8Character(destination, destinationEnd, trailSurrogate)) == null)
                        return (nuint)UnsafeHelper.PointerMaxValue;
                }
            }
            return unchecked((nuint)(destinationEnd - destination));
        }

        private static unsafe nuint SkipCharacters(ref byte* source, byte* sourceEnd, char* destination, nuint count)
        {
            for (nuint i = 0; i < count; i++)
            {
                if ((source = Utf8StringHelper.TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) == null)
                    return 0;
                if (Utf8StringHelper.ToUtf16Characters(unicodeValue, out _, out char trailSurrogate))
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
