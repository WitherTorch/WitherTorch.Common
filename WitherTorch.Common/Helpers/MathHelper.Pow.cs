using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    // Source code from https://gist.github.com/orlp/3551590
    public static partial class MathHelper
    {

        private static readonly byte[] highest_bit_set = [
        0, 1, 2, 2, 3, 3, 3, 3,
        4, 4, 4, 4, 4, 4, 4, 4,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 6,
        6, 6, 6, 6, 6, 6, 6, 255, // anything past 63 is a guaranteed overflow with _base > 1
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        255, 255, 255, 255, 255, 255, 255, 255,
        ];

        [Inline(InlineBehavior.Remove)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long PowCore(long _base, byte exp)
        {
            long result = 1;

            switch (highest_bit_set[exp])
            {
                case 255: // we use 255 as an overflow marker and return 0 on overflow/underflow
                    if (_base == 1)
                    {
                        return 1;
                    }

                    if (_base == -1)
                    {
                        return 1 - 2 * (exp & 1);
                    }

                    return 0;
                case 6:
                    if ((exp & 1) != 0) result *= _base;
                    exp >>= 1;
                    _base *= _base;
                    goto case 5;
                case 5:
                    if ((exp & 1) != 0) result *= _base;
                    exp >>= 1;
                    _base *= _base;
                    goto case 4;
                case 4:
                    if ((exp & 1) != 0) result *= _base;
                    exp >>= 1;
                    _base *= _base;
                    goto case 3;
                case 3:
                    if ((exp & 1) != 0) result *= _base;
                    exp >>= 1;
                    _base *= _base;
                    goto case 2;
                case 2:
                    if ((exp & 1) != 0) result *= _base;
                    exp >>= 1;
                    _base *= _base;
                    goto case 1;
                case 1:
                    if ((exp & 1) != 0) result *= _base;
                    goto default;
                default:
                    return result;
            }
        }
    }
}
