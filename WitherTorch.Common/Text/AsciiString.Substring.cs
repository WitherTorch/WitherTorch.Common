using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class AsciiString
    {
        protected internal override unsafe StringBase SubstringCore(nuint startIndex, nuint count)
        {
            byte[] buffer = new byte[count + 1];
            fixed (byte* ptrSource = _value, ptrBuffer = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource + startIndex, count * sizeof(byte));
            return new AsciiString(buffer);
        }

        protected override unsafe StringBase RemoveCore(nuint startIndex, nuint count)
        {
            nuint length = unchecked((nuint)_length);
            nuint endIndex = startIndex + count;
            if (endIndex >= length)
                return SubstringCore(0, startIndex);

            byte[] buffer = new byte[length - count + 1];
            fixed (byte* ptrSource = _value, ptrBuffer = buffer)
            {
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptrSource, startIndex * sizeof(byte));
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptrSource + endIndex, (length - endIndex) * sizeof(byte));
            }
            return new AsciiString(buffer);
        }
    }
}
