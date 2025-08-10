using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class AsciiLikeString
    {
        protected override unsafe nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (separator > GetCharacterLimit())
            {
                rangeBuffer = null;
                return 1;
            }

            byte[] source = _value;
            byte target = unchecked((byte)separator);
            nuint result = 1;

            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);
            fixed (byte* ptr = source)
            {
                byte* previous = ptr, iterator = ptr, ptrEnd = ptr + _length;
                while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, target)) != null)
                {
                    unchecked
                    {
                        list.Add(new SplitRange((nuint)(previous - ptr), (nuint)(iterator - previous)));
                    }
                    result++;
                    previous = iterator;
                    iterator++;
                }
                unchecked
                {
                    list.Add(new SplitRange((nuint)(previous - ptr), (nuint)(ptrEnd - previous)));
                }
            }
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        protected override unsafe nuint GetSplitCount(string separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            fixed (char* ptr = separator)
                return GetSplitCount(ptr, separatorLength, pool, out rangeBuffer);
        }

        protected override unsafe nuint GetSplitCount(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
            => separator switch
            {
                AsciiLikeString ascii => GetSplitCount(ascii, separatorLength, pool, out rangeBuffer),
                Utf16String utf16 => GetSplitCount(utf16, separatorLength, pool, out rangeBuffer),
                Utf8String utf8 => GetSplitCount(utf8, separatorLength, pool, out rangeBuffer),
                _ => GetSplitCount_Other(separator, separatorLength, pool, out rangeBuffer)
            };

        private unsafe nuint GetSplitCount(AsciiLikeString separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            fixed (byte* ptr = separator._value)
                return GetSplitCount(ptr, separatorLength, pool, out rangeBuffer);
        }

        private unsafe nuint GetSplitCount(Utf8String separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (GetCharacterLimit() > AsciiEncodingHelper.AsciiEncodingLimit_InByte)
                return GetSplitCount_Other(separator, separatorLength, pool, out rangeBuffer);

            fixed (byte* ptr = separator.GetInternalRepresentation())
                return GetSplitCount(ptr, separatorLength, pool, out rangeBuffer);
        }

        private unsafe nuint GetSplitCount(Utf16String separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            fixed (char* ptr = separator.GetInternalRepresentation())
                return GetSplitCount(ptr, separatorLength, pool, out rangeBuffer);
        }

        private unsafe nuint GetSplitCount(char* separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (SequenceHelper.ContainsGreaterThan(separator, separatorLength, Latin1EncodingHelper.Latin1EncodingLimit))
            {
                rangeBuffer = null;
                return 1;
            }

            ArrayPool<byte> bufferPool = ArrayPool<byte>.Shared;
            byte[] buffer = bufferPool.Rent(separatorLength);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(separator, ptr, separatorLength);
                    return GetSplitCount(ptr, separatorLength, pool, out rangeBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }

        private unsafe nuint GetSplitCount(byte* separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[] rangeBuffer)
        {
            byte[] source = _value;
            nuint result = 1;

            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);
            fixed (byte* ptrSource = source)
            {
                byte* previous = ptrSource, iterator = ptrSource, ptrEnd = ptrSource + _length;
                while ((iterator = InternalSequenceHelper.PointerIndexOf(iterator, ptrEnd, separator, separatorLength)) != null)
                {
                    unchecked
                    {
                        list.Add(new SplitRange((nuint)(previous - ptrSource), (nuint)(iterator - previous)));
                    }
                    result++;
                    previous = iterator;
                    iterator += separatorLength;
                }
                unchecked
                {
                    list.Add(new SplitRange((nuint)(previous - ptrSource), (nuint)(ptrEnd - previous)));
                }
            }
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        private unsafe nuint GetSplitCount_Other(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            ArrayPool<char> bufferPool = ArrayPool<char>.Shared;
            char[] buffer = bufferPool.Rent(separatorLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    separator.CopyToCore(temp, 0, separatorLength);
                    return GetSplitCount(temp, separatorLength, pool, out rangeBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }
    }
}
