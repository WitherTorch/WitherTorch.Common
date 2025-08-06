using System.IO;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.IO.Internals
{
    internal abstract class AsciiBasedStreamReaderBase : CustomStreamReaderBase
    {
        protected AsciiBasedStreamReaderBase(Stream stream, int bufferSize, bool leaveOpen) : base(stream, bufferSize, leaveOpen) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe nint FindNewLineMarkInAscii(byte[] buffer, nuint offset, nuint length)
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
        protected unsafe bool TryReadLineIntoPooledList(byte[] buffer, PooledList<byte> list) // Return value: stream is EOF or not
        {
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
                if (CheckEndOfStream(fullyCheck: false))
                {
                    _bufferPos = _bufferLength;
                    return true;
                }
            }

            fixed (byte* ptr = buffer)
            {
                list.AddRange(ptr, currentPos, unchecked((nuint)indexOf));
                byte* ptrIndexOf = ptr + currentPos + indexOf;
                if (*ptrIndexOf == (byte)'\r')
                {
                    ptrIndexOf++;
                    if (ptrIndexOf < (ptr + currentLength) && *ptrIndexOf == (byte)'\n')
                        indexOf++;
                }
            }
            _bufferPos = currentPos + unchecked((nuint)indexOf) + 1;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void ReadToEndIntoPooledList(byte[] buffer, PooledList<byte> list)
        {
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
                if (CheckEndOfStream(fullyCheck: false))
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
    }
}
