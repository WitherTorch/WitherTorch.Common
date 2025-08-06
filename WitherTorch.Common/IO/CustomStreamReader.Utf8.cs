using System.IO;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInUtf8Encoding(bool movePosition)
        {
            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                nuint charBufferPos = _charBufferPos;
                if (charBufferPos == 0 && charBuffer.Length > 0)
                {
                    if (movePosition)
                        _charBuffer = null;
                    return charBuffer[0];
                }
                _charBuffer = null;
            }
            byte[] buffer = _buffer;
            nuint currentPos;
            while ((currentPos = _bufferPos) >= _bufferLength)
            {
                if (_eofReached)
                    return -1;
                ReadStream();
            }
            nuint nextPos;
            while ((nextPos = currentPos + Utf8EncodingHelper.GetNextUtf8CharacterOffset(buffer[currentPos])) >= _bufferLength)
            {
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return -1;
                }
                currentPos = _bufferPos;
            }
            if (movePosition)
                _bufferPos = nextPos;
            fixed (byte* ptr = buffer)
            {
                byte* ptrOffset = Utf8EncodingHelper.TryReadUtf8Character(ptr + currentPos, ptr + nextPos, out uint unicodeValue);
                if (ptrOffset == null)
                    return -1;
                if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out char leadSurrogate, out char trailSurrogate) && movePosition)
                {
                    _charBuffer = [trailSurrogate];
                    _charBufferPos = 0;
                }
                return leadSurrogate;
            }
        }

        [LocalsInit(false)]
        private partial string? ReadLineInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                nuint charBufferPos = _charBufferPos;
                if (charBufferPos == 0 && charBuffer.Length > 0)
                {
                    char c = charBuffer[0];
                    if (c == '\n' || isEndOfStream)
                    {
                        _charBuffer = null;
                        return new string(c, 1);
                    }
                }
                else
                {
                    _charBuffer = null;
                    charBuffer = null;
                }
            }

            if (isEndOfStream)
                return null;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
            if (charBuffer is not null)
                builder.Append(charBuffer[0]);
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;
            nint indexOf;
            while ((indexOf = FindNewLineMarkInAscii(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
            {
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                    {
                        byte* iterator = ptr + currentPos;
                        byte* iteratorOld = iterator;
                        byte* ptrEnd = ptr + currentLength;
                        while ((iterator = Utf8EncodingHelper.TryReadUtf8Character(iterator, ptrEnd, out uint unicodeValue)) != null)
                        {
                            if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out char leadSurrogate, out char trailSurrogate))
                            {
                                builder.Append(leadSurrogate);
                                builder.Append(trailSurrogate);
                            }
                            else
                            {
                                builder.Append(leadSurrogate);
                            }
                            iteratorOld = iterator;
                        }
                        _bufferPos = unchecked((nuint)(iteratorOld - ptr));
                    }
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return builder.Length <= 0 ? null : builder.ToString();
                }
            }

            _bufferPos = currentPos + unchecked((nuint)indexOf) + 1;

            fixed (byte* ptr = buffer)
            {
                byte* startPointer = ptr + currentPos;
                if (indexOf > 0 && startPointer[indexOf - 1] == (byte)'\r')
                    indexOf--;
                byte* iterator = ptr + currentPos;
                byte* ptrEnd = iterator + indexOf;
                while ((iterator = Utf8EncodingHelper.TryReadUtf8Character(iterator, ptrEnd, out uint unicodeValue)) != null)
                {
                    if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out char leadSurrogate, out char trailSurrogate))
                    {
                        builder.Append(leadSurrogate);
                        builder.Append(trailSurrogate);
                    }
                    else
                    {
                        builder.Append(leadSurrogate);
                    }
                }
            }
            return builder.ToString();
        }

        [LocalsInit(false)]
        private partial string ReadToEndInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                _charBuffer = null;
                if (_charBufferPos == 0 && charBuffer.Length > 0)
                {
                    if (isEndOfStream)
                        return new string(charBuffer[0], 1);
                }
                else
                {
                    charBuffer = null;
                }
            }

            if (isEndOfStream)
                return string.Empty;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
            if (charBuffer is not null)
                builder.Append(charBuffer[0]);
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;
            do
            {
                currentPos = _bufferPos;
                currentLength = _bufferLength;
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                    {
                        byte* iterator = ptr + currentPos;
                        byte* iteratorOld = iterator;
                        byte* ptrEnd = ptr + currentLength;
                        while ((iterator = Utf8EncodingHelper.TryReadUtf8Character(iterator, ptrEnd, out uint unicodeValue)) != null)
                        {
                            if (Utf8EncodingHelper.ToUtf16Characters(unicodeValue, out char leadSurrogate, out char trailSurrogate))
                            {
                                builder.Append(leadSurrogate);
                                builder.Append(trailSurrogate);
                            }
                            else
                            {
                                builder.Append(leadSurrogate);
                            }
                            iteratorOld = iterator;
                        }
                        _bufferPos = unchecked((nuint)(iteratorOld - ptr));
                    }
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    break;
                }
            } while (true);
            return builder.ToString();
        }

        [LocalsInit(false)]
        private partial StringBase? ReadLineAsStringBaseInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                nuint charBufferPos = _charBufferPos;
                if (charBufferPos == 0 && charBuffer.Length > 0)
                {
                    char c = charBuffer[0];
                    if (c == '\n' || isEndOfStream)
                    {
                        _charBuffer = null;
                        return StringBase.Create(&c, 0u, 1u);
                    }
                }
                else
                {
                    _charBuffer = null;
                    charBuffer = null;
                }
            }

            if (isEndOfStream)
                return null;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            if (charBuffer is not null)
            {
                byte* tempBuffer = stackalloc byte[4];
                byte* tempBufferEnd = tempBuffer + 4;
                byte* tempBufferLimit = Utf8EncodingHelper.TryWriteUtf8Character(tempBuffer, tempBufferEnd, charBuffer[0]);
                if (tempBufferLimit > tempBuffer)
                    list.AddRange(tempBuffer, 0, unchecked((nuint)(tempBufferLimit - tempBuffer)));
            }

            isEndOfStream = TryReadLineIntoBuffer_AsciiLike(list);
            int count = list.Count;
            if (count <= 0)
                return isEndOfStream ? null : StringBase.Empty;
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateUtf8String(ptr, 0u, unchecked((nuint)count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        [LocalsInit(false)]
        private partial StringBase ReadToEndAsStringBaseInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                nuint charBufferPos = _charBufferPos;
                if (charBufferPos == 0 && charBuffer.Length > 0)
                {
                    if (isEndOfStream)
                    {
                        char c = charBuffer[0];
                        _charBuffer = null;
                        return StringBase.Create(&c, 0u, 1u);
                    }
                }
                else
                {
                    _charBuffer = null;
                    charBuffer = null;
                }
            }

            if (isEndOfStream)
                return StringBase.Empty;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            if (charBuffer is not null)
            {
                byte* tempBuffer = stackalloc byte[4];
                byte* tempBufferEnd = tempBuffer + 4;
                byte* tempBufferLimit = Utf8EncodingHelper.TryWriteUtf8Character(tempBuffer, tempBufferEnd, charBuffer[0]);
                if (tempBufferLimit > tempBuffer)
                    list.AddRange(tempBuffer, 0, unchecked((nuint)(tempBufferLimit - tempBuffer)));
            }

            ReadToEndIntoBuffer_AsciiLike(list);
            int count = list.Count;
            if (count <= 0)
                return StringBase.Empty;
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateUtf8String(ptr, 0u, unchecked((nuint)count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
