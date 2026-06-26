using System.Threading;

using RiceTea.Core.Native;
using RiceTea.Core.Windows.Internals;

namespace RiceTea.Core.Windows.Helpers;

public static unsafe class ThreadHelper
{
    private static readonly void* _setThreadDescriptionMethodPointer
        = NativeMethods.GetImportedMethodPointer("kernel32.dll", "SetThreadDescription");

    public static void SetCurrentThreadName(string name)
    {
        Thread.CurrentThread.Name = name; 
        SetCurrentThreadNameInWin32(name);
    }

    public static void SetCurrentThreadNameInWin32(string name)
    {
        void* methodPointer = _setThreadDescriptionMethodPointer;
        if (methodPointer == null)
            return;
        nint handle = Kernel32.OpenThread(0x0400u, false, unchecked((uint)Kernel32.GetCurrentThreadId()));
        if (handle == default)
            return;
        fixed (char* ptr = name)
            ((delegate* unmanaged[Stdcall]<nint, char*, void>)methodPointer)(handle, ptr);
        Kernel32.CloseHandle(handle);
    }
}
