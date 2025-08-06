using System;
using System.IO;
using System.Text;
using System.Threading;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.IO.Internals
{
    internal sealed class Latin1StreamReader : AsciiBasedStreamReaderBase
    {
        private static readonly LazyTiny<Encoding> _encodingLazy = new LazyTiny<Encoding>(GetEncoding, LazyThreadSafetyMode.PublicationOnly);

        public override Encoding CurrentEncoding => _encodingLazy.Value;

        public Latin1StreamReader(Stream stream, int bufferSize, bool leaveOpen) : base(stream, bufferSize, leaveOpen) { }

        private static Encoding GetEncoding()
        {
#if NET8_0_OR_GREATER
            return Encoding.Latin1;
#else
            try
            {
                return Encoding.GetEncoding(codepage: 28591);
            }
            catch (Exception)
            {
                return Encoding.GetEncoding(codepage: 1252); // Windows-1252 編碼頁，雖然與 Latin-1 (ISO-8859-1) 不完全一樣，但可做為佔位符使用
            }
#endif
        }

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
                    if (CheckEndOfStream(fullyCheck: false))
                    {
                        _bufferPos = _bufferLength;
                        return builder.Length <= 0 ? null : builder.ToString();
                    }
                }

                fixed (byte* source = buffer)
                {
                    fixed (char* destination = charBuffer)
                    {
                        char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + indexOf, destination, destination + currentLength);
                        builder.Append(destination, destinationEnd);
                    }
                    byte* ptrIndexOf = source + currentPos + indexOf;
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
                            char* destinationEnd = Latin1EncodingHelper.WriteToUtf16Buffer(source + currentPos, source + currentLength, destination, destination + currentLength);
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

        protected override unsafe StringBase ReadToEndAsStringBaseCore(byte[] buffer)
        {
            if (CheckEndOfStream(fullyCheck: true))
                return StringBase.Empty;

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            ReadToEndIntoPooledList(buffer, list);
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
