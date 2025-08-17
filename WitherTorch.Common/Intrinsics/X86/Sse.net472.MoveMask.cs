#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Sse
    {
#if (X86_ARCH || ANYCPU)
        /*
         * extern "C"
         *
         * using int32 = __int32;
         * 
         * int32 __cdecl movmskps_wrapper(__m128 vector)
         * {
         *     return _mm_movemask_ps(vector);
         * }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpsAsm()
        {
#if B64_ARCH
            return BuildMovmskpsAsm_X64();
#elif B32_ARCH
            return BuildMovmskpsAsm_X86();
#else
            return Helpers.PlatformHelper.IsX64 ? BuildMovmskpsAsm_X64() : BuildMovmskpsAsm_X86();
#endif
        }

#if (B32_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpsAsm_X86() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildMovmskpsAsm_X86() : StoreAsArray.BuildMovmskpsAsm_X86();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpsAsm_X86()
            {
                const int Length = 6;
                byte[] data = new byte[Length] {
                    0x0F, 0x50, 0xC0, 0xC2,
                    0x00, 0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpsAsm_X86()
            {
                const int Length = 6;
                ReadOnlySpan<byte> data = [
                    0x0F, 0x50, 0xC0, 0xC2,
                    0x00, 0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }
#endif

#if (B64_ARCH || ANYCPU)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpsAsm_X64() 
            => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildMovmskpsAsm_X64() : StoreAsArray.BuildMovmskpsAsm_X64();

        partial class StoreAsArray
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpsAsm_X64()
            {
                const int Length = 9;
                byte[] data = new byte[Length] {
                    0x0F, 0x10, 0x01, 0x0F,
                    0x50, 0xC0, 0xC2, 0x00,
                    0x00
                };
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

        partial class StoreAsSpan
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void* BuildMovmskpsAsm_X64()
            {
                const int Length = 9;
                ReadOnlySpan<byte> data = [
                    0x0F, 0x10, 0x01, 0x0F,
                    0x50, 0xC0, 0xC2, 0x00,
                    0x00
                ];
                return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
            }
        }

#endif
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* BuildMovmskpsAsm() => null;
#endif
    }
}
#endif