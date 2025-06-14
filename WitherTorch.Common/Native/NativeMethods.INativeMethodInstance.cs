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
            void* AllocMemory(nuint size);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void FreeMemory(void* ptr);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void CopyMemory(void* destination, void* source, nuint sizeInBytes);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SecurityCritical]
            void MoveMemory(void* destination, void* source, nuint sizeInBytes);
        }
    }
}
