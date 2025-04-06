using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class NativeObjectExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static T Clone<T>(this T _this) where T : NativeObject, new() => NativeObject.CopyReference(_this) ?? _this;
    }
}
