#if NET472_OR_GREATER
using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Source code from https://stackoverflow.com/questions/10439242/count-leading-zeroes-in-an-int32
        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(uint value)
        {
            unchecked
            {
                //do the smearing
                value |= value >> 1;
                value |= value >> 2;
                value |= value >> 4;
                value |= value >> 8;
                value |= value >> 16;
                //count the ones
                value -= value >> 1 & 0x55555555;
                value = (value >> 2 & 0x33333333) + (value & 0x33333333);
                value = (value >> 4) + value & 0x0f0f0f0f;
                value += value >> 8;
                value += value >> 16;
                const int numIntBits = sizeof(int) * 8; //compile time constant
                return (int)(numIntBits - (value & 0x0000003f)); //subtract # of 1s from 32
            }
        }

        // Source code from https://andrewlock.net/counting-the-leading-zeroes-in-a-binary-number/
        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(ulong value)
        {
            unchecked
            {
                // Do the smearing which turns (for example)
                // this: 0000 0101 0011
                // into: 0000 0111 1111
                value |= value >> 1;
                value |= value >> 2;
                value |= value >> 4;
                value |= value >> 8;
                value |= value >> 16;
                value |= value >> 32;

                // Count the ones
                value -= value >> 1 & 0x5555555555555555;
                value = (value >> 2 & 0x3333333333333333) + (value & 0x3333333333333333);
                value = (value >> 4) + value & 0x0f0f0f0f0f0f0f0f;
                value += value >> 8;
                value += value >> 16;
                value += value >> 32;

                const int numLongBits = sizeof(long) * 8; // compile time constant
                return (int)(numLongBits - (uint)(value & 0x0000007f)); // subtract # of 1s from 64
            }
        }

        // Source code from https://stackoverflow.com/questions/45221914/how-do-you-efficiently-count-the-trailing-zero-bits-in-a-number
        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(uint value)
        {
            unchecked
            {
                const int numIntBits = sizeof(int) * 8; //compile time constant

                if (value == 0)
                    return numIntBits;
                uint n = 1;
                if ((value & 0x0000FFFF) == 0)
                {
                    n += 16;
                    value >>= 16;
                }
                if ((value & 0x000000FF) == 0)
                {
                    n += 8;
                    value >>= 8;
                }
                if ((value & 0x0000000F) == 0)
                {
                    n += 4;
                    value >>= 4;
                }
                if ((value & 0x00000003) == 0)
                {
                    n += 2;
                    value >>= 2;
                }
                return (int)(n - value & 1U);
            }
        }
    }
}
#endif
