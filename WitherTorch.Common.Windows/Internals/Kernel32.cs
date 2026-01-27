using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.Internals
{
    internal static unsafe class Kernel32
    {
        private const string LibraryName = "kernel32.dll";

        [SuppressGCTransition]
        [DllImport(LibraryName)]
        public static extern bool CloseHandle(IntPtr hObject);

        [SuppressGCTransition]
        [DllImport(LibraryName)]
        public static extern void* GetProcAddress(IntPtr hModule, byte* lpProcName);

        [DllImport(LibraryName)]
        public static extern IntPtr LoadLibraryW(char* lpLibFileName);

        [SuppressGCTransition]
        [DllImport(LibraryName)]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [SuppressGCTransition]
        [DllImport(LibraryName)]
        public static extern int GetCurrentThreadId();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr LoadLibrary(string lpLibFileName)
        {
            fixed (char* ptr = lpLibFileName)
                return LoadLibraryW(ptr);
        }
    }
}
