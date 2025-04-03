using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Source code from https://stackoverflow.com/questions/25892665/performance-of-log10-function-returning-an-int

        private static readonly byte[] guess =
        [
            0, 0, 0, 0, 1, 1, 1, 2, 2, 2,
            3, 3, 3, 3, 4, 4, 4, 5, 5, 5,
            6, 6, 6, 6, 7, 7, 7, 8, 8, 8,
            9, 9, 9
        ];

        private static readonly uint[] tenToThe =
        [
            1, 10, 100, 1000, 10000, 100000,
            1000000, 10000000, 100000000, 1000000000,
        ];

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Log2(ulong value)
        {
            if (value < 2UL)
                return 0U;
            return unchecked((uint)(32 - LeadingZeroCount(value)));
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Log2(uint value)
        {
            if (value < 2U)
                return 0U;
            return unchecked((uint)(32 - LeadingZeroCount(value)));
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Log10(ulong value)
        {
            if (value < 10UL)
                return 0U;
            if (value > uint.MaxValue)
                return unchecked((uint)Math.Log(value, 10.0));
            unchecked
            {
                uint intValue = (uint)value;
                uint digits = guess[Log2(intValue)];
                return digits + (intValue >= tenToThe[digits] ? 1U : 0U);
            }
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Log10(uint value)
        {
            if (value < 10U)
                return 0U;
            unchecked
            {
                uint digits = guess[unchecked((uint)(32 - LeadingZeroCount(value)))];
                return digits + (value >= tenToThe[digits] ? 1U : 0U);
            }
        }
    }
}
