using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct TypedNativeMemoryBlock<T> where T : unmanaged
    {
        public static readonly TypedNativeMemoryBlock<T> Empty = new(null, 0);

        private readonly T* _nativePointer;
        private readonly nuint _length;

        public readonly bool IsValid => _nativePointer != null;

        public readonly nuint Length => _length;

        public readonly T* NativePointer => _nativePointer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryBlock(T* nativePointer, nuint length)
        {
            _nativePointer = nativePointer;
            _length = length;
        }
    }
}
