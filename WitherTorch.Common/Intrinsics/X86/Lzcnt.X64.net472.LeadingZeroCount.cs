#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using InlineIL;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Lzcnt
    {
        partial class X64
        {
#if ((X86_ARCH && B64_ARCH) || ANYCPU)
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectLzcntAsm()
            {
#if !B64_ARCH
                if (!Helpers.PlatformHelper.IsX64)
                    return;
#endif
                InjectLzcntAsm_X64();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectLzcntAsm_X64()
            {
                if (WTCommon.SystemMemoryExists)
                    StoreAsSpan.InjectLzcntAsm_X64();
                else
                    StoreAsArray.InjectLzcntAsm_X64();
            }

            partial class StoreAsArray
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void InjectLzcntAsm_X64()
                {
                    const int Length = 8;
                    byte[] data = new byte[Length] {
                        0xF3, 0x48, 0x0F, 0xBD, 0xC1,
                        0xC3,
                        0xCC, 0xCC
                    };
                    IL.Emit.Ldtoken(MethodRef.Method(typeof(X64), nameof(LeadingZeroCount)));
                    IL.Pop(out RuntimeMethodHandle method);
                    AsmCodeHelper.InjectAsmCode(method, data, Length);
                }
            }

            partial class StoreAsSpan
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void InjectLzcntAsm_X64()
                {
                    const int Length = 8;
                    ReadOnlySpan<byte> data = [
                        0xF3, 0x48, 0x0F, 0xBD, 0xC1,
                        0xC3,
                        0xCC, 0xCC
                    ];
                    IL.Emit.Ldtoken(MethodRef.Method(typeof(X64), nameof(LeadingZeroCount)));
                    IL.Pop(out RuntimeMethodHandle method);
                    AsmCodeHelper.InjectAsmCode(method, data, Length);
                }
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InjectLzcntAsm() {};
#endif
        }
    }
}
#endif