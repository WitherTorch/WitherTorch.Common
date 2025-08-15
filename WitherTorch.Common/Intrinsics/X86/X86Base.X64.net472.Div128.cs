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
             * using uint64 = unsigned __int64;
             * using int64 = __int64;
             * 
             * int64 __cdecl div64_wrapper(uint64 lower, int64 upper, int64 divisor, int64* remainder)
             * {
             *     return _div128(upper, lower, divisor, remainder);
             * }
             */
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildDiv128Asm()
            {
#if B64_ARCH
                return BuildDiv128Asm_X64();
#else
                return Helpers.PlatformHelper.IsX64 ? BuildDiv128Asm_X64() : null;
#endif
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildDiv128Asm_X64()
                => WTCommon.SystemBuffersExists ? StoreAsSpan.BuildDiv128Asm_X64() : StoreAsArray.BuildDiv128Asm_X64();

            partial class StoreAsArray
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildDiv128Asm_X64()
                {
                    const int Length = 12;
                    byte[] data = new byte[Length] {
                        0x48, 0x89, 0xC8, 0x49, 
                        0xF7, 0xF8, 0x49, 0x89, 
                        0x11, 0xC2, 0x00, 0x00
                    };
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }

            partial class StoreAsSpan
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void* BuildDiv128Asm_X64()
                {
                    const int Length = 12;
                    ReadOnlySpan<byte> data = [
                        0x48, 0x89, 0xC8, 0x49,
                        0xF7, 0xF8, 0x49, 0x89,
                        0x11, 0xC2, 0x00, 0x00
                    ];
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(data, Length);
                }
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void* BuildDiv128Asm() => null;
#endif
        }
    }
}
#endif