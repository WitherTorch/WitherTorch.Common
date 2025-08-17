#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Intrinsics.X86;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Source code from https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs

        private static readonly byte[] TrailingZeroCountDeBruijn32 = new byte[sizeof(uint) * 8]
        {
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09
        };

        // Table from https://github.com/omgtehlion/netintrinsics/blob/master/NetIntrinsics/Intrinsic.cs
        private static readonly byte[] DeBruijn64 = new byte[sizeof(ulong) * 8]
        {
            00, 47, 01, 56, 48, 27, 02, 60,
            57, 49, 41, 37, 28, 16, 03, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11, 04, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30, 09, 24,
            13, 18, 08, 12, 07, 06, 05, 63
        };

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(uint value)
        {
            if (Lzcnt.IsSupported)
                return unchecked((int)Lzcnt.LeadingZeroCount(value));

            if (value == 0)
                return 32;

            if (X86Base.IsSupported)
                return 31 ^ unchecked((int)X86Base.BitScanReverse(value));

            return 31 ^ Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(ulong value)
        {
            if (Lzcnt.X64.IsSupported)
                return unchecked((int)Lzcnt.X64.LeadingZeroCount(value));

            if (value == 0UL)
                return 64;

            if (X86Base.X64.IsSupported)
                return 63 ^ unchecked((int)X86Base.X64.BitScanReverse(value));

            if (X86Base.IsSupported)
            {
                uint hi = (uint)(value >> 32);
                if (hi == 0)
                    return 32 + (31 ^ unchecked((int)X86Base.BitScanReverse((uint)value)));
                return 31 ^ unchecked((int)X86Base.BitScanReverse(hi));
            }

            return 63 ^ Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => LeadingZeroCount((uint)value),
                sizeof(ulong) => LeadingZeroCount((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => LeadingZeroCount((uint)value),
                    sizeof(ulong) => LeadingZeroCount((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(uint value)
        {
            if (Bmi1.IsSupported)
                return unchecked((int)Bmi1.TrailingZeroCount(value));

            if (value == 0)
                return 32;

            if (X86Base.IsSupported)
                return unchecked((int)X86Base.BitScanForward(value));

            return TrailingZeroCountSoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(ulong value)
        {
            if (Bmi1.X64.IsSupported)
                return unchecked((int)Bmi1.X64.TrailingZeroCount(value));

            if (Bmi1.IsSupported)
                return unchecked((int)(Bmi1.TrailingZeroCount((uint)(value >> 32)) + Bmi1.TrailingZeroCount((uint)value)));

            if (value == 0UL)
                return 64;

            if (X86Base.X64.IsSupported)
                return unchecked((int)X86Base.X64.BitScanForward(value));

            if (X86Base.IsSupported)
            {
                uint lo = unchecked((uint)value);
                if (lo == 0)
                    return 32 + unchecked((int)X86Base.BitScanForward((uint)(value >> 32)));
                return unchecked((int)X86Base.BitScanForward(lo));
            }

            return TrailingZeroCountSoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => TrailingZeroCount((uint)value),
                sizeof(ulong) => TrailingZeroCount((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => TrailingZeroCount((uint)value),
                    sizeof(ulong) => TrailingZeroCount((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };


        private static int TrailingZeroCountSoftwareFallback(uint value)
        {
            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return UnsafeHelper.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_0111_1100_1011_0101_0011_0001u
                ref TrailingZeroCountDeBruijn32[0],
                // uint|long -> IntPtr cast on 32-bit platforms does expensive overflow checks not needed here
                (nuint)(int)(((value & (uint)-(int)value) * 0x077CB531u) >> 27)); // Multi-cast mitigates redundant conv.u8
        }

        private static int TrailingZeroCountSoftwareFallback(ulong value)
        {
            return UnsafeHelper.AddByteOffset(
                ref TrailingZeroCountDeBruijn32[0],
                (nuint)(int)((((value ^ (value - 1)) * 0x03F79D71B4CB0A89uL) >> 58)));
        }
    }
}
#endif
