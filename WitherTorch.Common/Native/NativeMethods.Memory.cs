using System;
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        public static unsafe char* AllocCStyleUtf16String(string value)
        {
            int length = value.Length;
            uint byteCount = unchecked((uint)length) * sizeof(char);
            char* result = (char*)_methodInstance.AllocMemory(byteCount + sizeof(char));
            result[length] = '\0';
            fixed (char* ptr = value)
                UnsafeHelper.CopyBlock(result, ptr, byteCount);
            return result;
        }

        public static unsafe T* AllocUnmanagedStructure<T>(T value) where T : unmanaged
        {
            T* result = (T*)_methodInstance.AllocMemory(sizeof(T));
            *result = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(int size)
        {
            return _methodInstance.AllocMemory(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(uint size)
        {
            return _methodInstance.AllocMemory(new UIntPtr(size));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(long size)
        {
            return _methodInstance.AllocMemory(new IntPtr(size));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(ulong size)
        {
            return _methodInstance.AllocMemory(new UIntPtr(size));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(IntPtr size)
        {
            return _methodInstance.AllocMemory(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void* AllocMemory(UIntPtr size)
        {
            return _methodInstance.AllocMemory(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void FreeMemory(IntPtr ptr)
        {
            _methodInstance.FreeMemory(ptr.ToPointer());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe void FreeMemory(void* ptr)
        {
            _methodInstance.FreeMemory(ptr);
        }
    }
}
