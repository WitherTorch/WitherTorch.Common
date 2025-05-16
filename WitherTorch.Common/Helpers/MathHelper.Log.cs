using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Original code from https://stackoverflow.com/questions/25892665/performance-of-log10-function-returning-an-int

        private static readonly byte[] _guessLog10 = new byte[33]
        {
            0, 0, 0, 0, 1, 1, 1, 2, 2, 2,
            3, 3, 3, 3, 4, 4, 4, 5, 5, 5,
            6, 6, 6, 6, 7, 7, 7, 8, 8, 8,
            9, 9, 9
        };

        private static readonly uint[] _powerOf10Sequence =
        [
            1, 10, 100, 1000, 10000, 100000,
            1000000, 10000000, 100000000, 1000000000,
        ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log10(ulong value)
        {
            if (value <= uint.MaxValue)
                return Log10((uint)value);
            return MathI.FloorPositive(Math.Log10(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Log10(uint value)
        {
            int digits = _guessLog10[Log2(value)];
            return digits + BooleanToInt32(value >= _powerOf10Sequence[digits]);
        }

#if NET472_OR_GREATER
        // Source code from https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs

        private static readonly byte[] Log2DeBruijn = new byte[sizeof(int) * 8]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        [Inline(InlineBehavior.Remove)]
        private static unsafe int Log2Core(uint value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            // Fill trailing zeros with ones, eg 00010010 becomes 00011111
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            return Log2DeBruijn[(value * 0x07C4ACDDu) >> 27];
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe int Log2Core(ulong value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            uint hi = unchecked((uint)(value >> 32));

            if (hi == 0)
                return Log2((uint)value);

            return 32 + Log2(hi);
        }
#endif
    }
}
