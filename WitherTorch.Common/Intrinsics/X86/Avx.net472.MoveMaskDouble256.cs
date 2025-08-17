#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Avx
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"
         *
         * using int32 = __int32;
         * 
         * int32 __cdecl vmovmskpd_wrapper(__m256d vector)
         * {
         *     return _mm256_movemask_pd(vector);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpdAsm_256()
        {
#if B64_ARCH
            return BuildVmovmskpdAsm_256_X64();
#elif B32_ARCH
            return BuildVmovmskpdAsm_256_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildVmovmskpdAsm_256_X64() : BuildVmovmskpdAsm_256_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpdAsm_256_X86()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVmovmskpdAsm_256_X86() : StoreAsArray.BuildVmovmskpdAsm_256_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpdAsm_256_X86()
            {
                const int Length = 10;
                byte[] data = new byte[Length] {
                    0xC5, 0xFD, 0x50, 0xC0, 
                    0xC5, 0xF8, 0x77, 0xC2, 
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpdAsm_256_X86()
            {
                const int Length = 10;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xFD, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpdAsm_256_X64()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVmovmskpdAsm_256_X64() : StoreAsArray.BuildVmovmskpdAsm_256_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpdAsm_256_X64()
            {
                const int Length = 14;
                byte[] data = new byte[Length] {
                    0xC5, 0xFD, 0x10, 0x01,
                    0xC5, 0xFD, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpdAsm_256_X64()
            {
                const int Length = 14;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xFD, 0x10, 0x01,
                    0xC5, 0xFD, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpdAsm_256() => null;
#endif
    }
}
#endif