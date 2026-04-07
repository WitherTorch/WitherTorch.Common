using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using InlineMethod;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class ToStringWrapperExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        [return: NotNullIfNotNull(nameof(_this))]
        [OverloadResolutionPriority(999)]
        public static StringWrapper? ToStringWrapper(this string? _this) => _this is null ? null : StringWrapper.CreateUtf16String(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this string? _this, StringCreateOptions options) => _this is null ? null : StringWrapper.Create(_this, options);

        [Inline(InlineBehavior.Keep, export: true)]
        [return: NotNullIfNotNull(nameof(_this))]
        [OverloadResolutionPriority(999)]
        public static StringWrapper? ToStringWrapper(this StringBuilder? _this) => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString());

        [Inline(InlineBehavior.Keep, export: true)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this StringBuilder? _this, StringCreateOptions options) => _this is null ? null : StringWrapper.Create(_this.ToString(), options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(0)]
        public static StringWrapper? ToStringWrapper<T>(this T? _this)
            => _this switch
            {
                string str => StringWrapper.CreateUtf16String(str),
                StringWrapper str => str,
                null => null,
                _ => _this?.ToString()?.ToStringWrapper()
            };
    }
}
