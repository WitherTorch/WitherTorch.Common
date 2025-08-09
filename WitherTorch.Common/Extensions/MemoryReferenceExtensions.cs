#if NET8_0_OR_GREATER
using System;

namespace WitherTorch.Common.Extensions
{
    public static class MemoryReferenceExtensions
    {
        public static ReadOnlySpan<T> AsSpan<T>(this IMemoryReference<T> _this) where T : unmanaged
        {
            if (_this is IPinnableReference<T> pinnable)
                return pinnable.AsSpan();
            return _this.AsMemory().Span;
        }
    }
}
#endif
