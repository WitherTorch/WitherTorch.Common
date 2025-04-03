using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.CrossNative.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    public static unsafe class Kernel32
    {
        private const string KERNEL32_DLL = "kernel32.dll";

        [DllImport(KERNEL32_DLL)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport(KERNEL32_DLL)]
        public static extern void* GetProcAddress(IntPtr hModule, byte* lpProcName);

        [DllImport(KERNEL32_DLL)]
        public static extern IntPtr LoadLibraryW(char* lpLibFileName);

        [DllImport(KERNEL32_DLL)]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport(KERNEL32_DLL)]
        public static extern int GetCurrentThreadId();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr LoadLibrary(string lpLibFileName)
        {
            fixed (char* ptr = lpLibFileName)
                return LoadLibraryW(ptr);
        }
    }
}
