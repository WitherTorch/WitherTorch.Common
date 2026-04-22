using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

#pragma warning disable IDE0130
namespace System
#pragma warning restore IDE0130
{
    public static class MathI
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(float value)
        {
            int result = Truncate(value);
            return result + MathHelper.BooleanToInt32(value > result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(double value)
        {
            int result = Truncate(value);
            return result + MathHelper.BooleanToInt32(value > result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(float value)
        {
            int result = Truncate(value);
            return result - MathHelper.BooleanToInt32(value < result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(double value)
        {
            int result = Truncate(value);
            return result - MathHelper.BooleanToInt32(value < result);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(float a) => (int)MathF.Round(a);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(float value, [InlineParameter] MidpointRounding mode)
            => mode switch
            {
                MidpointRounding.AwayFromZero => Truncate(value + MathF.CopySign(0.49999997f, value)),
                MidpointRounding.ToEven => Round(value),
#if NET8_0_OR_GREATER
                MidpointRounding.ToZero => Truncate(value),
                MidpointRounding.ToNegativeInfinity => Floor(value),
                MidpointRounding.ToPositiveInfinity => Ceiling(value),
#endif
                _ => throw new ArgumentOutOfRangeException(nameof(mode)),
            };

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(double val) => unchecked((int)Math.Round(val));

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Round(double val, MidpointRounding mode) => unchecked((int)Math.Round(val, mode));

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Truncate(double val) => (int)val;

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Truncate(float val) => (int)val;
    }
}
