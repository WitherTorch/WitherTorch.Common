#if NET472_OR_GREATER
using System;
using System.Runtime.Intrinsics.X86;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        // Source code from https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(uint value)
        {
            if (_isLzcntSupported)
            {
                // LZCNT contract is 0->32
                return (int)Lzcnt.LeadingZeroCount(value);
            }

            // Unguarded fallback contract is 0->31, BSR contract is 0->undefined
            if (value == 0)
                return 32;

            if (_isX86BaseSupported)
            {
                // LZCNT returns index starting from MSB, whereas BSR gives the index from LSB.
                // 31 ^ BSR here is equivalent to 31 - BSR since the BSR result is always between 0 and 31.
                // This saves an instruction, as subtraction from constant requires either MOV/SUB or NEG/ADD.
                return 31 ^ (int)X86Base.BitScanReverse(value);
            }

            return 31 ^ Log2SoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCountCore(ulong value)
        {
            if (_isLzcnt_X64Supported)
            {
                // LZCNT contract is 0->64
                return (int)Lzcnt.X64.LeadingZeroCount(value);
            }

            if (_isX86Base_X64Supported)
            {
                // BSR contract is 0->undefined
                return value == 0 ? 64 : 63 ^ (int)X86Base.X64.BitScanReverse(value);
            }

            uint hi = (uint)(value >> 32);

            if (hi == 0)
                return 32 + LeadingZeroCount((uint)value);

            return LeadingZeroCount(hi);
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
            if (_isBmi1Supported)
                return unchecked((int)Bmi1.TrailingZeroCount(value));

            if (value == 0)
                return 32;

            if (_isX86BaseSupported)
                return unchecked((int)X86Base.BitScanForward(value));

            return TrailingZeroCountSoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int TrailingZeroCountCore(ulong value)
        {
            if (_isBmi1_X64Supported)
                return unchecked((int)Bmi1.X64.TrailingZeroCount(value));

            if (value == 0UL)
                return 64;

            if (_isX86Base_X64Supported)
                return unchecked((int)X86Base.X64.BitScanForward(value));

            uint lo = (uint)value;

            if (lo == 0)
                return 32 + TrailingZeroCount((uint)(value >> 32));

            return TrailingZeroCount(lo);
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

        [Inline(InlineBehavior.Remove)]
        private static int PopCountCore(uint value)
        {
            if (_isPopcntSupported)
                return unchecked((int)Popcnt.PopCount(value));

            return PopCountSoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int PopCountCore(ulong value)
        {
            if (_isPopcnt_X64Supported)
                return unchecked((int)Popcnt.X64.PopCount(value));

            if (_isPopcntSupported)
                return unchecked((int)(Popcnt.PopCount((uint)value) + Popcnt.PopCount((uint)(value >> 32))));

            return PopCountSoftwareFallback(value);
        }

        [Inline(InlineBehavior.Remove)]
        private static int PopCountCore(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => PopCountCore((uint)value),
                sizeof(ulong) => PopCountCore((ulong)value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => PopCountCore((uint)value),
                    sizeof(ulong) => PopCountCore((ulong)value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        internal static int TrailingZeroCountSoftwareFallback(uint value)
        {
            if (_isSystemMemoryExists)
                return DeBruijn_StoreAsSpan.TrailingZeroCount(value);
            else
                return DeBruijn_StoreAsArray.TrailingZeroCount(value);
        }

        internal static int PopCountSoftwareFallback(uint value)
        {
            const uint c1 = 0x_55555555u;
            const uint c2 = 0x_33333333u;
            const uint c3 = 0x_0F0F0F0Fu;
            const uint c4 = 0x_01010101u;

            value -= (value >> 1) & c1;
            value = (value & c2) + ((value >> 2) & c2);
            value = (((value + (value >> 4)) & c3) * c4) >> 24;

            return (int)value;
        }

        internal static int PopCountSoftwareFallback(ulong value)
        {
            const ulong c1 = 0x_55555555_55555555ul;
            const ulong c2 = 0x_33333333_33333333ul;
            const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
            const ulong c4 = 0x_01010101_01010101ul;

            value -= (value >> 1) & c1;
            value = (value & c2) + ((value >> 2) & c2);
            value = (((value + (value >> 4)) & c3) * c4) >> 56;

            return (int)value;
        }
    }
}
#endif
