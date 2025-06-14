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
            T* result = (T*)_methodInstance.AllocMemory(unchecked((nuint)sizeof(T)));
            *result = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(int size) => _methodInstance.AllocMemory(MathHelper.MakeUnsigned(size));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(uint size) => _methodInstance.AllocMemory(size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(long size) => _methodInstance.AllocMemory(unchecked((nuint)MathHelper.MakeUnsigned(size)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(ulong size) => _methodInstance.AllocMemory(unchecked((nuint)size));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(nint size) => _methodInstance.AllocMemory(MathHelper.MakeUnsigned(size));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(nuint size) => _methodInstance.AllocMemory(size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemory(IntPtr ptr) => _methodInstance.FreeMemory(ptr.ToPointer());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemory(void* ptr) => _methodInstance.FreeMemory(ptr);
    }
}
