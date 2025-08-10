#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common.Text
{
    partial class Utf8String : IHolder<ReadOnlyMemory<byte>>
    {
        ReadOnlyMemory<byte> IHolder<ReadOnlyMemory<byte>>.Value
        {
            get
            {
                byte[] source = _value;
                int length = source.Length;
                if (length <= 0)
                    return ReadOnlyMemory<byte>.Empty;
                return new ReadOnlyMemory<byte>(source, 0, length - 1);
            }
        }
    }
}
#endif
