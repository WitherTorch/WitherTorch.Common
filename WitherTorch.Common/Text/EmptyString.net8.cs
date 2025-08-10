#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common.Text
{
    partial class EmptyString : IHolder<ReadOnlyMemory<byte>>, IHolder<ReadOnlyMemory<char>>
    {
        ReadOnlyMemory<byte> IHolder<ReadOnlyMemory<byte>>.Value => ReadOnlyMemory<byte>.Empty;

        ReadOnlyMemory<char> IHolder<ReadOnlyMemory<char>>.Value => ReadOnlyMemory<char>.Empty;
    }
}
#endif
