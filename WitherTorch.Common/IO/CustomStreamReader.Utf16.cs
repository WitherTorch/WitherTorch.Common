using System.IO;

using LocalsInit;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private partial int ReadOneInUtf16Encoding(bool movePosition)
        {
            byte[] buffer = _buffer;
            nuint currentPos, nextPos;
            while ((nextPos = (currentPos = _bufferPos) + 2) >= _bufferLength)
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
            fixed (byte* ptr = buffer)
                return *(char*)(ptr + currentPos);
        }

        [LocalsInit(false)]
        private partial string? ReadLineInUtf16Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return null;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* charBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(charBuffer, Limits.MaxStackallocChars);
            }
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;
            nint indexOf;
            while ((indexOf = FindNewLineMarkInUtf16(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
            {
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                    {
                        char* startPointer = (char*)(ptr + currentPos);
                        nuint count = (currentPos - currentLength) / 2;
                        builder.Append(startPointer, startPointer + count);
                        _bufferPos += count * 2;
                    }
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return builder.Length <= 0 ? null : builder.ToString();
                }
            }

            _bufferPos = currentPos + unchecked((nuint)indexOf) + 2;

            fixed (byte* ptr = buffer)
            {
                char* startPointer = (char*)(ptr + currentPos);
                builder.Append(startPointer, unchecked((int)indexOf));
            }
            return builder.ToString();
        }

        [LocalsInit(false)]
        private partial string ReadToEndInUtf16Encoding()
        {
            if (_eofReached && _bufferPos >= _bufferLength)
                return string.Empty;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* charBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(charBuffer, Limits.MaxStackallocChars);
            }
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
                        char* startPointer = (char*)(ptr + currentPos);
                        nuint count = (currentPos - currentLength) / 2;
                        builder.Append(startPointer, startPointer + count);
                        _bufferPos += count * 2;
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

        private partial StringBase? ReadLineAsStringBaseInUtf16Encoding()
        {
            string? result = ReadLineInUtf16Encoding();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        private partial StringBase ReadToEndAsStringBaseInUtf16Encoding()
        {
            string result = ReadToEndInUtf16Encoding();
            return StringBase.Create(result, StringCreateOptions.None);
        }
    }
}
