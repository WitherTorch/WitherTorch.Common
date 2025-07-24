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
            ArrayPool<StringSlice> slicePool = ArrayPool<StringSlice>.Shared;
            StringSlice[]? sliceBuffer = null;
            try
            {
                nuint splitCount = GetSplitCount(separator, slicePool, out sliceBuffer);
                if (ShouldRemoveEmptyEntries(options))
                    return SplitCore_RemoveEmptyEntries(sliceBuffer, splitCount);
                return SplitCore(sliceBuffer, splitCount);
            }
            finally
            {
                if (sliceBuffer is not null)
                    slicePool.Return(sliceBuffer);
            }
        }

        public StringBase[] Split(string separator) => Split(separator, StringSplitOptions.None);

        public StringBase[] Split(string separator, StringSplitOptions options)
        {
            ArrayPool<StringSlice> slicePool = ArrayPool<StringSlice>.Shared;
            StringSlice[]? sliceBuffer = null;
            try
            {
                nuint splitCount = GetSplitCount(separator, slicePool, out sliceBuffer);
                if (ShouldRemoveEmptyEntries(options))
                    return SplitCore_RemoveEmptyEntries(sliceBuffer, splitCount);
                return SplitCore(sliceBuffer, splitCount);
            }
            finally
            {
                if (sliceBuffer is not null)
                    slicePool.Return(sliceBuffer);
            }
        }

        private StringBase[] SplitCore(StringSlice[]? sliceBuffer, nuint count)
        {
            if (count < 2 || sliceBuffer is null)
                return [this];

            StringBase[] result = new StringBase[count];
            for (nuint i = 0; i < count; i++)
                result[i] = sliceBuffer[i].ToStringBase();
            return result;
        }

        private StringBase[] SplitCore_RemoveEmptyEntries(StringSlice[]? sliceBuffer, nuint count)
        {
            if (count < 2 || sliceBuffer is null)
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
                    StringBase slicedString = sliceBuffer[i].ToStringBase();
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

        protected abstract nuint GetSplitCount(char separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer);

        protected abstract nuint GetSplitCount(string separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer);

        protected virtual nuint GetSplitCount(StringBase separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
            => GetSplitCount(separator.ToString(), pool, out sliceBuffer);

        [Inline(InlineBehavior.Remove)]
        private static bool ShouldRemoveEmptyEntries(StringSplitOptions options)
            => (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;
    }
}
