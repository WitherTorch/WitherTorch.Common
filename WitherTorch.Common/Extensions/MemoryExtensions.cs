using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class MemoryExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static StringWrapper ToStringWrapper(this in ReadOnlySpan<char> _this) 
            => StringWrapper.Create(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        public static StringWrapper ToStringWrapper(this in ReadOnlySpan<char> _this, StringCreateOptions options) 
            => StringWrapper.Create(_this, options);

        [Inline(InlineBehavior.Keep, export: true)]
        public static StringWrapper ToStringWrapper(this in Span<char> _this)
            => StringWrapper.Create(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        public static StringWrapper ToStringWrapper(this in Span<char> _this, StringCreateOptions options)
            => StringWrapper.Create(_this, options);
    }
}
