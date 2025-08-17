using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class UnixNativeMethodInstance : INativeMethodInstance
        {
            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern int gettid();

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* malloc(nuint size);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void free(void* memblock);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memmove(void* dest, void* src, nuint sizeInBytes);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memcpy(void* dest, void* src, nuint sizeInBytes);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mmap(void* ptr, nuint length, ProtectMemoryPageFlags prot, MemoryMapFlags flags, int fd, nint offset);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mprotect(void* ptr, nuint length, ProtectMemoryPageFlags flags);

            int INativeMethodInstance.GetCurrentThreadId() => gettid();

            void* INativeMethodInstance.AllocMemory(nuint size) => malloc(size);

            void INativeMethodInstance.FreeMemory(void* ptr) => free(ptr);

            void INativeMethodInstance.CopyMemory(void* destination, void* source, nuint sizeInBytes) => memcpy(destination, source, sizeInBytes);

            void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes) => memmove(destination, source, sizeInBytes);

            unsafe void* INativeMethodInstance.AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags) 
                => mmap(null, size, flags, MemoryMapFlags.Private | MemoryMapFlags.Anomymous, -1, 0);

            void INativeMethodInstance.ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags) 
                => mprotect(ptr, size, flags);

            private enum MemoryMapFlags : uint
            {
                Failed = unchecked((uint)-1),

                Shared = 0x01,
                Private = 0x02,
                SharedValidate = 0x03,
                Fixed = 0x10,
                Anomymous = 0x20,
                NoReserve = 0x4000,
                GrowsDown = 0x0100,
                DenyWrite = 0x0800,
                Executable = 0x1000,
                Locked = 0x2000,
                Populate = 0x8000,
                NonBlock = 0x10000,
                Stack = 0x20000,
                HugeTlb = 0x40000,
                Sync = 0x80000,
                FixedNoReplace = 0x100000,
                File = 0
            }
        }
    }
}
