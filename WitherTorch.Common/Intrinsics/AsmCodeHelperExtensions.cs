using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics
{
    internal static class AsmCodeHelperExtensions
    {
        extension(AsmCodeHelper)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void* PackAsmCodeIntoNativeMemory(in ReadOnlySpan<byte> span, nuint length)
            {
                fixed (byte* ptr = span)
                    return AsmCodeHelper.PackAsmCodeIntoNativeMemory(ptr, length);
            }
        }
    }
}
