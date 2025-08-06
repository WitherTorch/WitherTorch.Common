using System.IO;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.IO
{
    unsafe partial class CustomStreamReader : TextReader
    {
        private void ReadStream()
        {
            if (_eofReached)
                return;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nint FindNewLineMarkInUtf16(char* ptr, nint length)
        {
            nint result = SequenceHelper.IndexOf(ptr, length, '\0');
            if (result >= 0)
                length = result;
            nint result2 = SequenceHelper.IndexOf(ptr, length, '\r');
            if (result2 >= 0)
                length = result = result2;
            result2 = SequenceHelper.IndexOf(ptr, length, '\n');
            if (result2 >= 0)
                result = result2;
            return result < 0 ? result : result * 2;
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
                nint result2 = SequenceHelper.IndexOf(startPointer, scanLength, '\r');
                if (result2 >= 0)
                    scanLength = result = result2;
                result2 = SequenceHelper.IndexOf(startPointer, scanLength, '\n');
                if (result2 >= 0)
                    result = result2;
                return result < 0 ? result : result * 2;
            }
        }

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
                nint result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\r');
                if (result2 >= 0)
                    scanLength = result = result2;
                result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\n');
                return result2 >= 0 ? result2 : result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryReadLineIntoBuffer_AsciiLike(PooledList<byte> list) // Return value: stream is EOF or not
        {
            byte[] buffer = _buffer;
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
                    return true;
                }
            }

            fixed (byte* ptr = buffer)
            {
                list.AddRange(ptr, currentPos, unchecked((nuint)indexOf));
                byte* ptrIndexOf = ptr + currentPos + indexOf;
                if (ptrIndexOf < (ptr + currentLength) && ptrIndexOf[1] == (byte)'\n')
                    indexOf++;
            }
            _bufferPos = currentPos + unchecked((nuint)indexOf) + 1;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReadToEndIntoBuffer_AsciiLike(PooledList<byte> list)
        {
            byte[] buffer = _buffer;
            nuint currentPos, currentLength;
            while (true)
            {
                currentPos = _bufferPos;
                currentLength = _bufferLength;
                if (currentPos < currentLength)
                {
                    _bufferPos = currentLength;
                    fixed (byte* ptr = buffer)
                        list.AddRange(ptr, currentPos, currentLength - currentPos);
                }
                ReadStream();
                if (_eofReached)
                {
                    currentPos = _bufferPos;
                    currentLength = _bufferLength;
                    if (currentPos < currentLength)
                    {
                        _bufferPos = currentLength;
                        fixed (byte* ptr = buffer)
                            list.AddRange(ptr, currentPos, currentLength - currentPos);
                    }
                    break;
                }
            }
        }

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
    }
}
