using System.IO;
using System.Text;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    internal sealed class AsciiStreamReader : AsciiBasedStreamReader
    {
        private static readonly Encoding _encoding = Encoding.ASCII;

        public override Encoding CurrentEncoding => _encoding;

        public AsciiStreamReader(Stream stream, int bufferSize, bool leaveOpen) : base(stream, bufferSize, leaveOpen) { }

        protected override char? ReadCharacterCore(byte[] buffer, bool movePosition)
        {
            nuint currentPos, nextPos;
            while ((nextPos = (currentPos = _bufferPos) + 1) >= _bufferLength)
            {
                ReadStream();
                if (CheckEndOfStream(fullyCheck: false))
                {
                    _bufferPos = _bufferLength;
                    return null;
                }
            }
            if (movePosition)
                _bufferPos = nextPos;
            return unchecked((char)buffer[currentPos]);
        }

        [LocalsInit(false)]
        protected override unsafe string? ReadLineCore(byte[] buffer)
        {
            if (CheckEndOfStream(fullyCheck: true))
                return null;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
            nuint currentPos, currentLength;
            nuint? indexOf;

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] charBuffer = pool.Rent(buffer.Length);
            try
            {
                while ((indexOf = FindNewLineMark(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) is null)
                {
                    if (currentPos < currentLength)
                    {
                        fixed (byte* source = buffer)
                        fixed (char* destination = charBuffer)
                        {
                            char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + currentLength, destination, destination + currentLength);
                            SequenceHelper.ReplaceGreaterThan(destination, unchecked((nuint)(destinationEnd - destination)), AsciiEncodingHelper.AsciiEncodingLimit, '?');
                            builder.Append(destination, destinationEnd);
                        }
                        _bufferPos = currentLength;
                    }
                    ReadStream();
                    if (CheckEndOfStream(fullyCheck: false))
                    {
                        _bufferPos = _bufferLength;
                        return builder.Length <= 0 ? null : builder.ToString();
                    }
                }

                nuint indexOfReal = indexOf.Value;
                fixed (byte* source = buffer)
                {
                    fixed (char* destination = charBuffer)
                    {
                        char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + indexOfReal, destination, destination + currentLength);
                        SequenceHelper.ReplaceGreaterThan(destination, unchecked((nuint)(destinationEnd - destination)), AsciiEncodingHelper.AsciiEncodingLimit, '?');
                        builder.Append(destination, destinationEnd);
                    }
                    byte* ptrIndexOf = source + currentPos + indexOfReal;
                    if (*ptrIndexOf == (byte)'\r')
                    {
                        ptrIndexOf++;
                        if (ptrIndexOf < (source + currentLength) && *ptrIndexOf == (byte)'\n')
                            indexOf++;
                    }
                }
                _bufferPos = currentPos + unchecked((nuint)indexOf) + 1;
            }
            finally
            {
                pool.Return(charBuffer);
            }
            return builder.ToString();
        }

        [LocalsInit(false)]
        protected override unsafe string ReadToEndCore(byte[] buffer)
        {
            if (CheckEndOfStream(fullyCheck: true))
                return string.Empty;

            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* stackBuffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(stackBuffer, Limits.MaxStackallocChars);
            }
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
                            char* destinationEnd = AsciiEncodingHelper.WriteToUtf16Buffer(source + currentPos, source + currentLength, destination, destination + currentLength);
                            SequenceHelper.ReplaceGreaterThan(destination, unchecked((nuint)(destinationEnd - destination)), AsciiEncodingHelper.AsciiEncodingLimit, '?');
                            builder.Append(destination, destinationEnd);
                        }
                        _bufferPos = currentLength;
                    }
                    ReadStream();
                    if (CheckEndOfStream(fullyCheck: false))
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

        protected override unsafe StringBase? ReadLineAsStringBaseCore(byte[] buffer)
        {
            if (CheckEndOfStream(fullyCheck: true))
                return null;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            bool isEndOfStream = TryReadLineIntoPooledList(buffer, list);
            try
            {
                (buffer, int count) = list;
                if (count <= 0)
                    return isEndOfStream ? null : StringBase.Empty;
                fixed (byte* ptr = buffer)
                {
                    SequenceHelper.ReplaceGreaterThan(ptr, unchecked((nuint)count), AsciiEncodingHelper.AsciiEncodingLimit_InByte, (byte)'?');
                    return StringBase.CreateAsciiString(ptr, 0u, unchecked((nuint)count));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        protected override unsafe StringBase ReadToEndAsStringBaseCore(byte[] buffer)
        {
            if (CheckEndOfStream(fullyCheck: true))
                return StringBase.Empty;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            ReadToEndIntoPooledList(buffer, list);
            try
            {
                (buffer, int count) = list;
                if (count <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = buffer)
                {
                    SequenceHelper.ReplaceGreaterThan(ptr, unchecked((nuint)count), AsciiEncodingHelper.AsciiEncodingLimit_InByte, (byte)'?');
                    return StringBase.CreateAsciiString(ptr, 0u, unchecked((nuint)count));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
