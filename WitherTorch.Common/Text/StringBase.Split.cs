using System;
using System.Collections.Generic;
using System.Linq;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        public StringBase[] Split(char separator) => Split(separator, StringSplitOptions.None);

        public StringBase[] Split(char separator, StringSplitOptions options)
        {
            ArrayPool<SplitRange> rangePool = ArrayPool<SplitRange>.Shared;
            SplitRange[]? rangeBuffer = null;
            try
            {
                nuint splitCount = GetSplitCount(separator, rangePool, out rangeBuffer);
                if (ShouldRemoveEmptyEntries(options))
                    return SplitCore_RemoveEmptyEntries(rangeBuffer, splitCount);
                return SplitCore(rangeBuffer, splitCount);
            }
            finally
            {
                if (rangeBuffer is not null)
                    rangePool.Return(rangeBuffer);
            }
        }

        public StringBase[] Split(string separator) => Split(separator, StringSplitOptions.None);

        public StringBase[] Split(string separator, StringSplitOptions options)
        {
            int separatorLength = separator.Length;
            if (separatorLength <= 0)
                return Length <= 0 && ShouldRemoveEmptyEntries(options) ? Array.Empty<StringBase>() : [this];

            ArrayPool<SplitRange> rangePool = ArrayPool<SplitRange>.Shared;
            SplitRange[]? rangeBuffer = null;
            try
            {
                nuint splitCount = GetSplitCount(separator, unchecked((nuint)separatorLength), rangePool, out rangeBuffer);
                if (ShouldRemoveEmptyEntries(options))
                    return SplitCore_RemoveEmptyEntries(rangeBuffer, splitCount);
                return SplitCore(rangeBuffer, splitCount);
            }
            finally
            {
                if (rangeBuffer is not null)
                    rangePool.Return(rangeBuffer);
            }
        }

        public StringBase[] Split(StringBase separator) => Split(separator, StringSplitOptions.None);

        public StringBase[] Split(StringBase separator, StringSplitOptions options)
        {
            int separatorLength = separator.Length;
            if (separatorLength <= 0)
                return Length <= 0 && ShouldRemoveEmptyEntries(options) ? Array.Empty<StringBase>() : [this];

            ArrayPool<SplitRange> rangePool = ArrayPool<SplitRange>.Shared;
            SplitRange[]? rangeBuffer = null;
            try
            {
                nuint splitCount = GetSplitCount(separator, unchecked((nuint)separatorLength), rangePool, out rangeBuffer);
                if (ShouldRemoveEmptyEntries(options))
                    return SplitCore_RemoveEmptyEntries(rangeBuffer, splitCount);
                return SplitCore(rangeBuffer, splitCount);
            }
            finally
            {
                if (rangeBuffer is not null)
                    rangePool.Return(rangeBuffer);
            }
        }

        private StringBase[] SplitCore(SplitRange[]? rangeBuffer, nuint count)
        {
            if (count < 2 || rangeBuffer is null)
                return [this];

            StringBase[] result = new StringBase[count];
            for (nuint i = 0; i < count; i++)
            {
                SplitRange range = rangeBuffer[i];
                result[i] = SubstringCore(range.StartIndex, range.Count);
            }
            return result;
        }

        private StringBase[] SplitCore_RemoveEmptyEntries(SplitRange[]? rangeBuffer, nuint count)
        {
            if (count < 2 || rangeBuffer is null)
            {
                if (Length <= 0)
                    return Array.Empty<StringBase>();
                return [this];
            }
            ArrayPool<StringBase?> pool = ArrayPool<StringBase?>.Shared;
            StringBase?[] buffer = pool.Rent(count);
            nuint resultLength = 0;
            try
            {
                for (nuint i = 0; i < count; i++)
                {
                    SplitRange range = rangeBuffer[i];
                    StringBase slicedString = SubstringCore(range.StartIndex, range.Count);
                    if (IsNullOrEmpty(slicedString))
                    {
                        buffer[i] = null;
                        continue;
                    }
                    buffer[i] = slicedString;
                    resultLength++;
                }
                if (count == 0)
                    return Array.Empty<StringBase>();
                StringBase[] result = new StringBase[resultLength];
                for (nuint i = 0, j = 0; i < count && j < resultLength; i++)
                {
                    StringBase? item = buffer[i];
                    if (item is null)
                        continue;
                    result[j++] = item;
                }
                return result;
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        protected virtual nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            IEnumerable<nuint> indexes = this.WithNativeIndex().WhereEqualsTo(separator).Select(static item => item.Index);
            LazyTinyRef<PooledList<SplitRange>> listLazy = new(() => new PooledList<SplitRange>(pool));
            nuint start = 0, counter = 1;
            foreach (nuint index in indexes)
            {
                listLazy.Value.Add(new SplitRange(start, index - start));
                start = index + 1;
                counter++;
            }
            PooledList<SplitRange>? list = listLazy.GetValueDirectly();
            if (list is null)
            {
                rangeBuffer = null;
                return 1;
            }
            list.Add(new SplitRange(start, unchecked((nuint)Length) - start));
            rangeBuffer = list.DestructAndReturnBuffer();
            return counter;
        }

        protected virtual unsafe nuint GetSplitCount(string separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (separatorLength == 1)
                return GetSplitCount(separator[0], pool, out rangeBuffer);

            fixed (char* ptr = separator)
                return GetSplitCount_Fallback(ptr, separatorLength, pool, out rangeBuffer);
        }

        protected virtual unsafe nuint GetSplitCount(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            if (separatorLength == 1)
                return GetSplitCount(separator.GetCharAt(0), pool, out rangeBuffer);

            if (separator is Utf16String utf16)
            {
                fixed (char* ptr = utf16.GetInternalRepresentation())
                    return GetSplitCount_Fallback(ptr, separatorLength, pool, out rangeBuffer);
            }

            return GetSplitCount_Fallback(separator, separatorLength, pool, out rangeBuffer);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool ShouldRemoveEmptyEntries(StringSplitOptions options)
            => (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;

        private unsafe nuint GetSplitCount_Fallback(char* separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            ArrayPool<char> bufferPool = ArrayPool<char>.Shared;
            nuint length = unchecked((nuint)Length);
            char[] buffer = bufferPool.Rent(length);
            try
            {
                fixed (char* temp = buffer)
                {
                    CopyToCore(temp, 0, length);
                    return GetSplitCount_Fallback(temp, length, separator, separatorLength, pool, out rangeBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }

        private unsafe nuint GetSplitCount_Fallback(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            ArrayPool<char> bufferPool = ArrayPool<char>.Shared;
            char[] buffer = bufferPool.Rent(separatorLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    separator.CopyToCore(temp, 0, separatorLength);
                    return GetSplitCount_Fallback(temp, separatorLength, pool, out rangeBuffer);
                }
            }
            finally
            {
                bufferPool.Return(buffer);
            }
        }

        private static unsafe nuint GetSplitCount_Fallback(char* source, nuint sourceLength, char* separator, nuint separatorLength, 
            ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            nuint result = 1;
            using PooledList<SplitRange> list = new PooledList<SplitRange>(pool);

            char* previous = source, iterator = source, ptrEnd = source + sourceLength;
            while ((iterator = InternalSequenceHelper.PointerIndexOf(iterator, ptrEnd, separator, separatorLength)) != null)
            {
                unchecked
                {
                    list.Add(new SplitRange((nuint)(previous - source), (nuint)(iterator - previous)));
                }
                result++;
                previous = iterator;
                iterator += separatorLength;
            }
            unchecked
            {
                list.Add(new SplitRange((nuint)(previous - source), (nuint)(ptrEnd - previous)));
            }

            rangeBuffer = list.DestructAndReturnBuffer();
            return result;
        }
    }
}
