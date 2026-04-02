using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

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

        protected override nuint GetSplitCount(StringWrapper separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
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

        private unsafe nuint GetSplitCount_Other(StringWrapper separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            NativeMemoryPool bufferPool = NativeMemoryPool.Shared;
            TypedNativeMemoryBlock<char> buffer = bufferPool.Rent<char>(separatorLength);
            try
            {
                char* temp = buffer.NativePointer;
                separator.CopyToCore(temp, 0, separatorLength);
                return GetSplitCount(temp, separatorLength, pool, out rangeBuffer);
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }
    }
}
