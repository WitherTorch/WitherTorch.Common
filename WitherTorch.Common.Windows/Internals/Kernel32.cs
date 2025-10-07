using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Windows.Internals
{
    internal static unsafe partial class Kernel32
    {
        public static partial bool CloseHandle(IntPtr hObject);
        public static partial void* GetProcAddress(IntPtr hModule, byte* lpProcName);
        public static partial IntPtr LoadLibraryW(char* lpLibFileName);
        public static partial IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        public static partial int GetCurrentThreadId();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr LoadLibrary(string lpLibFileName)
        {
            fixed (char* ptr = lpLibFileName)
                return LoadLibraryW(ptr);
        }
    }
}
