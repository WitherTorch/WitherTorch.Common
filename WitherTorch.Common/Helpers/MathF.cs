#if NET472_OR_GREATER
using InlineMethod;

#pragma warning disable IDE0130
namespace System
#pragma warning restore IDE0130
{
    public static class MathF
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static float Floor(float val) => unchecked((float)Math.Floor(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Floor(double val) => unchecked((float)Math.Floor(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Ceiling(float val) => unchecked((float)Math.Ceiling(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Ceiling(double val) => unchecked((float)Math.Ceiling(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Round(float val) => unchecked((float)Math.Round(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Round(double val) => unchecked((float)Math.Round(val));
    }
}
#endif
