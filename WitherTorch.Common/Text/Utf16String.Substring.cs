using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected internal override unsafe StringBase SubstringCore(nuint startIndex, nuint count)
        {
            unchecked
            {
                fixed (char* ptr = _value)
                    return Create(ptr, (int)startIndex, (int)count);
            }
        }

        protected override unsafe StringBase RemoveCore(nuint startIndex, nuint count)
        {
            string source = _value;
            nuint length = unchecked((nuint)source.Length);
            nuint endIndex = startIndex + count;
            fixed (char* ptr = source)
            {
                if (endIndex >= length)
                    return Create(ptr, 0, unchecked((int)startIndex));

                ArrayPool<char> pool = ArrayPool<char>.Shared;
                nuint newLength = length - count;
                char[] buffer = pool.Rent(newLength);
                try
                {
                    fixed (char* ptrBuffer = buffer)
                    {
                        UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptr, unchecked((uint)startIndex * sizeof(char)));
                        UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptr + endIndex, unchecked((uint)(length - endIndex) * sizeof(char)));
                        return Create(ptrBuffer, 0, unchecked((int)newLength));
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }
    }
}
