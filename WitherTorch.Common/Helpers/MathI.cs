using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class MathI
    {
        private const float CeilConstantSingle = 0.999999f;
        private const double CeilConstantDouble = 0.999999999999999d;
        private const int NumberLimit = 32768;
        private const float NumberLimitSingle = NumberLimit;
        private const double NumberLimitDouble = NumberLimit;

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Ceiling(double val)
        {
            if (val > NumberLimitDouble)
                return FloorPositive(val + CeilConstantDouble);
            return NumberLimit - FloorPositive(NumberLimitDouble - val);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Ceiling(float val)
        {
            if (val > NumberLimitSingle)
                return FloorPositive(val + CeilConstantSingle);
            return NumberLimit - FloorPositive(NumberLimitSingle - val);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Floor(double val)
        {
            if (val < NumberLimitDouble)
            {
                if (val < 0.0)
                    return FloorPositive(val) - 1;
                return FloorPositive(val + NumberLimitDouble) - NumberLimit;
            }
            return FloorPositive(val);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Floor(float val)
        {
            if (val < NumberLimitSingle)
            {
                if (val < 0.0f)
                    return FloorPositive(val) - 1;
                return FloorPositive(val + NumberLimitSingle) - NumberLimit;
            }
            return FloorPositive(val);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(double val)
        {
            return (int)val;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int FloorPositive(float val)
        {
            return (int)val;
        }

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
