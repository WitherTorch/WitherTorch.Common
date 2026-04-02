using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected internal override unsafe StringWrapper SubstringCore(nuint startIndex, nuint count)
        {
            unchecked
            {
                fixed (char* ptr = _value)
                    return Create(ptr, (int)startIndex, (int)count);
            }
        }

        protected override unsafe StringWrapper RemoveCore(nuint startIndex, nuint count)
        {
            string source = _value;
            nuint length = unchecked((nuint)source.Length);
            nuint endIndex = startIndex + count;
            fixed (char* ptr = source)
            {
                if (endIndex >= length)
                    return Create(ptr, 0, unchecked((int)startIndex));

                NativeMemoryPool pool = NativeMemoryPool.Shared;
                nuint newLength = length - count;
                var buffer = pool.Rent<char>(newLength);
                try
                {
                    char* ptrBuffer = buffer.NativePointer;
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptr, startIndex * sizeof(char));
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptr + endIndex, (length - endIndex) * sizeof(char));
                    return Create(ptrBuffer, 0, unchecked((int)newLength));
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }
    }
}
