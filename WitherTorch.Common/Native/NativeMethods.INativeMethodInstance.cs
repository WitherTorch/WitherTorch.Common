using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        private unsafe interface INativeMethodInstance
        {        
            int GetCurrentThreadId();
            void* AllocMemory(nuint size);
            void FreeMemory(void* ptr);
            void CopyMemory(void* destination, void* source, nuint sizeInBytes);
            void MoveMemory(void* destination, void* source, nuint sizeInBytes);
            void ProtectMemory(void* ptr, nuint length, ProtectMemoryFlags flags);
        }

        [Flags]
        public enum ProtectMemoryFlags : int
        {
            None = 0x0,
            CanRead = 0x1,
            CanWrite = 0x2,
            CanExecute = 0x4,
        }
    }
}
