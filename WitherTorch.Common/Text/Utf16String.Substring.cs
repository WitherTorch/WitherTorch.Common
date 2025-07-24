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

                if (!WTCommon.AllowLatin1StringCompression||
                    SequenceHelper.ContainsGreaterThan(ptr, startIndex, InternalStringHelper.Latin1StringLimit) ||
                    SequenceHelper.ContainsGreaterThan(ptr + startIndex, length - endIndex, InternalStringHelper.Latin1StringLimit))
                {
                    string buffer = StringHelper.AllocateRawString(unchecked((int)(length - count)));
                    fixed (char* ptrBuffer = buffer)
                    {
                        UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptr, unchecked((uint)startIndex * sizeof(char)));
                        UnsafeHelper.CopyBlockUnaligned(ptrBuffer + startIndex, ptr + endIndex, unchecked((uint)(length - endIndex) * sizeof(char)));
                    }
                    return new Utf16String(buffer);
                }
                else
                {
                    byte[] buffer = new byte[length - count + 1];
                    fixed (byte* ptrBuffer = buffer)
                    {
                        InternalStringHelper.NarrowAndCopyTo(ptr, startIndex, ptrBuffer);
                        InternalStringHelper.NarrowAndCopyTo(ptr + endIndex, length - endIndex, ptrBuffer + startIndex);
                    }
                    return new Latin1String(buffer);
                }
            }
        }
    }
}
