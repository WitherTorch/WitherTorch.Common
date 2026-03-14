#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class X86Base
    {
        partial class X64
        {
            private static readonly bool _isSupported;

            static X64()
            {
                if (!PlatformHelper.IsX64)
                    return;
                _isSupported = true;
            }

            public static partial bool IsSupported => _isSupported;

            [DebuggerHidden]
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            public static partial uint BitScanForward(ulong value)
            {
                InjectBsfAsm();

                return (uint)MathHelper.TrailingZeroCountSoftwareFallback(value);
            }

            [DebuggerHidden]
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            public static partial uint BitScanReverse(ulong value)
            {
                InjectBsrAsm();

                return (uint)MathHelper.Log2SoftwareFallback(value);
            }

            [DebuggerHidden]
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            public static partial (long Quotient, long Remainder) DivRem(ulong lower, long upper, long divisor)
            {
                InjectDiv128Asm();

                if (divisor == 0)
                    throw new DivideByZeroException();

                if (upper == ((long)lower >> 63))
                    return ((long)lower / divisor, (long)lower % divisor);

                bool isNegativeDividend = upper < 0;
                bool isNegativeDivisor = divisor < 0;
                bool isNegativeQuotient = isNegativeDividend ^ isNegativeDivisor;

                ulong uUpper = (ulong)upper;
                ulong uLower = lower;
                if (isNegativeDividend)
                {
                    uLower = ~uLower;
                    uUpper = ~uUpper;
                    if (++uLower == 0) uUpper++;
                }
                ulong uDivisor = (ulong)(isNegativeDivisor ? -divisor : divisor);

                if (uUpper >= uDivisor) throw new OverflowException();

                (ulong uQ, ulong uR) = DivRem(uLower, uUpper, uDivisor);

                long q = (long)uQ;
                long r = (long)uR;

                if (isNegativeQuotient) q = -q;
                if (isNegativeDividend) r = -r;

                return (q, r);
            }

            [DebuggerHidden]
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            public static partial (ulong Quotient, ulong Remainder) DivRem(ulong lower, ulong upper, ulong divisor)
            {
                InjectUDiv128Asm();

                if (divisor == 0)
                    throw new DivideByZeroException();

                if (upper == 0)
                    return (lower / divisor, lower % divisor);

                if (upper >= divisor)
                    throw new OverflowException("Quotient is too large (Integer Overflow).");

                ulong quotient = 0;
                ulong remainder = upper;

                for (int i = 63; i >= 0; i--)
                {
                    ulong bit = (lower >> i) & 1;
                    remainder = (remainder << 1) | bit;

                    if (remainder >= divisor)
                    {
                        remainder -= divisor;
                        quotient |= (1UL << i);
                    }
                }

                return (quotient, remainder);
            }

            private static partial class StoreAsArray { }

            private static partial class StoreAsSpan { }
        }
    }
}
#endif