using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class MathI
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static int Ceiling(double val) => (int)val + MathHelper.BooleanToInt32(val > (int)val);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Ceiling(float val) => (int)val + MathHelper.BooleanToInt32(val > (int)val);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Floor(double val) => (int)val - MathHelper.BooleanToInt32(val < (int)val);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Floor(float val) => (int)val - MathHelper.BooleanToInt32(val < (int)val);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(double val) => (int)val;

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(float val) => (int)val;

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(double val)
        {
            if (val < 0.0f)
                return FloorPositive(val - 0.5);
            return FloorPositive(val + 0.5);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(float val)
        {
            if (val < 0.0f)
                return FloorPositive(val - 0.5f);
            return FloorPositive(val + 0.5f);
        }
    }
}
