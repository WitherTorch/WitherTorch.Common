using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe class Kernel32
    {
        private const string KERNEL32_DLL = "kernel32.dll";

        [DllImport(KERNEL32_DLL)]
        public static extern bool CloseHandle(nint hObject);

        [DllImport(KERNEL32_DLL)]
        public static extern void* GetProcAddress(nint hModule, byte* lpProcName);

        [DllImport(KERNEL32_DLL)]
        public static extern nint LoadLibraryW(char* lpLibFileName);

        [DllImport(KERNEL32_DLL)]
        public static extern nint OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport(KERNEL32_DLL)]
        public static extern int GetCurrentThreadId();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint LoadLibrary(string lpLibFileName)
        {
            fixed (char* ptr = lpLibFileName)
                return LoadLibraryW(ptr);
        }
    }
}
