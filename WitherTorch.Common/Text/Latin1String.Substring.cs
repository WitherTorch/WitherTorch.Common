using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Latin1String
    {
        protected internal override unsafe StringBase SubstringCore(nuint startIndex, nuint count)
        {
            byte[] buffer = new byte[count + 1];
            fixed (byte* ptrSource = _value, ptrBuffer = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource + startIndex, unchecked((uint)count * sizeof(byte)));
            return new Latin1String(buffer);
        }

        protected override unsafe StringBase RemoveCore(nuint startIndex, nuint count)
        {
            byte[] source = _value;
            byte[] buffer;
            nuint length = unchecked((nuint)_length);
            nuint endIndex = startIndex + count;
            if (endIndex >= length)
            {
                buffer = new byte[startIndex + 1];
                fixed (byte* ptrSource = source, ptrBuffer = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource, unchecked((uint)startIndex * sizeof(byte)));
            }
            else
            {
                buffer = new byte[length - count + 1];
                fixed (byte* ptrSource = source, ptrBuffer = buffer)
                {
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource, unchecked((uint)startIndex * sizeof(byte)));
                    UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptrSource + endIndex, unchecked((uint)(length - endIndex) * sizeof(byte)));
                }
            }
            return new Latin1String(buffer);
        }
    }
}
