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
        public static unsafe NativeMemoryBlock AllocMemoryBlock(int size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe NativeMemoryBlock AllocMemoryBlock(uint size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe NativeMemoryBlock AllocMemoryBlock(long size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe NativeMemoryBlock AllocMemoryBlock(ulong size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(MathHelper.MakeSigned(size));
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe NativeMemoryBlock AllocMemoryBlock(nint size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(size);
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static unsafe NativeMemoryBlock AllocMemoryBlock(nuint size)
        {
            void* ptr = AllocMemory(size);
            if (ptr == default)
                return default;
            GC.AddMemoryPressure(MathHelper.MakeSigned(size));
            return new NativeMemoryBlock(ptr, (void*)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
#if B64_ARCH
        public static unsafe void FreeMemoryBlock(in NativeMemoryBlock block)
#else
        public static unsafe void FreeMemoryBlock(NativeMemoryBlock block)
#endif
        {
            void* ptr = block.NativePointer;
            if (ptr == null)
                return;
            _methodInstance.FreeMemory(ptr);
            GC.RemoveMemoryPressure(block.LongSize);
        }
    }
}
