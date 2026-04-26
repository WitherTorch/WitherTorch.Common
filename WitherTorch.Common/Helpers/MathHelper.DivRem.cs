#if NET472_OR_GREATER
using System.Runtime.Intrinsics.X86;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Remove)]
        private static int DivRemCore(int a, int b, out int rem)
        {
            if (_isX86BaseSupported)
            {
                (int quotient, rem) = X86Base.DivRem((uint)a, -BooleanToInt32(a < 0), b);
                return quotient;
            }

            return DivRemSoftwareFallback(a, b, out rem);
        }

        [Inline(InlineBehavior.Remove)]
        private static uint DivRemCore(uint a, uint b, out uint rem)
        {
            if (_isX86BaseSupported)
            {
                (uint quotient, rem) = X86Base.DivRem(a, 0, b);
                return quotient;
            }

            return DivRemUnsignedSoftwareFallback(a, b, out rem);
        }

        [Inline(InlineBehavior.Remove)]
        private static long DivRemCore(long a, long b, out long rem)
        {
            if (_isX86Base_X64Supported)
            {
                (long quotient, rem) = X86Base.X64.DivRem((ulong)a, -BooleanToInt64(a < 0), b);
                return quotient;
            }

            return DivRemSoftwareFallback(a, b, out rem);
        }

        [Inline(InlineBehavior.Remove)]
        private static ulong DivRemCore(ulong a, ulong b, out ulong rem)
        {
            if (_isX86Base_X64Supported)
            {
                (ulong quotient, rem) = X86Base.X64.DivRem(a, 0, b);
                return quotient;
            }
            if (_isX86BaseSupported && b <= uint.MaxValue)
            {
                (uint quotient, rem) = X86Base.DivRem(a, (uint)b);
                return quotient;
            }

            return DivRemUnsignedSoftwareFallback(a, b, out rem);
        }
    }
}
#endif