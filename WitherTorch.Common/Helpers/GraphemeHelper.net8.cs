#if NET8_0_OR_GREATER
using System;
using System.Globalization;

using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class GraphemeHelper
    {
        private static unsafe partial int[] GetGraphemeIndicesCore(char* ptr, int length)
        {
            using PooledList<int> list = new PooledList<int>(length);

            ReadOnlySpan<char> span = new ReadOnlySpan<char>(ptr, length);
            do
            {
                list.Add(length - span.Length);
                span = span.Slice(StringInfo.GetNextTextElementLength(span));
            } while (span.IsEmpty);

            return list.ToArray();
        }
    }
}
#endif