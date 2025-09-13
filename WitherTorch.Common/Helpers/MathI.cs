using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class MathI
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(double val) => (int)val + MathHelper.BooleanToInt32(val > (int)val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(float val) => (int)val + MathHelper.BooleanToInt32(val > (int)val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(double val) => (int)val - MathHelper.BooleanToInt32(val < (int)val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(float val) => (int)val - MathHelper.BooleanToInt32(val < (int)val);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(double val) => (int)val;

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(float val) => (int)val;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(double val)
        {
            int flooredVal = (int)val;

            if (val >= 0.0)
                return flooredVal + MathHelper.BooleanToInt32(val - flooredVal >= 0.5);

            return flooredVal - MathHelper.BooleanToInt32(val - flooredVal <= -0.5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(float val)
        {
            int flooredVal = (int)val;

            if (val >= 0.0f)
                return flooredVal + MathHelper.BooleanToInt32(val - flooredVal >= 0.5f);

            return flooredVal - MathHelper.BooleanToInt32(val - flooredVal <= -0.5f);
        }
    }
}
