using System;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Helpers
{
    public static partial class GraphemeHelper
    {
        public static unsafe int[] GetGraphemeIndices(string str)
        {
            int length = str.Length;
            if (length <= 0)
                return Array.Empty<int>();
            fixed (char* ptr = str)
                return GetGraphemeIndicesCore(ptr, length);
        }

        public static unsafe int[] GetGraphemeIndices(StringBase str)
        {
            int length = str.Length;
            if (length <= 0)
                return Array.Empty<int>();
            if (str.StringType == StringType.Utf16 && str is IPinnableReference<char> reference)
            {
                fixed (char* ptr = reference)
                    return GetGraphemeIndicesCore(ptr, length);
            }

            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            try
            {
                fixed (char* temp = buffer)
                {
                    str.CopyToCore(temp, 0, (nuint)length);
                    return GetGraphemeIndicesCore(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        public static unsafe int[] GetGraphemeIndices(char* ptr, int length)
        {
            if (length <= 0)
                return Array.Empty<int>();
            return GetGraphemeIndicesCore(ptr, length);
        }

        private static unsafe partial int[] GetGraphemeIndicesCore(char* ptr, int length);
    }
}
