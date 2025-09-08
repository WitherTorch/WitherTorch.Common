using System;
using System.Runtime.InteropServices;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        private sealed class FallbackNativeMethodInstance : INativeMethodInstance
        {
            int INativeMethodInstance.GetCurrentThreadId() => Environment.CurrentManagedThreadId;

            unsafe void* INativeMethodInstance.AllocMemory(nuint size) => Marshal.AllocHGlobal(unchecked((nint)size)).ToPointer();

            unsafe void INativeMethodInstance.FreeMemory(void* ptr) => Marshal.FreeHGlobal(new IntPtr(ptr));

            unsafe void INativeMethodInstance.CopyMemory(void* destination, void* source, nuint sizeInBytes)
                => UnsafeHelper.CopyBlockUnaligned(destination, source, sizeInBytes);

            unsafe void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes)
                => Buffer.MemoryCopy(source, destination, sizeInBytes, sizeInBytes);

            unsafe void* INativeMethodInstance.AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags)
            {
                return AllocMemory(size);
            }

            unsafe void INativeMethodInstance.ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
            {
                // Do nothing
            }

            public int GetCurrentProcessorId()
            {
#if NET8_0_OR_GREATER
                return Thread.GetCurrentProcessorId();
#else
                return Thread.CurrentThread.ManagedThreadId;
#endif
            }
        }
    }
}
