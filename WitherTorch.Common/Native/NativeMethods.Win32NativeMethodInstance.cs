using InlineIL;

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class Win32NativeMethodInstance : INativeMethodInstance
        {
            private readonly IntPtr _heap;

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern int GetCurrentThreadId();

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern IntPtr GetProcessHeap();

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void* HeapAlloc(IntPtr hHeap, int dwFlags, UIntPtr size);

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void HeapFree(IntPtr hHeap, int dwFlags, void* ptr);

            [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
            private static extern void qsort(void* ptr, IntPtr number, IntPtr width, void* compareFunc);

            public Win32NativeMethodInstance()
            {
                _heap = GetProcessHeap();
            }

            int INativeMethodInstance.GetCurrentThreadId()
            {
                return GetCurrentThreadId();
            }

            void* INativeMethodInstance.AllocMemory(int size)
            {
                return HeapAlloc(_heap, 0, new UIntPtr(unchecked((uint)size)));
            }

            void* INativeMethodInstance.AllocMemory(uint size)
            {
                return HeapAlloc(_heap, 0, new UIntPtr(size));
            }

            void* INativeMethodInstance.AllocMemory(IntPtr size)
            {
                return HeapAlloc(_heap, 0, new UIntPtr(size.ToPointer()));
            }

            void* INativeMethodInstance.AllocMemory(UIntPtr size)
            {
                return HeapAlloc(_heap, 0, size);
            }

            void INativeMethodInstance.FreeMemory(void* ptr)
            {
                HeapFree(_heap, 0, ptr);
            }
        }
    }
}
