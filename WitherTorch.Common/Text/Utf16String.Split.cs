using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected override unsafe nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            string source = _value;
            nuint result = 1;

            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);
            fixed (char* ptr = source)
            {
                char* previous = ptr, iterator = ptr, ptrEnd = ptr + source.Length;
                while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, separator)) != null)
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
            (rangeBuffer, _) = list;
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
                Utf16String utf16 => GetSplitCount(utf16, separatorLength, pool, out rangeBuffer),
                _ => GetSplitCount_Other(separator, separatorLength, pool, out rangeBuffer)
            };

        private unsafe nuint GetSplitCount(char* separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            string source = _value;
            nuint result = 1;

            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);
            fixed (char* ptrSource = source)
            {
                char* previous = ptrSource, iterator = ptrSource, ptrEnd = ptrSource + source.Length;
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
            (rangeBuffer, _) = list;
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
