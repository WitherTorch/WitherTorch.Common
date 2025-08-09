using System.IO;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.IO.Internals
{
    internal abstract class AsciiBasedStreamReader : CustomStreamReaderBase
    {
        protected AsciiBasedStreamReader(Stream stream, int bufferSize, bool leaveOpen) : base(stream, bufferSize, leaveOpen) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe nuint? FindNewLineMark(byte[] buffer, nuint offset, nuint length)
        {
            if (offset >= length)
                return null;
            fixed (byte* ptr = buffer)
            {
                byte* startPointer = ptr + offset;
                nuint scanLength = length - offset;
                nuint? result = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\0');
                if (result.HasValue)
                    scanLength = result.Value;
                nuint? result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\r');
                if (result2.HasValue)
                {
                    result = result2;
                    scanLength = result2.Value;
                }
                result2 = SequenceHelper.IndexOf(startPointer, scanLength, (byte)'\n');
                return result2.HasValue ? result2 : result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe bool TryReadLineIntoPooledList(byte[] buffer, PooledList<byte> list) // Return value: stream is EOF or not
        {
            nuint currentPos, currentLength;
            nuint? indexOf;
            while ((indexOf = FindNewLineMark(buffer, currentPos = _bufferPos, currentLength = _bufferLength)) is null)
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

            nuint indexOfReal = indexOf.Value;
            fixed (byte* ptr = buffer)
            {
                list.AddRange(ptr, currentPos, indexOfReal);
                byte* ptrIndexOf = ptr + currentPos + indexOfReal;
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
