using System.IO;
using System.Text;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInOtherEncoding(bool movePosition)
        {
            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                nuint charBufferLength = unchecked((nuint)charBuffer.Length);
                nuint charBufferPos = _charBufferPos;
                if (charBufferPos < charBufferLength)
                {
                    int result = charBuffer[charBufferPos];
                    if (movePosition)
                    {
                        if (++charBufferPos < charBufferLength)
                            _charBufferPos = charBufferPos;
                        else
                            _charBuffer = null;
                    }
                    return result;
                }
                _charBuffer = null;
            }
            nuint currentPos;
            while ((currentPos = _bufferPos) >= _bufferLength)
            {
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return -1;
                }
            }

            Decoder decoder = _encoding.GetDecoder();
            byte[] buffer = _buffer;

            nuint bufferPos;
            int receivedCharCount;
            while ((receivedCharCount = decoder.GetCharCount(buffer, (int)(bufferPos = _bufferPos), (int)(_bufferLength - bufferPos), flush: true)) <= 0)
            {
                ReadStream();

                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return -1;
                }
            }

            charBuffer = new char[receivedCharCount];
            fixed (byte* source = buffer)
            fixed (char* destination = charBuffer)
            {
                decoder.Convert(source + bufferPos, (int)(_bufferLength - bufferPos), destination, receivedCharCount, true, out int sourceOffset, out _, out bool completed);
                if (completed)
                    _bufferPos = _bufferLength;
                else
                    _bufferPos = bufferPos + MathHelper.MakeUnsigned(sourceOffset);
            }
            _charBuffer = charBuffer;
            _charBufferPos = movePosition ? 1u : 0u;
            return charBuffer[0];
        }

        private partial string? ReadLineInOtherEncoding()
        {
            bool isEndOfStream = _eofReached;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                string? result = ReadLineInCharBuffer(charBuffer, isEndOfStream);
                if (isEndOfStream || result is null)
                    _charBuffer = null;
                else
                    return result;
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
            {
                fixed (char* ptr = charBuffer)
                    builder.Append(ptr + _charBufferPos, ptr + charBuffer.Length);
            }

            Decoder decoder = _encoding.GetDecoder();
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            do
            {
                currentPos = _bufferPos;
                currentLength = _bufferLength;

                int currentCount = unchecked((int)(currentLength - currentPos));
                int charBufferLength;
                fixed (byte* ptr = buffer)
                {
                    charBufferLength = decoder.GetCharCount(ptr + currentPos, currentCount, flush: true);
                    if (charBufferLength == 0)
                        goto Read;
                    charBuffer = pool.Rent(charBufferLength);
                }
                try
                {
                    fixed (char* destination = charBuffer)
                    {
                        char* destinationEnd;
                        fixed (byte* source = buffer)
                        {
                            decoder.Convert(source + currentPos, currentCount, destination, charBufferLength, true, out int sourceOffset, out int destinationOffset, out bool completed);
                            destinationEnd = destination + destinationOffset;
                            if (completed)
                                _bufferPos = _bufferLength;
                            else
                                _bufferPos = currentPos + MathHelper.MakeUnsigned(sourceOffset);
                        }
                        char* destinationIndexOf = SequenceHelper.PointerIndexOf(destination, destinationEnd, '\n');
                        if (destinationIndexOf == null)
                            builder.Append(destination, destinationEnd);
                        else
                        {
                            nuint count = unchecked((nuint)(destinationEnd - destinationIndexOf));
                            if (destinationIndexOf > destination)
                            {
                                char* destinationIndexOfBackward = destinationIndexOf - 1;
                                if (*destinationIndexOfBackward == '\r')
                                    destinationIndexOf = destinationIndexOfBackward;
                            }
                            builder.Append(destination, destinationIndexOf);
                            if (count > 1)
                            {
                                char[] newBuffer = new char[count];
                                fixed (char* ptrNew = newBuffer)
                                    UnsafeHelper.CopyBlockUnaligned(ptrNew, destinationEnd - count, count * sizeof(char));
                                _charBuffer = newBuffer;
                                _charBufferPos = 0;
                            }
                            break;
                        }
                    }
                }
                finally
                {
                    pool.Return(charBuffer);
                }

            Read:
                ReadStream();

                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return builder.Length <= 0 ? null : builder.ToString();
                }
            } while (true);
            return builder.ToString();
        }

        private partial StringBase? ReadLineAsStringBaseInOtherEncoding()
        {
            string? result = ReadLineInOtherEncoding();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }
    }
}
