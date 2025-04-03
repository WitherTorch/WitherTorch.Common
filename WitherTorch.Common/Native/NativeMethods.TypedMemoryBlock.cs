using System;
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(int size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(uint size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(new UIntPtr(size));
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(long size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(new IntPtr(size));
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(ulong size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(new UIntPtr(size));
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(IntPtr size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(UIntPtr size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
#if B64_ARCH
        public static unsafe void FreeMemoryBlock<T>(in TypedNativeMemoryBlock<T> block) where T : unmanaged
#else
        public static unsafe void FreeMemoryBlock<T>(TypedNativeMemoryBlock<T> block) where T : unmanaged
#endif
        {
            void* ptr = block.NativePointer;
            if (ptr == null)
                return;
            _methodInstance.FreeMemory(ptr);
        }
    }
}
