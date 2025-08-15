#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class X86Base
    {
        unsafe partial class X64
        {
#if ((X86_ARCH && B64_ARCH) || ANYCPU)
            /*
             * extern "C"
             *
             * unsigned long bsf_wrap(__int64 value)
             * {
             *     unsigned long result;
             *     _BitScanForward64(&result, value);
             *     return result;
             * }
             */
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildBsfAsm()
            {
#if B64_ARCH
                return BuildBsfAsm_X64();
#else
                return Helpers.PlatformHelper.IsX64 ? BuildBsfAsm_X64() : null;
#endif
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildBsfAsm_X64()
                => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildBsfAsm_X64() : StoreAsArray.BuildBsfAsm_X64();

            partial class StoreAsArray
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildBsfAsm_X64()
                {
                    const int Length = 7;
                    byte[] data = new byte[Length] {
                        0x48, 0x0F, 0xBC, 0xC1,
                        0xC2, 0x00, 0x00
                    };
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }

            partial class StoreAsSpan
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildBsfAsm_X64()
                {
                    const int Length = 7;
                    ReadOnlySpan<byte> data = [
                        0x48, 0x0F, 0xBC, 0xC1, 
                        0xC2, 0x00, 0x00
                    ];
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildBsfAsm() => null;
#endif
        }
    }
}
#endif