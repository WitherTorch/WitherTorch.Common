using System.IO;
using System.Text;

using LocalsInit;

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

            Decoder decoder = _encoding.GetDecoder();
            nuint currentPos, currentLength;
            byte[] buffer = _buffer;
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            charBuffer = pool.Rent(buffer.Length);
            try
            {
                do
                {
                    currentPos = _bufferPos;
                    currentLength = _bufferLength;
                    if (currentPos < currentLength)
                    {
                        int currentCount = unchecked((int)(currentLength - currentPos));

                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                                flush: false, out int consumedBytes, out int producedChars, out bool completed);
                            if (completed)
                                _bufferPos = currentLength;
                            else
                                _bufferPos = currentPos + MathHelper.MakeUnsigned(consumedBytes);
                            if (producedChars > 0)
                            {
                                char[] newCharBuffer = new char[producedChars];
                                fixed (char* ptr = newCharBuffer)
                                    UnsafeHelper.CopyBlockUnaligned(ptr, destination, unchecked((uint)producedChars * sizeof(char)));
                                _charBuffer = newCharBuffer;
                                _charBufferPos = movePosition ? 1u : 0u;
                                return newCharBuffer[0];
                            }
                        }
                    }

                    ReadStream();
                    if (_eofReached)
                    {
                        currentPos = _bufferPos;
                        currentLength = _bufferLength;
                        if (currentPos < currentLength)
                        {
                            _bufferPos = currentLength;

                            int currentCount = unchecked((int)(currentLength - currentPos));

                            fixed (byte* source = buffer)
                            fixed (char* destination = charBuffer)
                            {
                                decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                                    flush: true, out int _, out int producedChars, out bool _);
                                if (producedChars > 0)
                                {
                                    char[] newCharBuffer = new char[producedChars];
                                    fixed (char* ptr = newCharBuffer)
                                        UnsafeHelper.CopyBlockUnaligned(ptr, destination, unchecked((uint)producedChars * sizeof(char)));
                                    _charBuffer = newCharBuffer;
                                    _charBufferPos = movePosition ? 1u : 0u;
                                    return newCharBuffer[0];
                                }
                            }
                        }
                        return -1;
                    }
                }
                while (true);
            }
            finally
            {
                pool.Return(charBuffer);
            }
        }

        [LocalsInit(false)]
        private partial string? ReadLineInOtherEncoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

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
            charBuffer = pool.Rent(buffer.Length);
            do
            {
                currentPos = _bufferPos;
                currentLength = _bufferLength;
                if (currentPos < currentLength)
                {
                    int currentCount = unchecked((int)(currentLength - currentPos));
                    fixed (byte* source = buffer)
                    fixed (char* destination = charBuffer)
                    {
                        decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                            flush: false, out int consumedBytes, out int producedChars, out bool completed);
                        if (completed)
                            _bufferPos = currentLength;
                        else
                            _bufferPos = currentPos + MathHelper.MakeUnsigned(consumedBytes);
                        if (producedChars > 0)
                        {
                            nint indexOf = FindNewLineMarkInUtf16(destination, producedChars);
                            if (indexOf < 0)
                            {
                                builder.Append(destination, producedChars);
                            }
                            else
                            {
                                builder.Append(destination, (int)indexOf);
                                producedChars -= (int)indexOf + 1;
                                if (producedChars > 0)
                                {
                                    char[] newCharBuffer = new char[producedChars];
                                    fixed (char* ptr = newCharBuffer)
                                        UnsafeHelper.CopyBlockUnaligned(ptr, destination + indexOf + 1, unchecked((uint)producedChars * sizeof(char)));
                                    _charBuffer = newCharBuffer;
                                    _charBufferPos = 0;
                                }
                                break;
                            }
                        }
                    }
                }

                ReadStream();

                if (_eofReached)
                {
                    currentPos = _bufferPos;
                    currentLength = _bufferLength;
                    if (currentPos < currentLength)
                    {
                        _bufferPos = currentLength;
                        int currentCount = unchecked((int)(currentLength - currentPos));
                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                                flush: true, out int _, out int producedChars, out bool _);
                            if (producedChars > 0)
                            {
                                nint indexOf = FindNewLineMarkInUtf16(destination, producedChars);
                                if (indexOf < 0)
                                {
                                    builder.Append(destination, producedChars);
                                }
                                else
                                {
                                    builder.Append(destination, (int)indexOf);
                                    producedChars -= (int)indexOf + 1;
                                    if (producedChars > 0)
                                    {
                                        char[] newCharBuffer = new char[producedChars];
                                        fixed (char* ptr = newCharBuffer)
                                            UnsafeHelper.CopyBlockUnaligned(ptr, destination + indexOf + 1, unchecked((uint)producedChars * sizeof(char)));
                                        _charBuffer = newCharBuffer;    
                                        _charBufferPos = 0;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    return builder.Length <= 0 ? null : builder.ToString();
                }
            } while (true);
            return builder.ToString();
        }

        [LocalsInit(false)]
        private partial string ReadToEndInOtherEncoding()
        {
            bool isEndOfStream = _eofReached && _bufferPos >= _bufferLength;

            char[]? charBuffer = _charBuffer;
            if (charBuffer is not null)
            {
                _charBuffer = null;
                if (isEndOfStream)
                    return new string(charBuffer);
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
            {
                fixed (char* ptr = charBuffer)
                    builder.Append(ptr + _charBufferPos, ptr + charBuffer.Length);
            }

            Decoder decoder = _encoding.GetDecoder();
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            charBuffer = pool.Rent(buffer.Length);
            try
            {
                do
                {
                    currentPos = _bufferPos;
                    currentLength = _bufferLength;
                    if (currentPos < currentLength)
                    {
                        int currentCount = unchecked((int)(currentLength - currentPos));

                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                                flush: true, out int consumedBytes, out int producedChars, out bool completed);
                            if (completed)
                                _bufferPos = currentLength;
                            else
                                _bufferPos = currentPos + MathHelper.MakeUnsigned(consumedBytes);
                            if (producedChars > 0)
                                builder.Append(destination, producedChars);
                        }
                    }
                    ReadStream();
                    if (_eofReached)
                    {
                        currentPos = _bufferPos;
                        currentLength = _bufferLength;
                        if (currentPos < currentLength)
                        {
                            _bufferPos = currentLength;
                            int currentCount = unchecked((int)(currentLength - currentPos));
                            fixed (byte* source = buffer)
                            fixed (char* destination = charBuffer)
                            {
                                decoder.Convert(source + currentPos, currentCount, destination, charBuffer.Length,
                                    flush: true, out int _, out int producedChars, out bool _);
                                if (producedChars > 0)
                                    builder.Append(destination, producedChars);
                            }
                        }
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

        private partial StringBase? ReadLineAsStringBaseInOtherEncoding()
        {
            string? result = ReadLineInOtherEncoding();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        private partial StringBase ReadToEndAsStringBaseInOtherEncoding()
            => StringBase.Create(ReadToEndInOtherEncoding(), StringCreateOptions.None);
    }
}
