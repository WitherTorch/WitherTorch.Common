#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        unsafe partial class X64
        {
#if ((X86_ARCH && B64_ARCH) || ANYCPU)
            /*
             * extern "C"
             *
             * __int64 __cdecl lzcnt_wrap(__int64 value)
             * {
             *     return _lzcnt_u64(value);
             * }
             */
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildLzcntAsm()
            {
#if B64_ARCH
                return BuildLzcntAsm_X64();
#else
                return Helpers.PlatformHelper.IsX64 ? BuildLzcntAsm_X64() : null;
#endif
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildLzcntAsm_X64()
                => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildLzcntAsm_X64() : StoreAsArray.BuildLzcntAsm_X64();

            partial class StoreAsArray
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildLzcntAsm_X64()
                {
                    const int Length = 8;
                    byte[] data = new byte[Length] {
                        0xF3, 0x48, 0x0F, 0xBD,
                        0xC1, 0xC2, 0x00, 0x00
                    };
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }

            partial class StoreAsSpan
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildLzcntAsm_X64()
                {
                    const int Length = 8;
                    ReadOnlySpan<byte> data = [
                        0xF3, 0x48, 0x0F, 0xBD, 
                        0xC1, 0xC2, 0x00, 0x00
                    ];
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildLzcntAsm() => null;
#endif
        }
    }
}
#endif