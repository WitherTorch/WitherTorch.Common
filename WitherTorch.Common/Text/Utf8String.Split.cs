using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            if (!enumerator.MoveNext())
            {
                rangeBuffer = null;
                return 1;
            }

            nuint result = 1, startIndex = 0, iterator = 0, endIndex = (nuint)_length;
            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);

            do
            {
                if (enumerator.Current != separator)
                {
                    iterator++;
                    continue;
                }
                list.Add(new SplitRange(startIndex, iterator - startIndex));
                startIndex = ++iterator;
                result++;
            } while (enumerator.MoveNext());

            list.Add(new SplitRange(startIndex, endIndex - startIndex));
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        protected override unsafe nuint GetSplitCount(string separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (separatorLength == 1)
                return GetSplitCount(separator[0], pool, out rangeBuffer);

            return GetSplitCountUtf16(separator, separatorLength, pool, out rangeBuffer);
        }

        protected override nuint GetSplitCount(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (separatorLength == 1)
                return GetSplitCount(separator.GetCharAt(0), pool, out rangeBuffer);

            return separator switch
            {
                Utf8String utf8 => GetSplitCountUtf8(utf8._value, separatorLength, pool, out rangeBuffer),
                Latin1String latin1 => GetSplitCountLatin1(latin1.GetInternalRepresentation(), separatorLength, pool, out rangeBuffer),
                Utf16String utf16 => GetSplitCountUtf16(utf16.GetInternalRepresentation(), separatorLength, pool, out rangeBuffer),
                _ => GetSplitCountOther(separator, separatorLength, pool, out rangeBuffer)
            };
        }

        private unsafe nuint GetSplitCountUtf8(byte[] separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            using CharEnumerator enumeratorSource = new CharEnumerator(_value);
            using CharEnumerator enumeratorValue = new CharEnumerator(separator);
            if (!enumeratorSource.MoveNext() || !enumeratorValue.MoveNext())
            {
                rangeBuffer = null;
                return 1;
            }

            nuint result = 1, startIndex = 0, iterator = 0, endIndex = (nuint)_length;
            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);
            char separatorHead = enumeratorValue.Current;

            do
            {
                if (enumeratorSource.Current != separatorHead)
                {
                    iterator++;
                    continue;
                }
                bool flag = true;
                using CharEnumerator loopEnumeratorSource = new CharEnumerator(enumeratorSource);
                using CharEnumerator loopEnumeratorValue = new CharEnumerator(enumeratorValue);
                for (nuint i = 1; i < length; i++)
                {
                    if (!loopEnumeratorSource.MoveNext() || loopEnumeratorSource.Current != loopEnumeratorValue.Current)
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    iterator++;
                    continue;
                }
                list.Add(new SplitRange(startIndex, iterator - startIndex));
                iterator += length;
                startIndex = iterator;
                result++;
            } while (enumeratorSource.MoveNext());

            list.Add(new SplitRange(startIndex, endIndex - startIndex));
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        private unsafe nuint GetSplitCountLatin1(byte[] separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            fixed (byte* ptr = separator)
                return GetSplitCountLatin1(ptr, length, pool, out rangeBuffer);
        }

        private unsafe nuint GetSplitCountLatin1(byte* separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            if (!enumerator.MoveNext())
            {
                rangeBuffer = null;
                return 1;
            }

            nuint result = 1, startIndex = 0, iterator = 0, endIndex = (nuint)_length;
            byte separatorHead = separator[0];
            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);

            do
            {
                if (enumerator.Current != separatorHead)
                {
                    iterator++;
                    continue;
                }
                bool flag = true;
                using CharEnumerator loopEnumerator = new CharEnumerator(enumerator);
                for (nuint j = 1; j < length; j++)
                {
                    if (!loopEnumerator.MoveNext() || loopEnumerator.Current != separator[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    iterator++;
                    continue;
                }
                list.Add(new SplitRange(startIndex, iterator - startIndex));
                iterator += length;
                startIndex = iterator;
                result++;
            } while (enumerator.MoveNext());

            list.Add(new SplitRange(startIndex, endIndex - startIndex));
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        private unsafe nuint GetSplitCountUtf16(string separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            fixed (char* ptr = separator)
                return GetSplitCountUtf16(ptr, length, pool, out rangeBuffer);
        }

        private unsafe nuint GetSplitCountUtf16(char* separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            if (!enumerator.MoveNext())
            {
                rangeBuffer = null;
                return 1;
            }

            nuint result = 1, startIndex = 0, iterator = 0, endIndex = (nuint)_length;
            char separatorHead = separator[0];
            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);

            do
            {
                if (enumerator.Current != separatorHead)
                {
                    iterator++;
                    continue;
                }
                bool flag = true;
                using CharEnumerator loopEnumerator = new CharEnumerator(enumerator);
                for (nuint j = 1; j < length; j++)
                {
                    if (!loopEnumerator.MoveNext() || loopEnumerator.Current != separator[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    iterator++;
                    continue;
                }
                list.Add(new SplitRange(startIndex, iterator - startIndex));
                iterator += length;
                startIndex = iterator;
                result++;
            } while (enumerator.MoveNext());

            list.Add(new SplitRange(startIndex, endIndex - startIndex));
            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }

        private unsafe nuint GetSplitCountOther(StringBase separator, nuint length, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            ArrayPool<char> bufferPool = ArrayPool<char>.Shared;
            char[] buffer = bufferPool.Rent(length);
            try
            {
                fixed (char* ptr = buffer)
                {
                    separator.CopyToCore(ptr, 0, length);
                    return GetSplitCountUtf16(ptr, length, pool, out rangeBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }
    }
}
