using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class MemoryExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static StringBase ToStringBase(this in ReadOnlySpan<char> _this) 
            => StringBase.Create(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe StringBase ToStringBase(this in ReadOnlySpan<char> _this, StringCreateOptions options) 
            => StringBase.Create(_this, options);

        [Inline(InlineBehavior.Keep, export: true)]
        public static StringBase ToStringBase(this in Span<char> _this)
            => StringBase.Create(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe StringBase ToStringBase(this in Span<char> _this, StringCreateOptions options)
            => StringBase.Create(_this, options);
    }
}
