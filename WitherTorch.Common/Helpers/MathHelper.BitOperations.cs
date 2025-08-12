#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

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
        private static int LeadingZeroCountCore(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => LeadingZeroCountCore((uint)value),
                sizeof(ulong) => LeadingZeroCountCore((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => LeadingZeroCountCore((uint)value),
                    sizeof(ulong) => LeadingZeroCountCore((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(uint value)
        {
            if (value == 0)
                return 32;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return UnsafeHelper.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_0111_1100_1011_0101_0011_0001u
                ref TrailingZeroCountDeBruijn[0],
                // uint|long -> IntPtr cast on 32-bit platforms does expensive overflow checks not needed here
                (nuint)(int)(((value & (uint)-(int)value) * 0x077CB531u) >> 27)); // Multi-cast mitigates redundant conv.u8
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(ulong value)
        {
            uint lo = unchecked((uint)value);

            if (lo == 0)
                return 32 + TrailingZeroCount((uint)(value >> 32));

            return TrailingZeroCount(lo);
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => TrailingZeroCountCore((uint)value),
                sizeof(ulong) => TrailingZeroCountCore((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => TrailingZeroCountCore((uint)value),
                    sizeof(ulong) => TrailingZeroCountCore((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };
    }
}
#endif
