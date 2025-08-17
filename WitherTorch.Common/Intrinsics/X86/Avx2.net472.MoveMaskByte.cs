#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Avx2
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"
         *
         * using int32 = __int32;
         * 
         * int32 __cdecl vpmovmskb_wrapper(__m256i vector)
         * {
         *     return _mm256_movemask_epi8(vector);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVpmovmskbAsm()
        {
#if B64_ARCH
            return BuildVpmovmskbAsm_X64();
#elif B32_ARCH
            return BuildVpmovmskbAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildVpmovmskbAsm_X64() : BuildVpmovmskbAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVpmovmskbAsm_X86()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVpmovmskbAsm_X86() : StoreAsArray.BuildVpmovmskbAsm_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVpmovmskbAsm_X86()
            {
                const int Length = 10;
                byte[] data = new byte[Length] {
                    0xC5, 0xFD, 0xD7, 0xC0, 
                    0xC5, 0xF8, 0x77, 0xC2, 
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVpmovmskbAsm_X86()
            {
                const int Length = 10;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xFD, 0xD7, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVpmovmskbAsm_X64()
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildVpmovmskbAsm_X64() : StoreAsArray.BuildVpmovmskbAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVpmovmskbAsm_X64()
            {
                const int Length = 16;
                byte[] data = new byte[Length] {
                    0xC5, 0xF9, 0xEF, 0xC0,
                    0xC5, 0xFE, 0x6F, 0x01,
                    0xC5, 0xFD, 0xD7, 0xC0,
                    0xC5, 0xF8, 0x77, 0xC3
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildVpmovmskbAsm_X64()
            {
                const int Length = 16;
                ReadOnlySpan<byte> data = [
                    0xC5, 0xF9, 0xEF, 0xC0, 
                    0xC5, 0xFE, 0x6F, 0x01, 
                    0xC5, 0xFD, 0xD7, 0xC0, 
                    0xC5, 0xF8, 0x77, 0xC3
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildVpmovmskbAsm() => null;
#endif
    }
}
#endif