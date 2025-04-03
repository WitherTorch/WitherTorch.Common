using System;
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
            private static extern void* malloc(UIntPtr size);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void free(void* memblock);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void qsort(void* ptr, IntPtr number, IntPtr width, void* compareFunc);

            int INativeMethodInstance.GetCurrentThreadId()
            {
                return gettid();
            }

            void* INativeMethodInstance.AllocMemory(int size)
            {
                return malloc(new UIntPtr(unchecked((uint)size)));
            }

            void* INativeMethodInstance.AllocMemory(uint size)
            {
                return malloc(new UIntPtr(size));
            }

            void* INativeMethodInstance.AllocMemory(IntPtr size)
            {
                return malloc(new UIntPtr(size.ToPointer()));
            }

            void* INativeMethodInstance.AllocMemory(UIntPtr size)
            {
                return malloc(size);
            }

            void INativeMethodInstance.FreeMemory(void* ptr)
            {
                free(ptr);
            }
        }
    }
}
