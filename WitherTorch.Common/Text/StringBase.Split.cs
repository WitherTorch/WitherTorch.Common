using System;

using InlineMethod;

using WitherTorch.Common.Buffers;

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

        protected abstract nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer);

        protected abstract nuint GetSplitCount(string separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer);

        protected virtual nuint GetSplitCount(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
            => GetSplitCount(separator.ToString(), separatorLength, pool, out rangeBuffer);

        [Inline(InlineBehavior.Remove)]
        private static bool ShouldRemoveEmptyEntries(StringSplitOptions options)
            => (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;
    }
}
