using System;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct NativeMemoryBlock
    {
        private readonly void* _nativePointer;
        private readonly void* _size;

        public bool IsValid => _nativePointer != null;

        public void* NativePointer => _nativePointer;

        public int Size => MathHelper.MakeSigned((uint)_size);

        public long LongSize => MathHelper.MakeSigned((ulong)_size);

        public IntPtr NativeSize => new IntPtr(_size);

        public NativeMemoryBlock(void* nativePointer, void* size)
        {
            _nativePointer = nativePointer;
            _size = size;
        }
    }
}
