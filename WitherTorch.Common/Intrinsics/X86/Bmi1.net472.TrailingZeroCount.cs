#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Bmi1
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"
         *
         * __int32 __cdecl tzcnt_wrap(__int32 value)
         * {
         *     return _tzcnt_u32(value);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildTzcntAsm()
        {
#if B64_ARCH
            return BuildTzcntAsm_X64();
#elif B32_ARCH
            return BuildTzcntAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildTzcntAsm_X64() : BuildTzcntAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildTzcntAsm_X86() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildTzcntAsm_X86() : StoreAsArray.BuildTzcntAsm_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildTzcntAsm_X86()
            {
                const int Length = 9;
                byte[] data = new byte[Length] {
                    0xF3, 0x0F, 0xBC, 0x44, 
                    0x24, 0x04, 0xC2, 0x00, 
                    0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildTzcntAsm_X86()
            {
                const int Length = 9;
                ReadOnlySpan<byte> data = [
                    0xF3, 0x0F, 0xBC, 0x44,
                    0x24, 0x04, 0xC2, 0x00,
                    0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildTzcntAsm_X64() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildTzcntAsm_X64() : StoreAsArray.BuildTzcntAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildTzcntAsm_X64()
            {
                const int Length = 7;
                byte[] data = new byte[Length] {
                    0xF3, 0x0F, 0xBC, 0xC1, 
                    0xC2, 0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildTzcntAsm_X64()
            {
                const int Length = 7;
                ReadOnlySpan<byte> data = [
                    0xF3, 0x0F, 0xBC, 0xC1,
                    0xC2, 0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildTzcntAsm() => null;
#endif
    }
}
#endif