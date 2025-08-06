using System.IO;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInLatin1Encoding(bool movePosition)
        {
            byte[] buffer = _buffer;
            nuint currentPos, nextPos;
            while ((nextPos = (currentPos = _bufferPos) + 1) >= _bufferLength)
            {
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return -1;
                }
            }
            if (movePosition)
                _bufferPos = nextPos;
            return buffer[currentPos];
        }

        [LocalsInit(false)]
        private partial string? ReadLineInLatin1Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return null;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;
            nint indexOf;

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] charBuffer = pool.Rent(buffer.Length);
            try
            {
                while ((indexOf = FindNewLineMarkInAscii(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
                {
                    if (currentPos < currentLength)
                    {
                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + currentLength, destination, destination + currentLength);
                            builder.Append(destination, destinationEnd);
                        }
                        _bufferPos = currentLength;
                    }
                    ReadStream();
                    if (_eofReached)
                    {
                        _bufferPos = _bufferLength;
                        return builder.Length <= 0 ? null : builder.ToString();
                    }
                }

                _bufferPos = currentPos + unchecked((nuint)indexOf) + 1;

                fixed (byte* source = buffer)
                {
                    byte* startPointer = source + currentPos;
                    if (indexOf > 0 && startPointer[indexOf - 1] == (byte)'\r')
                        indexOf--;
                    fixed (char* destination = charBuffer)
                    {
                        char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + indexOf, destination, destination + currentLength);
                        builder.Append(destination, destinationEnd);
                    }
                }
            }
            finally
            {
                pool.Return(charBuffer);
            }
            return builder.ToString();
        }

        [LocalsInit(false)]
        private partial string ReadToEndInLatin1Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return string.Empty;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] charBuffer = pool.Rent(buffer.Length);
            try
            {
                do
                {
                    currentPos = _bufferPos;
                    currentLength = _bufferLength;
                    if (currentPos < currentLength)
                    {
                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + currentLength, destination, destination + currentLength);
                            builder.Append(destination, destinationEnd);
                        }
                        _bufferPos = currentLength;
                    }
                    ReadStream();
                    if (_eofReached)
                    {
                        _bufferPos = _bufferLength;
                        break;
                    }
                } while (true);
            }
            finally
            {
                pool.Return(charBuffer);
            }
            return builder.ToString();
        }

        private partial StringBase? ReadLineAsStringBaseInLatin1Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return null;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            bool isEndOfStream = TryReadLineIntoBuffer_AsciiLike(list);
            int count = list.Count;
            if (count <= 0)
                return isEndOfStream ? null : StringBase.Empty;
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private partial StringBase ReadToEndAsStringBaseInLatin1Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return StringBase.Empty;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            ReadToEndIntoBuffer_AsciiLike(list);
            int count = list.Count;
            if (count <= 0)
                return StringBase.Empty;
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
