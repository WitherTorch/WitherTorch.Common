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
         * int32 __cdecl vmovmskps_wrapper(__m256 vector)
         * {
         *     return _mm256_movemask_ps(vector);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpsAsm_256()
        {
#if B64_ARCH
            return BuildVmovmskpsAsm_256_X64();
#elif B32_ARCH
            return BuildVmovmskpsAsm_256_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildVmovmskpsAsm_256_X64() : BuildVmovmskpsAsm_256_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpsAsm_256_X86()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVmovmskpsAsm_256_X86() : StoreAsArray.BuildVmovmskpsAsm_256_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpsAsm_256_X86()
            {
                const int Length = 10;
                byte[] data = new byte[Length] {
                    0xC5, 0xFC, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpsAsm_256_X86()
            {
                const int Length = 10;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xFC, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpsAsm_256_X64()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVmovmskpsAsm_256_X64() : StoreAsArray.BuildVmovmskpsAsm_256_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpsAsm_256_X64()
            {
                const int Length = 14;
                byte[] data = new byte[Length] {
                    0xC5, 0xFC, 0x10, 0x01, 
                    0xC5, 0xFC, 0x50, 0xC0, 
                    0xC5, 0xF8, 0x77, 0xC2, 
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVmovmskpsAsm_256_X64()
            {
                const int Length = 14;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xFC, 0x10, 0x01,
                    0xC5, 0xFC, 0x50, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVmovmskpsAsm_256() => null;
#endif
    }
}
#endif