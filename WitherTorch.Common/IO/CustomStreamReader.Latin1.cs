using System.IO;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInLatin1Encoding(bool movePosition)
        {
            nuint currentPos;
            while ((currentPos = _bufferPos) >= _bufferLength)
            {
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return -1;
                }
            }
            byte[] buffer = _buffer;
            nuint nextPos;
            while ((nextPos = currentPos + 1) >= _bufferLength)
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
            return buffer[currentPos];
        }

        private partial string? ReadLineInLatin1Encoding()
        {
            if (_eofReached)
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

                fixed (byte* ptr = buffer)
                {
                    byte* startPointer = ptr + currentPos;
                    if (indexOf > 0 && startPointer[indexOf - 1] == (byte)'\r')
                        indexOf--;
                    fixed (byte* source = buffer)
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

        private partial StringBase? ReadLineAsStringBaseInLatin1Encoding()
        {
            if (_eofReached)
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
    }
}
