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
            private static extern void* HeapAlloc(IntPtr hHeap, int dwFlags, nuint size);

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void HeapFree(IntPtr hHeap, int dwFlags, void* ptr);

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlMoveMemory(void* dest, void* src, nuint sizeInBytes);

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlCopyMemory(void* dest, void* src, nuint sizeInBytes);

            public Win32NativeMethodInstance()
            {
                _heap = GetProcessHeap();
            }

            int INativeMethodInstance.GetCurrentThreadId() => GetCurrentThreadId();

            void* INativeMethodInstance.AllocMemory(nuint size) => HeapAlloc(_heap, 0, size);

            void INativeMethodInstance.FreeMemory(void* ptr) => HeapFree(_heap, 0, ptr);

            void INativeMethodInstance.CopyMemory(void* destination, void* source, nuint sizeInBytes) => RtlCopyMemory(destination, source, sizeInBytes);

            void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes) => RtlMoveMemory(destination, source, sizeInBytes);
        }
    }
}
