using System;
using System.Threading;

using WitherTorch.CrossNative.Windows.Internals;

namespace WitherTorch.CrossNative.Windows.Helpers
{
    public static unsafe class ThreadHelper
    {
        private static readonly void* _setThreadDescriptionMethodPointer
            = MethodImportHelper.GetImportedMethodPointer("kernel32.dll", "SetThreadDescription");

        public static void SetCurrentThreadName(string name)
        {
            Thread.CurrentThread.Name = name;
            void* methodPointer = _setThreadDescriptionMethodPointer;
            if (methodPointer == null)
                return;
            IntPtr handle = Kernel32.OpenThread(0x0400u, false, unchecked((uint)Kernel32.GetCurrentThreadId()));
            if (handle == IntPtr.Zero)
                return;
            fixed (char* ptr = name)
                ((delegate*<IntPtr, char*, void>)methodPointer)(handle, ptr);
            Kernel32.CloseHandle(handle);
        }
    }
}
