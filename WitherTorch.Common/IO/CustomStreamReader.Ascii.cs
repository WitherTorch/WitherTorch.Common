using System.IO;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInAsciiEncoding(bool movePosition)
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
            int result = buffer[currentPos];
            if (result > Latin1EncodingHelper.AsciiEncodingLimit_InByte)
                result = '?';
            return result;
        }

        [LocalsInit(false)]
        private partial string? ReadLineInAsciiEncoding()
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
                            char* iterator = destination;
                            while ((iterator = SequenceHelper.PointerIndexOfGreaterThan(iterator, destinationEnd, Latin1EncodingHelper.AsciiEncodingLimit)) != null)
                            {
                                *iterator = '?';
                                iterator++;
                            }
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
                        char* iterator = destination;
                        while ((iterator = SequenceHelper.PointerIndexOfGreaterThan(iterator, destinationEnd, Latin1EncodingHelper.AsciiEncodingLimit)) != null)
                        {
                            *iterator = '?';
                            iterator++;
                        }
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
        private partial string ReadToEndInAsciiEncoding()
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
                            char* iterator = destination;
                            while ((iterator = SequenceHelper.PointerIndexOfGreaterThan(iterator, destinationEnd, Latin1EncodingHelper.AsciiEncodingLimit)) != null)
                            {
                                *iterator = '?';
                                iterator++;
                            }
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

        private partial StringBase? ReadLineAsStringBaseInAsciiEncoding()
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
                {
                    byte* iterator = ptr;
                    byte* ptrEnd = ptr + count;
                    while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, Latin1EncodingHelper.AsciiEncodingLimit_InByte)) != null)
                    {
                        *iterator = (byte)'?';
                        iterator++;
                    }
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)count));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private partial StringBase ReadToEndAsStringBaseInAsciiEncoding()
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
                {
                    byte* iterator = ptr;
                    byte* ptrEnd = ptr + count;
                    while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, Latin1EncodingHelper.AsciiEncodingLimit_InByte)) != null)
                    {
                        *iterator = (byte)'?';
                        iterator++;
                    }
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)count));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
