#if NET472_OR_GREATER
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class X86Base
    {
        unsafe partial class X64
        {
            private static readonly bool _isSupported;
            private static readonly void* _bsfAsm, _bsrAsm, _div128Asm, _udiv128Asm;

            static X64()
            {
                if (!PlatformHelper.IsX64)
                    return;
                _isSupported = true;
                _bsfAsm = BuildBsfAsm();
                _bsrAsm = BuildBsrAsm();
                _div128Asm = BuildDiv128Asm();
                _udiv128Asm = BuildUDiv128Asm();
            }

            public static partial bool IsSupported => _isSupported;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial uint BitScanForward(ulong value) => ((delegate* unmanaged[Cdecl]<ulong, uint>)_bsfAsm)(value);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial uint BitScanReverse(ulong value) => ((delegate* unmanaged[Cdecl]<ulong, uint>)_bsrAsm)(value);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial (long Quotient, long Remainder) DivRem(ulong lower, long upper, long divisor)
            {
                long remainder;
                long quotient = ((delegate* unmanaged[Cdecl]<ulong, long, long, long*, long>)_div128Asm)(lower, upper, divisor, &remainder);
                return (quotient, remainder);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial (ulong Quotient, ulong Remainder) DivRem(ulong lower, ulong upper, ulong divisor)
            {
                ulong remainder;
                ulong quotient = ((delegate* unmanaged[Cdecl]<ulong, ulong, ulong, ulong*, ulong>)_udiv128Asm)(lower, upper, divisor, &remainder);
                return (quotient, remainder);
            }

            private static partial class StoreAsArray { }

            private static partial class StoreAsSpan { }
        }
    }
}
#endif