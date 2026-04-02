using System.Runtime.InteropServices;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct NativeMemoryBlock
    {
        public static readonly NativeMemoryBlock Empty = new(null, 0);

        private readonly void* _nativePointer;
        private readonly nuint _length;

        public bool IsValid => _nativePointer != null;

        public void* NativePointer => _nativePointer;

        public nuint Length => _length;

        public NativeMemoryBlock(void* nativePointer, nuint length)
        {
            _nativePointer = nativePointer;
            _length = length;
        }
    }
}
