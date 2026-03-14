#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using LocalsInit;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    [SuppressUnmanagedCodeSecurity]
    unsafe partial class X86Base
    {
        private static readonly bool _isSupported;
        private static readonly void* _cpuIdAsm, _div64Asm, _udiv64Asm;

        static X86Base()
        {
            if (!PlatformHelper.IsX86)
                return;
            _isSupported = true;
            _cpuIdAsm = BuildCpuIdAsm();
            _div64Asm = BuildDiv64Asm();
            _udiv64Asm = BuildUDiv64Asm();
        }

        public static partial bool IsSupported => _isSupported;

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId)
        {
            UnsafeHelper.SkipInit(out Registers registers);
            ((delegate* unmanaged[Cdecl]<Registers*, int, int, void>)_cpuIdAsm)(&registers, functionId, subFunctionId);
            return UnsafeHelper.As<Registers, (int Eax, int Ebx, int Ecx, int Edx)>(registers);
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static partial uint BitScanForward(uint value)
        {
            InjectBsfAsm();

            switch (value)
            {
                case uint.MaxValue:
                    return 0;
                case 0:
                    return 32;
                default:
                    uint result = 0;
                    while ((value & 1) == 0)
                    {
                        value >>= 1;
                        result++;
                    }
                    return result;
            }
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static partial uint BitScanReverse(uint value)
        {
            InjectBsrAsm();

            switch (value)
            {
                case uint.MaxValue:
                    return 31;
                case 0:
                    return 0;
                default:
                    for (int i = 31; i >= 0; i--)
                    {
                        if (((value >> i) & 1) != 0)
                            return (uint)i;
                    }
                    return 0;
            }
        }


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

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = sizeof(int) * 4)]
        private readonly struct Registers
        {
            private readonly int _eax, _ebx, _ecx, _edx;

            public override readonly string ToString()
                => $"{{EAX = {_eax}, EBX = {_ebx}, ECX = {_ecx}, EDX = {_edx}}}";
        }
    }
}
#endif