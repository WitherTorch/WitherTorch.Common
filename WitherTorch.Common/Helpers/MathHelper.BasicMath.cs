using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CheckedAdd(ulong a, ulong b)
        {
            ulong result = unchecked(a + b);
            if (result < a) //Overflowed
                return ulong.MaxValue;
            return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CheckedAdd(uint a, uint b)
        {
            uint result = unchecked(a + b);
            if (result < a) //Overflowed
                return uint.MaxValue;
            return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort CheckedAdd(ushort a, ushort b)
        {
            ushort result = unchecked((ushort)(a + b));
            if (result < a) //Overflowed
                return ushort.MaxValue;
            return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte CheckedAdd(byte a, byte b)
        {
            byte result = unchecked((byte)(a + b));
            if (result < a) //Overflowed
                return byte.MaxValue;
            return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CheckedSubstract(ulong a, ulong b)
        {
            if (a <= b)
                return ulong.MinValue;
            return unchecked(a - b);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CheckedSubstract(uint a, uint b)
        {
            if (a <= b)
                return uint.MinValue;
            return unchecked(a - b);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort CheckedSubstract(ushort a, ushort b)
        {
            if (a <= b)
                return ushort.MinValue;
            return unchecked((ushort)(a - b));
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte CheckedSubstract(byte a, byte b)
        {
            if (a <= b)
                return byte.MinValue;
            return unchecked((byte)(a - b));
        }
    }
}
