using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Latin1String
    {
        protected override unsafe nuint GetSplitCount(char separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            if (separator > InternalStringHelper.Latin1StringLimit)
            {
                sliceBuffer = null;
                return 1;
            }

            byte[] source = _value;
            byte target = unchecked((byte)separator);
            nuint result = 1;

            using PooledList<StringSlice> list = new PooledList<StringSlice>(pool);
            fixed (byte* ptr = source)
            {
                byte* previous = ptr, iterator = ptr, ptrEnd = ptr + _length;
                while ((iterator = SequenceHelper.PointerIndexOf(iterator, ptrEnd, target)) != null)
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
                Utf16String utf16 => GetSplitCount(utf16.GetInternalRepresentation(), pool, out sliceBuffer),
                _ => base.GetSplitCount(separator, pool, out sliceBuffer)
            };

        private unsafe nuint GetSplitCount(Latin1String separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            fixed (byte* ptr = separator._value)
                return GetSplitCount(ptr, unchecked((nuint)separator._length), pool, out sliceBuffer);
        }

        private unsafe nuint GetSplitCount(char* separator, nuint separatorLength, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            if (SequenceHelper.ContainsGreaterThan(separator, separatorLength, InternalStringHelper.Latin1StringLimit))
            {
                sliceBuffer = null;
                return 1;
            }

            ArrayPool<byte> bufferPool = ArrayPool<byte>.Shared;
            byte[] buffer = bufferPool.Rent(separatorLength);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    InternalStringHelper.NarrowAndCopyTo(separator, separatorLength, ptr);
                    return GetSplitCount(ptr, separatorLength, pool, out sliceBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }

        private unsafe nuint GetSplitCount(byte* separator, nuint separatorLength, ArrayPool<StringSlice> pool, out StringSlice[] sliceBuffer)
        {
            byte[] source = _value;
            nuint result = 1;

            using PooledList<StringSlice> list = new PooledList<StringSlice>(pool);
            fixed (byte* ptrSource = source)
            {
                byte* previous = ptrSource, iterator = ptrSource, ptrEnd = ptrSource + _length;
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
