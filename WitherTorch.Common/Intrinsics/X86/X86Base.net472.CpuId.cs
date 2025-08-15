#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class X86Base
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"

         * void cpuid_wrap(int foo[4], int e, int f)
         * {
         *     __cpuidex(foo, e, f);
         * }   
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildCpuIdAsm()
        {
#if B64_ARCH
            return BuildCpuIdAsm_X64();
#elif B32_ARCH
            return BuildCpuIdAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildCpuIdAsm_X64() : BuildCpuIdAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildCpuIdAsm_X86() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildCpuIdAsm_X86() : StoreAsArray.BuildCpuIdAsm_X86();

        partial class StoreAsArray
        {
            public static void* BuildCpuIdAsm_X86()
            {
                const int Length = 43;
                byte[] data = new byte[Length] {
                    0x8B, 0x84, 0x24, 0xFC,
                    0xFF, 0xFF, 0xFF, 0x8B,
                    0x8C, 0x24, 0xFC, 0xFF,
                    0xFF, 0xFF, 0x53, 0x56,
                    0x8B, 0xB4, 0x24, 0x04,
                    0x00, 0x00, 0x00, 0x0F,
                    0xA2, 0x89, 0x06, 0x31,
                    0xC0, 0x89, 0x5E, 0x04,
                    0x89, 0x4E, 0x08, 0x89,
                    0x56, 0x0C, 0x5E, 0x5B,
                    0xC2, 0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            public static void* BuildCpuIdAsm_X86()
            {
                const int Length = 43;
                ReadOnlySpan<byte> data = [
                    0x8B, 0x84, 0x24, 0xFC,
                    0xFF, 0xFF, 0xFF, 0x8B,
                    0x8C, 0x24, 0xFC, 0xFF,
                    0xFF, 0xFF, 0x53, 0x56,
                    0x8B, 0xB4, 0x24, 0x04,
                    0x00, 0x00, 0x00, 0x0F,
                    0xA2, 0x89, 0x06, 0x31,
                    0xC0, 0x89, 0x5E, 0x04,
                    0x89, 0x4E, 0x08, 0x89,
                    0x56, 0x0C, 0x5E, 0x5B,
                    0xC2, 0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildCpuIdAsm_X64()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildCpuIdAsm_X64() : StoreAsArray.BuildCpuIdAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildCpuIdAsm_X64()
            {
                const int Length = 40;
                byte[] data = new byte[Length] {
                    0x48, 0x89, 0x5C, 0x24,
                    0x08, 0x49, 0x89, 0xC9,
                    0x89, 0xD0, 0x44, 0x89,
                    0xC1, 0x0F, 0xA2, 0x41,
                    0x89, 0x01, 0x31, 0xC0,
                    0x41, 0x89, 0x59, 0x04,
                    0x48, 0x8B, 0x5C, 0x24,
                    0x08, 0x41, 0x89, 0x49,
                    0x08, 0x41, 0x89, 0x51,
                    0x0C, 0xC2, 0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildCpuIdAsm_X64()
            {
                const int Length = 40;
                ReadOnlySpan<byte> data = [
                    0x48, 0x89, 0x5C, 0x24,
                    0x08, 0x49, 0x89, 0xC9,
                    0x89, 0xD0, 0x44, 0x89,
                    0xC1, 0x0F, 0xA2, 0x41,
                    0x89, 0x01, 0x31, 0xC0,
                    0x41, 0x89, 0x59, 0x04,
                    0x48, 0x8B, 0x5C, 0x24,
                    0x08, 0x41, 0x89, 0x49,
                    0x08, 0x41, 0x89, 0x51,
                    0x0C, 0xC2, 0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildCpuIdAsm() => null;
#endif
    }
}
#endif