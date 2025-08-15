#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using LocalsInit;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class X86Base
    {
        private static readonly bool _isSupported;
        private static readonly void* _cpuIdAsm, _bsfAsm, _bsrAsm, _div64Asm, _udiv64Asm;

        static X86Base()
        {
            if (!PlatformHelper.IsX86)
                return;
            _isSupported = true;
            _cpuIdAsm = BuildCpuIdAsm();
            _bsfAsm = BuildBsfAsm();
            _bsrAsm = BuildBsrAsm();
            _div64Asm = BuildDiv64Asm();
            _udiv64Asm = BuildUDiv64Asm();
        }

        public static partial bool IsSupported => _isSupported;

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId)
        {
            ((delegate* unmanaged[Cdecl]<out Registers, int, int, void>)_cpuIdAsm)(out Registers result, functionId, subFunctionId);
            return UnsafeHelper.As<Registers, (int Eax, int Ebx, int Ecx, int Edx)>(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial uint BitScanForward(uint value) => ((delegate* unmanaged[Cdecl]<uint, uint>)_bsfAsm)(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial uint BitScanReverse(uint value) => ((delegate* unmanaged[Cdecl]<uint, uint>)_bsrAsm)(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (int Quotient, int Remainder) DivRem(long dividend, int divisor)
        {
            int remainder;
            int quotient = ((delegate* unmanaged[Cdecl]<long, int, int*, int>)_div64Asm)(dividend, divisor, &remainder);
            return (quotient, remainder);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (uint Quotient, uint Remainder) DivRem(ulong dividend, uint divisor)
        {
            uint remainder;
            uint quotient = ((delegate* unmanaged[Cdecl]<ulong, uint, uint*, uint>)_udiv64Asm)(dividend, divisor, &remainder);
            return (quotient, remainder);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (int Quotient, int Remainder) DivRem(uint lower, int upper, int divisor)
            => DivRem(unchecked((long)((ulong)upper << 32 | lower)), divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (uint Quotient, uint Remainder) DivRem(uint lower, uint upper, uint divisor)
            => DivRem(unchecked((ulong)upper << 32 | lower), divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (nint Quotient, nint Remainder) DivRem(nuint lower, nint upper, nint divisor)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => DivRem((uint)lower, (int)upper, (int)divisor),
                sizeof(long) => UnsafeHelper.As<(long, long), (nint, nint)>(X64.DivRem(lower, upper, divisor)),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => DivRem((uint)lower, (int)upper, (int)divisor),
                    sizeof(long) => UnsafeHelper.As<(long, long), (nint, nint)>(X64.DivRem(lower, upper, divisor)),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (nuint Quotient, nuint Remainder) DivRem(nuint lower, nuint upper, nuint divisor)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => DivRem((uint)lower, (uint)upper, (uint)divisor),
                sizeof(long) => UnsafeHelper.As<(ulong, ulong), (nuint, nuint)>(X64.DivRem(lower, upper, divisor)),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => DivRem((uint)lower, (uint)upper, (uint)divisor),
                    sizeof(long) => UnsafeHelper.As<(ulong, ulong), (nuint, nuint)>(X64.DivRem(lower, upper, divisor)),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private readonly struct Registers
        {
            private readonly int _eax, _ebx, _ecx, _edx;
        }
    }
}
#endif