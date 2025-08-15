using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            {
#pragma warning disable CS0162
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        UnsafeHelper.CopyBlock(destination, source, unchecked((uint)sizeInBytes));
                        break;
                    case sizeof(ulong):
                        UnsafeHelper.CopyBlock(destination, source, unchecked((uint)sizeInBytes));
                        UnsafeHelper.CopyBlock(destination, source, unchecked((uint)((ulong)sizeInBytes >> 32)));
                        break;
                    default:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                UnsafeHelper.CopyBlock(destination, source, unchecked((uint)sizeInBytes));
                                break;
                            case sizeof(ulong):
                                UnsafeHelper.CopyBlock(destination, source, unchecked((uint)sizeInBytes));
                                UnsafeHelper.CopyBlock(destination, source, unchecked((uint)((ulong)sizeInBytes >> 32)));
                                break;
                            default:
                                throw new NotSupportedException("Unsupported pointer size: " + UnsafeHelper.PointerSize);
                        }
                        break;
                }
#pragma warning restore CS0162
            }

            unsafe void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes)
            {
                for (nuint i = 0; i < sizeInBytes; i++)
                    ((byte*)destination)[i] = ((byte*)source)[i];
            }

            public unsafe void ProtectMemory(void* ptr, nuint length, ProtectMemoryFlags flags)
            {
                // Do nothing
            }
        }
    }
}
