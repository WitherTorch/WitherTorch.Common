using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        private unsafe interface INativeMethodInstance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            int GetCurrentThreadId();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void* AllocMemory(int size);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void* AllocMemory(uint size);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void* AllocMemory(IntPtr size);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void* AllocMemory(UIntPtr size);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void FreeMemory(void* ptr);
        }
    }
}
