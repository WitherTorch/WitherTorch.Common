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
         *
         * unsigned long bsr_wrap(unsigned long value)
         * {
         *     unsigned long result;
         *     _BitScanReverse(&result, value);
         *     return result;
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildBsrAsm()
        {
#if B64_ARCH
            return BuildBsrAsm_X64();
#elif B32_ARCH
            return BuildBsrAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildBsrAsm_X64() : BuildBsrAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildBsrAsm_X86()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildBsrAsm_X86() : StoreAsArray.BuildBsrAsm_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildBsrAsm_X86()
            {
                const int Length = 8;
                byte[] data = new byte[Length] {
                    0x0F, 0xBD, 0x44, 0x24,
                    0x04, 0xC2, 0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildBsrAsm_X86()
            {
                const int Length = 8;
                ReadOnlySpan<byte> data = [
                    0x0F, 0xBD, 0x44, 0x24,
                    0x04, 0xC2, 0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildBsrAsm_X64() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildBsrAsm_X64() : StoreAsArray.BuildBsrAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildBsrAsm_X64()
            {
                const int Length = 6;
                byte[] data = new byte[Length] { 
                    0x0F, 0xBD, 0xC1, 0xC2, 
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildBsrAsm_X64()
            {
                const int Length = 6;
                ReadOnlySpan<byte> data = [
                    0x0F, 0xBD, 0xC1, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildBsrAsm() => null;
#endif
    }
}
#endif