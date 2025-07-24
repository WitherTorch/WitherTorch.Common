using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected override unsafe nuint GetSplitCount(char separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            string source = _value;
            nuint result = 1;

            using PooledList<StringSlice> list = new PooledList<StringSlice>(pool);
            fixed (char* ptr = source)
            {
                char* previous = ptr, iterator = ptr, ptrEnd = ptr + source.Length;
                while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, separator)) != null)
                {
                    unchecked
                    {
                        list.Add(new StringSlice(this, (nuint)(previous - ptr), (nuint)(iterator - previous)));
                    }
                    result++;
                    previous = iterator;
                    iterator++;
                }
                unchecked
                {
                    list.Add(new StringSlice(this, (nuint)(previous - ptr), (nuint)(ptrEnd - previous)));
                }
            }
            sliceBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        protected override unsafe nuint GetSplitCount(string separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            fixed (char* ptr = separator)
                return GetSplitCount(ptr, unchecked((nuint)separator.Length), pool, out sliceBuffer);
        }

        protected override unsafe nuint GetSplitCount(StringBase separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
            => separator switch
            {
                Latin1String latin1 => GetSplitCount(latin1, pool, out sliceBuffer),
                Utf16String utf16 => GetSplitCount(utf16._value, pool, out sliceBuffer),
                _ => base.GetSplitCount(separator, pool, out sliceBuffer)
            };

        private unsafe nuint GetSplitCount(Latin1String separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            fixed (byte* ptr = separator.GetInternalRepresentation())
                return GetSplitCount(ptr, unchecked((nuint)separator.Length), pool, out sliceBuffer);
        }

        private unsafe nuint GetSplitCount(byte* separator, nuint separatorLength, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            ArrayPool<char> bufferPool = ArrayPool<char>.Shared;
            char[] buffer = bufferPool.Rent(separatorLength);
            try
            {
                fixed (char* ptr = buffer)
                {
                    InternalStringHelper.WidenAndCopyTo(separator, separatorLength, ptr);
                    return GetSplitCount(ptr, separatorLength, pool, out sliceBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }

        private unsafe nuint GetSplitCount(char* separator, nuint separatorLength, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            string source = _value;
            nuint result = 1;

            using PooledList<StringSlice> list = new PooledList<StringSlice>(pool);
            fixed (char* ptrSource = source)
            {
                char* previous = ptrSource, iterator = ptrSource, ptrEnd = ptrSource + source.Length;
                while ((iterator = InternalStringHelper.PointerIndexOf(iterator, ptrEnd, separator, separatorLength)) != null)
                {
                    unchecked
                    {
                        list.Add(new StringSlice(this, (nuint)(previous - ptrSource), (nuint)(iterator - previous)));
                    }
                    result++;
                    previous = iterator;
                    iterator += separatorLength;
                }
                unchecked
                {
                    list.Add(new StringSlice(this, (nuint)(previous - ptrSource), (nuint)(ptrEnd - previous)));
                }
            }
            sliceBuffer = list.DestructAndReturnBuffer();
            return result;
        }
    }
}
