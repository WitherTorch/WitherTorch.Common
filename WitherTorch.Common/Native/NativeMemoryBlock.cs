using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct NativeMemoryBlock : IReadOnlyList<byte>
    {
        public static readonly NativeMemoryBlock Empty = new(null, 0);

        private readonly void* _nativePointer;
        private readonly nuint _length;

        public bool IsValid => _nativePointer != null;

        public void* NativePointer => _nativePointer;

        public nuint Length => _length;

        int IReadOnlyCollection<byte>.Count => (int)MathHelper.MakeSigned(_length);

        public byte this[nuint index]
        {
            get
            {
                if (index >= _length)
                    throw new IndexOutOfRangeException();
                return ((byte*)_nativePointer)[index];
            }
            set
            {
                if (index >= _length)
                    throw new IndexOutOfRangeException();
                ((byte*)_nativePointer)[index] = value;
            }
        }

        byte IReadOnlyList<byte>.this[int index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException();
                return this[(nuint)index];
            }
        }

        public NativeMemoryBlock(void* nativePointer, nuint length)
        {
            _nativePointer = nativePointer;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryBlock<T> ToTypedNativeMemoryBlock<T>() where T : unmanaged
            => new TypedNativeMemoryBlock<T>((T*)_nativePointer, _length / UnsafeHelper.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryEnumerator<byte> GetEnumerator()
            => new TypedNativeMemoryEnumerator<byte>((byte*)_nativePointer, _length);

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
