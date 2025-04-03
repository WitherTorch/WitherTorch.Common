using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        private sealed class FallbackNativeMethodInstance : INativeMethodInstance
        {
            int INativeMethodInstance.GetCurrentThreadId()
            {
                return Environment.CurrentManagedThreadId;
            }

            unsafe void* INativeMethodInstance.AllocMemory(int size)
            {
                return Marshal.AllocHGlobal(size).ToPointer();
            }

            unsafe void* INativeMethodInstance.AllocMemory(uint size)
            {
                return Marshal.AllocHGlobal(new IntPtr(size)).ToPointer();
            }

            unsafe void* INativeMethodInstance.AllocMemory(IntPtr size)
            {
                return Marshal.AllocHGlobal(size).ToPointer();
            }

            unsafe void* INativeMethodInstance.AllocMemory(UIntPtr size)
            {
                return Marshal.AllocHGlobal(new IntPtr(size.ToPointer())).ToPointer();
            }

            unsafe void INativeMethodInstance.FreeMemory(void* ptr)
            {
                Marshal.FreeHGlobal(new IntPtr(ptr));
            }
        }
    }
}
