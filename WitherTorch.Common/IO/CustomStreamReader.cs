using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using InlineIL;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    public unsafe sealed class CustomStreamReader : TextReader
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly byte[] _buffer;
        private readonly void* _readOneFunc, _readLineFunc, _readLineAsStringBaseFunc;
        private readonly bool _leaveOpen;

        private char[]? _charBuffer;
        private nuint _bufferPos, _bufferLength, _charBufferPos;
        private bool _eofReached, _disposed;

        public Stream BaseStream => _stream;
        public Encoding CurrentEncoding => _encoding;
        public bool EndOfStream
        {
            get
            {
                if (!_eofReached)
                    return false;

                if (_bufferPos < _bufferLength)
                    return false;

                if (_charBuffer is not null && _charBufferPos < unchecked((nuint)_charBuffer.Length))

                    ReadStream();

                if (_bufferPos < _bufferLength)
                    return false;

                return _eofReached;
            }
        }

        public CustomStreamReader(Stream stream, Encoding encoding) : this(stream, encoding, bufferSize: 1024, leaveOpen: false) { }

        public CustomStreamReader(Stream stream, Encoding encoding, int bufferSize) : this(stream, encoding, bufferSize, leaveOpen: false) { }

        public CustomStreamReader(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen)
        {
            _stream = stream;
            _buffer = new byte[bufferSize];
            _encoding = encoding;
            _leaveOpen = leaveOpen;
            _bufferLength = 0;
            _bufferPos = 0;
            _disposed = false;

            void* readOneFunc, readLineFunc, readLineAsStringBaseFunc;
            switch (encoding.CodePage)
            {
                case 1200: // UTF-16 code page (little-endian encoding)
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInUtf16Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInUtf16Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInUtf16Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 65001: // UTF-8 code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInUtf8Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInUtf8Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInUtf8Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 28591: // Latin-1 code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInLatin1Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInLatin1Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInLatin1Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 20127: // Ascii code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInAsciiEncoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInAsciiEncoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInAsciiEncoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                default:
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInOtherEncoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInOtherEncoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInOtherEncoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
            }
            _readOneFunc = readOneFunc;
            _readLineFunc = readLineFunc;
            _readLineAsStringBaseFunc = readLineAsStringBaseFunc;
        }

        public override int Peek()
        {
            IL.Emit.Ldarg_0();
            IL.Push(false);
            IL.Push(_readOneFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(int), typeof(bool)));
            return IL.Return<int>();
        }

        public override int Read()
        {
            IL.Emit.Ldarg_0();
            IL.Push(true);
            IL.Push(_readOneFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(int), typeof(bool)));
            return IL.Return<int>();
        }

        public override string? ReadLine()
        {
            IL.Emit.Ldarg_0();
            IL.Push(_readLineFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(string)));
            return IL.Return<string>();
        }

        public StringBase? ReadLineAsStringBase()
        {
            IL.Emit.Ldarg_0();
            IL.Push(_readLineAsStringBaseFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(StringBase)));
            return IL.Return<StringBase>();
        }

        public Task<StringBase?> ReadLineAsStringBaseAsync()
            => Task.Factory.StartNew(ReadLineAsStringBase, TaskCreationOptions.LongRunning);

        private unsafe int ReadOneInUtf16Encoding(bool movePosition)
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
            while ((nextPos = currentPos + 2) >= _bufferLength)
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
                return *(char*)(ptr + currentPos);
        }

        private unsafe int ReadOneInUtf8Encoding(bool movePosition)
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

        private unsafe int ReadOneInLatin1Encoding(bool movePosition)
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

        private unsafe int ReadOneInAsciiEncoding(bool movePosition)
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
            int result = buffer[currentPos];
            if (result > Latin1EncodingHelper.AsciiEncodingLimit_InByte)
                result = '?';
            return result;
        }

        private unsafe int ReadOneInOtherEncoding(bool movePosition)
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string? ReadLineInUtf16Encoding()
        {
            if (_eofReached)
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
                fixed (byte* ptr = buffer)
                {
                    char* startPointer = (char*)(ptr + currentPos);
                    nuint count = (currentPos - currentLength) / 2;
                    builder.Append(startPointer, startPointer + count);
                    _bufferPos += count * 2;
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    return builder.Length <= 0 ? null : builder.ToString();
                }
            }

            _bufferPos = SkipAllNewLineMarkInUtf16(buffer, currentPos + unchecked((nuint)indexOf) + 2, currentLength);

            fixed (byte* ptr = buffer)
            {
                char* startPointer = (char*)(ptr + currentPos);
                builder.Append(startPointer, unchecked((int)indexOf));
            }
            return builder.ToString();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string? ReadLineInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached;

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

            _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string? ReadLineInLatin1Encoding()
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

                _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string? ReadLineInAsciiEncoding()
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

                _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string? ReadLineInOtherEncoding()
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

        private StringBase? ReadLineAsStringBaseInUtf16Encoding()
        {
            string? result = ReadLineInUtf16Encoding();
            return result is null ? null : StringBase.Create(result);
        }

        private StringBase? ReadLineAsStringBaseInUtf8Encoding()
        {
            bool isEndOfStream = _eofReached;

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
            nuint currentPos, currentLength;
            nint indexOf;
            while ((indexOf = FindNewLineMarkInAscii(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
            {
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                        list.AddRange(ptr, currentPos, currentLength - currentPos);
                    _bufferPos = currentLength;
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    if (list.Count > 0)
                        goto Result;
                    return null;
                }
            }

            _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

            fixed (byte* ptr = buffer)
                list.AddRange(ptr, currentPos, unchecked((nuint)indexOf));

            int count = list.Count;
            if (count <= 0)
                return StringBase.Empty;

            Result:
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateUtf8String(ptr, 0u, unchecked((nuint)list.Count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private StringBase? ReadLineAsStringBaseInLatin1Encoding()
        {
            if (_eofReached)
                return null;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            nuint currentPos, currentLength;
            nint indexOf;
            while ((indexOf = FindNewLineMarkInAscii(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
            {
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                        list.AddRange(ptr, currentPos, currentPos - currentLength);
                    _bufferPos = currentLength;
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    if (list.Count > 0)
                        goto Result;
                    return null;
                }
            }

            _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

            fixed (byte* ptr = buffer)
                list.AddRange(ptr, currentPos, unchecked((nuint)indexOf));

            int count = list.Count;
            if (count <= 0)
                return StringBase.Empty;

            Result:
            buffer = list.DestructAndReturnBuffer();
            try
            {
                fixed (byte* ptr = buffer)
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)list.Count));
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private StringBase? ReadLineAsStringBaseInAsciiEncoding()
        {
            if (_eofReached)
                return null;

            byte[] buffer = _buffer;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            using PooledList<byte> list = new PooledList<byte>(pool, buffer.Length);
            nuint currentPos, currentLength;
            nint indexOf;
            while ((indexOf = FindNewLineMarkInAscii(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) < 0)
            {
                if (currentPos < currentLength)
                {
                    fixed (byte* ptr = buffer)
                        list.AddRange(ptr, currentPos, currentPos - currentLength);
                    _bufferPos = currentLength;
                }
                ReadStream();
                if (_eofReached)
                {
                    _bufferPos = _bufferLength;
                    if (list.Count > 0)
                        goto Result;
                    return null;
                }
            }

            _bufferPos = SkipAllNewLineMarkInAscii(buffer, currentPos + unchecked((nuint)indexOf) + 1, currentLength);

            fixed (byte* ptr = buffer)
                list.AddRange(ptr, currentPos, unchecked((nuint)indexOf));

            int count = list.Count;
            if (count <= 0)
                return StringBase.Empty;

            Result:
            buffer = list.DestructAndReturnBuffer();
            try
            {
                nuint castedCount = unchecked((nuint)list.Count);
                fixed (byte* ptr = buffer)
                {
                    byte* iterator = ptr;
                    byte* ptrEnd = ptr + castedCount;
                    while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, Latin1EncodingHelper.AsciiEncodingLimit_InByte)) != null)
                    {
                        *iterator = (byte)'?';
                        iterator++;
                    }
                    return StringBase.CreateLatin1String(ptr, 0, castedCount);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private StringBase? ReadLineAsStringBaseInOtherEncoding()
        {
            string? result = ReadLineInOtherEncoding();
            return result is null ? null : StringBase.Create(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nint FindNewLineMarkInUtf16(byte[] buffer, nuint offset, nuint length)
        {
            if (offset >= length)
                return -1;
            fixed (byte* ptr = buffer)
            {
                char* startPointer = (char*)(ptr + offset);
                nint scanLength = unchecked((nint)(length - offset)) / 2;
                nint result = SequenceHelper.IndexOf(startPointer, scanLength, '\0');
                if (result >= 0)
                    scanLength = result;
                nint result2 = SequenceHelper.IndexOf(startPointer, scanLength, '\n');
                if (result2 >= 0)
                    scanLength = result = result2;
                result2 = SequenceHelper.IndexOf(startPointer, scanLength, '\r');
                if (result2 >= 0)
                    result = result2;
                return result < 0 ? result : result * 2;
            }
        }

        private static nuint SkipAllNewLineMarkInUtf16(byte[] buffer, nuint offset, nuint length)
        {
            fixed (byte* ptr = buffer)
            {
                char* iterator = (char*)(ptr + offset);
                char* ptrEnd = (char*)(ptr + length);
                while (iterator < ptrEnd && IsNewLineMark(*iterator++)) ;

                return unchecked((nuint)((byte*)iterator - ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNewLineMark(char c) => c == '\0' || c == '\n' || c == '\r';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nint FindNewLineMarkInAscii(byte[] buffer, nuint offset, nuint length)
        {
            if (offset >= length)
                return -1;
            fixed (byte* ptr = buffer)
            {
                byte* startPointer = ptr + offset;
                nint scanLength = (nint)(length - offset);
                nint result = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\0');
                if (result >= 0)
                    scanLength = result;
                nint result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\n');
                if (result2 >= 0)
                    scanLength = result = result2;
                result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\r');
                return result2 >= 0 ? result2 : result;
            }
        }

        private static nuint SkipAllNewLineMarkInAscii(byte[] buffer, nuint offset, nuint length)
        {
            fixed (byte* ptr = buffer)
            {
                byte* iterator = ptr + offset;
                byte* ptrEnd = ptr + length;
                while (iterator < ptrEnd && IsNewLineMark(*iterator++)) ;

                return unchecked((nuint)(iterator - ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNewLineMark(byte c) => c == (byte)'\0' || c == (byte)'\n' || c == (byte)'\r';

        private string? ReadLineInCharBuffer(char[] charBuffer, bool isEndOfStream)
        {
            nuint currentPos = _charBufferPos;
            nuint length = unchecked((nuint)charBuffer.Length);
            if (currentPos >= length)
                return isEndOfStream ? string.Empty : null;

            fixed (char* ptr = charBuffer)
            {
                char* startPointer = ptr + currentPos;
                char* endPointer = ptr + length;
                char* ptrIndexOf = SequenceHelper.PointerIndexOf(startPointer, endPointer, '\n');
                if (ptrIndexOf < startPointer)
                    return isEndOfStream ? new string(startPointer, 0, unchecked((int)(endPointer - startPointer))) : null;
                if (ptrIndexOf == startPointer)
                    return string.Empty;
                _charBufferPos = unchecked((nuint)(ptrIndexOf - ptr));

                char* ptrIndexOfBackward = ptrIndexOf - 1;
                if (*ptrIndexOfBackward == '\r') // Check \r\n set
                    ptrIndexOf = ptrIndexOfBackward;
                if (ptrIndexOf == startPointer)
                    return string.Empty;

                return new string(startPointer, 0, unchecked((int)(ptrIndexOf - startPointer)));
            }
        }
        private void ReadStream()
        {
            nuint currentPos = _bufferPos;
            nuint currentLength = _bufferLength;
            if (currentPos == 0) // Just expand buffer
            {
                int castedCurrentLength = unchecked((int)currentLength);
                if (castedCurrentLength < _buffer.Length) // Buffer has space
                {
                    nuint readLength = ReadStreamCore(castedCurrentLength);
                    _bufferLength += readLength;
                }
                return;
            }
            if (currentPos >= currentLength) // buffer is empty
            {
                _bufferPos = 0;
                _bufferLength = ReadStreamCore(0);
                return;
            }
            nuint newPos = currentLength - currentPos;
            fixed (byte* ptr = _buffer)
                NativeMethods.MoveMemory(ptr, ptr + currentPos, newPos);
            _bufferPos = newPos;
            _bufferLength = newPos + ReadStreamCore(unchecked((int)newPos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private nuint ReadStreamCore(int position)
        {
            byte[] buffer = _buffer;
            int length = _stream.Read(buffer, position, buffer.Length - position);
            if (length <= 0)
            {
                _eofReached = true;
                return 0;
            }
            return unchecked((nuint)length);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_disposed)
                return;
            _disposed = true;

            if (!_leaveOpen)
                _stream.Dispose();
        }
    }
}
