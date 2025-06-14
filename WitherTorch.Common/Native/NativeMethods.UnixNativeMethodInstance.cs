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

            int INativeMethodInstance.GetCurrentThreadId() => gettid();

            void* INativeMethodInstance.AllocMemory(nuint size) => malloc(size);

            void INativeMethodInstance.FreeMemory(void* ptr) => free(ptr);

            void INativeMethodInstance.CopyMemory(void* destination, void* source, nuint sizeInBytes) => memcpy(destination, source, sizeInBytes);

            void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes) => memmove(destination, source, sizeInBytes);
        }
    }
}
