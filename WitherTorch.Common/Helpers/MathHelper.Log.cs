using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Intrinsics.X86;

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
            return MathI.Truncate(Math.Log10(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log10(uint value)
        {
            int digits = _guessLog10[Log2(value)];
            return digits + BooleanToInt32(value >= _powerOf10Sequence[digits]);
        }

#if NET472_OR_GREATER
        [Inline(InlineBehavior.Remove)]
        private static int Log2Core(uint value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            // value    lzcnt   actual  expected
            // ..0001   31      31-31    0
            // ..0010   30      31-30    1
            // 0010..    2      31-2    29
            // 0100..    1      31-1    30
            // 1000..    0      31-0    31
            if (_isLzcntSupported)
                return 31 ^ unchecked((int)Lzcnt.LeadingZeroCount(value));

            if (_isX86BaseSupported)
                return unchecked((int)X86Base.BitScanReverse(value));

            return Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int Log2Core(ulong value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            if (_isLzcnt_X64Supported)
                return 63 ^ unchecked((int)Lzcnt.X64.LeadingZeroCount(value));

            if (_isX86Base_X64Supported)
                return unchecked((int)X86Base.X64.BitScanReverse(value));

            uint hi = (uint)(value >> 32);

            if (hi == 0)
                return Log2((uint)value);

            return 32 + Log2(hi);
        }

        [Inline(InlineBehavior.Remove)]
        private static int Log2Core(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => Log2((uint)value),
                sizeof(ulong) => Log2((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => Log2((uint)value),
                    sizeof(ulong) => Log2((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        internal static int Log2SoftwareFallback(uint value)
        {
            if (_isSystemMemoryExists)
                return DeBruijn_StoreAsSpan.Log2(value);
            else
                return DeBruijn_StoreAsArray.Log2(value);
        }
#endif
    }
}
