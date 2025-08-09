#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common
{
    public interface IMemoryReference<T> where T : unmanaged
    {
        ReadOnlyMemory<T> AsMemory();
    }
}
#endif
