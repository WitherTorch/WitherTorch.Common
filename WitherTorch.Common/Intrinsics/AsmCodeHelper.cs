using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Intrinsics
{
    public static class AsmCodeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* PackAsmCodeIntoNativeMemory(byte[] source, nuint length)
        {
            fixed (byte* ptr = source)
                return PackAsmCodeIntoNativeMemory(ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* PackAsmCodeIntoNativeMemory(void* source, nuint length)
        {
            void* result = NativeMethods.AllocMemory(length);
            UnsafeHelper.CopyBlockUnaligned(result, source, length);
            NativeMethods.ProtectMemory(result, length,
                NativeMethods.ProtectMemoryFlags.CanExecute | NativeMethods.ProtectMemoryFlags.CanWrite | NativeMethods.ProtectMemoryFlags.CanRead);
            return result;
        }
    }
}
