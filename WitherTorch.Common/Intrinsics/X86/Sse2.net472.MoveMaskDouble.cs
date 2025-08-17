#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Sse2
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"
         *
         * using int32 = __int32;
         * 
         * int32 __cdecl movmskpd_wrapper(__m128d vector)
         * {
         *     return _mm_movemask_pd(vector);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpdAsm()
        {
#if B64_ARCH
            return BuildMovmskpdAsm_X64();
#elif B32_ARCH
            return BuildMovmskpdAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildMovmskpdAsm_X64() : BuildMovmskpdAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpdAsm_X86() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildMovmskpdAsm_X86() : StoreAsArray.BuildMovmskpdAsm_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpdAsm_X86()
            {
                const int Length = 7;
                byte[] data = new byte[Length] {
                    0x66, 0x0F, 0x50, 0xC0, 
                    0xC2, 0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpdAsm_X86()
            {
                const int Length = 7;
                ReadOnlySpan<byte> data = [
                    0x66, 0x0F, 0x50, 0xC0,
                    0xC2, 0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpdAsm_X64() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildMovmskpdAsm_X64() : StoreAsArray.BuildMovmskpdAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpdAsm_X64()
            {
                const int Length = 10;
                byte[] data = new byte[Length] {
                    0x0F, 0x10, 0x01, 0x66, 
                    0x0F, 0x50, 0xC0, 0xC2, 
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpdAsm_X64()
            {
                const int Length = 10;
                ReadOnlySpan<byte> data = [
                    0x0F, 0x10, 0x01, 0x66,
                    0x0F, 0x50, 0xC0, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpdAsm() => null;
#endif
    }
}
#endif