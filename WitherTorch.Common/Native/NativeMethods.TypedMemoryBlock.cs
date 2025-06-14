using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(int size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(uint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(long size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(ulong size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory(new UIntPtr(size));
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(nint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(nuint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            return new TypedNativeMemoryBlock<T>((T*)ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemoryBlock<T>(TypedNativeMemoryBlock<T> block) where T : unmanaged
        {
            void* ptr = block.NativePointer;
            if (ptr == null)
                return;
            _methodInstance.FreeMemory(ptr);
        }
    }
}
