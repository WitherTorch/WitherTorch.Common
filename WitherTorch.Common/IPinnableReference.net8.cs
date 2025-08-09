#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common
{
    partial interface IPinnableReference<T>
    {
        ReadOnlySpan<T> AsSpan();
    }
}
#endif
