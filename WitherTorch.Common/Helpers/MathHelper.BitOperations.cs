#if NET472_OR_GREATER
using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Source code from https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs

        private static readonly byte[] TrailingZeroCountDeBruijn = new byte[sizeof(uint) * 8]
        {
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09
        };

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(uint value)
            => value == 0 ? 32 : 31 ^ Log2Core(value);

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(ulong value)
        {
            uint hi = (uint)(value >> 32);

            if (hi == 0)
            {
                return 32 + LeadingZeroCount((uint)value);
            }

            return LeadingZeroCount(hi);
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(uint value)
        {
            if (value == 0)
                return 32;
            return TrailingZeroCountDeBruijn[((value & (uint)-(int)value) * 0x077CB531u) >> 27];
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(ulong value)
        {
            uint lo = unchecked((uint)value);

            if (lo == 0)
                return 32 + TrailingZeroCount((uint)(value >> 32));

            return TrailingZeroCount(lo);
        }
    }
}
#endif
