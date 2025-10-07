#if NET472_OR_GREATER
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe partial class Kernel32
    {
        private const string KERNEL32_DLL = "kernel32.dll";

        [DllImport(KERNEL32_DLL)]
        public static extern partial bool CloseHandle(IntPtr hObject);

        [DllImport(KERNEL32_DLL)]
        public static extern partial void* GetProcAddress(IntPtr hModule, byte* lpProcName);

        [DllImport(KERNEL32_DLL)]
        public static extern partial IntPtr LoadLibraryW(char* lpLibFileName);

        [DllImport(KERNEL32_DLL)]
        public static extern partial IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport(KERNEL32_DLL)]
        public static extern partial int GetCurrentThreadId();
    }
}
#endif