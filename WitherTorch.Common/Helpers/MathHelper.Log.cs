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

        private static readonly byte[] Log2DeBruijn32 = new byte[sizeof(uint) * 8]
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

            // value    lzcnt   actual  expected
            // ..0001   31      31-31    0
            // ..0010   30      31-30    1
            // 0010..    2      31-2    29
            // 0100..    1      31-1    30
            // 1000..    0      31-0    31
            if (Lzcnt.IsSupported)
                return 31 ^ unchecked((int)Lzcnt.LeadingZeroCount(value));

            if (X86Base.IsSupported)
                return unchecked((int)X86Base.BitScanReverse(value));

            return Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe int Log2Core(ulong value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            if (Lzcnt.X64.IsSupported)
                return 63 ^ unchecked((int)Lzcnt.X64.LeadingZeroCount(value));

            if (Lzcnt.IsSupported)
                return unchecked((int)((31 ^ Lzcnt.LeadingZeroCount((uint)(value >> 32))) + (31 ^ Lzcnt.LeadingZeroCount((uint)value))));

            if (X86Base.X64.IsSupported)
                return unchecked((int)X86Base.X64.BitScanReverse(value));

            if (X86Base.IsSupported)
            {
                uint hi = unchecked((uint)(value >> 32));
                if (hi == 0)
                    return unchecked((int)X86Base.BitScanReverse((uint)value));
                return 32 + unchecked((int)X86Base.BitScanReverse(hi));
            }

            return Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe int Log2Core(nuint value)
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

        private static unsafe int Log2SoftwareFallback(uint value)
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

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return UnsafeHelper.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_1100_0100_1010_1100_1101_1101u
                ref Log2DeBruijn32[0],
                // uint|long -> IntPtr cast on 32-bit platforms does expensive overflow checks not needed here
                (uint)(int)((value * 0x07C4ACDDu) >> 27));
        }

        private static unsafe int Log2SoftwareFallback(ulong value)
        {
            value |= 1;

            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;
            value |= value >> 32;

            return UnsafeHelper.AddByteOffset(
                ref DeBruijn64[0],
                (uint)(int)((value * 0x03F79D71B4CB0A89uL) >> 58));
        }
#endif
    }
}
