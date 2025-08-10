#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common.Text
{
    partial class Utf16String : IHolder<ReadOnlyMemory<char>>
    {
        ReadOnlyMemory<char> IHolder<ReadOnlyMemory<char>>.Value => _value.AsMemory();
    }
}
#endif
