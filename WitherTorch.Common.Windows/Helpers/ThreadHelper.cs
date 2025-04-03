using System;
using System.Threading;

using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.Common.Windows.Helpers
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
            nint handle = Kernel32.OpenThread(0x0400u, false, unchecked((uint)Kernel32.GetCurrentThreadId()));
            if (handle == nint.Zero)
                return;
            fixed (char* ptr = name)
                ((delegate*<nint, char*, void>)methodPointer)(handle, ptr);
            Kernel32.CloseHandle(handle);
        }
    }
}
