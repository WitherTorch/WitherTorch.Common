using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static int CeilDiv(int a, int b)
        {
            int result = a / b;
            int rem = a % b;
            if (rem < 0)
                return result - 1;
            else if (rem > 0)
                return result + 1;
            else
                return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CeilDiv(uint a, uint b)
        {
            uint result = a / b;
            uint rem = a % b;
            if (rem < 0)
                return result - 1;
            else if (rem > 0)
                return result + 1;
            else
                return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static long CeilDiv(long a, long b)
        {
            long result = a / b;
            long rem = a % b;
            if (rem < 0)
                return result - 1;
            else if (rem > 0)
                return result + 1;
            else
                return result;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CeilDiv(ulong a, ulong b)
        {
            ulong result = a / b;
            ulong rem = a % b;
            if (rem < 0)
                return result - 1;
            else if (rem > 0)
                return result + 1;
            else
                return result;
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Pow([InlineParameter] int _base, [InlineParameter] int pow)
        {
            if (_base == 2)
                return 1 << pow;
            if (pow < 0 || pow > byte.MaxValue)
                return (int)Math.Pow(_base, pow);
            return (int)PowCore(_base, unchecked((byte)pow));
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Pow([InlineParameter] long _base, [InlineParameter] long pow)
        {
            if (_base == 2 && pow <= int.MaxValue)
                return 1L << unchecked((int)pow);
            if (pow < 0 || pow > byte.MaxValue)
                return (long)Math.Pow(_base, pow);
            return PowCore(_base, unchecked((byte)pow));
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int LeadingZeroCount(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(value);
#else
            return LeadingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int LeadingZeroCount(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(value);
#else
            return LeadingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int TrailingZeroCount(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(value);
#else
            return TrailingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int TrailingZeroCount(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(value);
#else
            return TrailingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int Log2(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.Log2(value);
#else
            return Log2Core(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int Log2(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.Log2(value);
#else
            return Log2Core(value);
#endif
        }
    }
}
