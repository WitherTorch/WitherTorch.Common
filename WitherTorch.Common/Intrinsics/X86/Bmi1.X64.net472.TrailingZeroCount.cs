#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using InlineIL;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        partial class X64
        {
#if ((X86_ARCH && B64_ARCH) || ANYCPU)
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectTzcntAsm()
            {
#if !B64_ARCH
                if (!PlatformHelper.IsX64)
                    return;
#endif
                InjectTzcntAsm_X64();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectTzcntAsm_X64()
            {
                if (WTCommon.SystemBuffersExists)
                    StoreAsSpan.InjectTzcntAsm_X64();
                else
                    StoreAsArray.InjectTzcntAsm_X64();
            }

            partial class StoreAsArray
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void InjectTzcntAsm_X64()
                {
                    const int Length = 8;
                    byte[] data = new byte[Length] {
                        0xF3, 0x48, 0x0F, 0xBC, 0xC1,
                        0xC3, 0xCC, 0xCC
                    };
                    IL.Emit.Ldtoken(MethodRef.Method(typeof(X64), nameof(TrailingZeroCount)));
                    IL.Pop(out RuntimeMethodHandle method);
                    AsmCodeHelper.InjectAsmCode(method, data, Length);
                }
            }

            partial class StoreAsSpan
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void InjectTzcntAsm_X64()
                {
                    const int Length = 8;
                    ReadOnlySpan<byte> data = [
                        0xF3, 0x48, 0x0F, 0xBC, 0xC1,
                        0xC3, 0xCC, 0xCC
                    ];
                    IL.Emit.Ldtoken(MethodRef.Method(typeof(X64), nameof(TrailingZeroCount)));
                    IL.Pop(out RuntimeMethodHandle method);
                    AsmCodeHelper.InjectAsmCode(method, data, Length);
                }
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectTzcntAsm() {};
#endif
        }
    }
}
#endif