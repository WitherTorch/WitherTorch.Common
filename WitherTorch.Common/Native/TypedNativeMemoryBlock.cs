using System;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct TypedNativeMemoryBlock<T> where T : unmanaged
    {
        private readonly T* _nativePointer;
        private readonly void* _size;

        public bool IsValid => _nativePointer != null;

        public T* NativePointer => _nativePointer;

        public int Size => MathHelper.MakeSigned((uint)_size);

        public long LongSize => MathHelper.MakeSigned((ulong)_size);

        public IntPtr NativeSize => new IntPtr(_size);

        public TypedNativeMemoryBlock(T* nativePointer, void* size)
        {
            _nativePointer = nativePointer;
            _size = size;
        }
    }
}
