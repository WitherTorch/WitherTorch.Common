using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

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
            GC.AddMemoryPressure(size);
            return new TypedNativeMemoryBlock<T>((T*)ptr, (nuint)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(uint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new TypedNativeMemoryBlock<T>((T*)ptr, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(long size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new TypedNativeMemoryBlock<T>((T*)ptr, (nuint)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(ulong size) where T : unmanaged
        {
            void* ptr = _methodInstance.AllocMemory((nuint)size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(MathHelper.MakeSigned(size));
            return new TypedNativeMemoryBlock<T>((T*)ptr, (nuint)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(nint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new TypedNativeMemoryBlock<T>((T*)ptr, (nuint)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TypedNativeMemoryBlock<T> AllocMemoryBlock<T>(nuint size) where T : unmanaged
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(MathHelper.MakeSigned(size));
            return new TypedNativeMemoryBlock<T>((T*)ptr, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemoryBlock<T>(in TypedNativeMemoryBlock<T> block) where T : unmanaged
        {
            void* ptr = block.NativePointer;
            if (ptr == null)
                return;
            _methodInstance.FreeMemory(ptr);
            GC.RemoveMemoryPressure(MathHelper.MakeSigned(block.Length));
        }
    }
}
