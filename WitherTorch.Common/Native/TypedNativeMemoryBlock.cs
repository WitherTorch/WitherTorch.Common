using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe readonly struct TypedNativeMemoryBlock<T> : IReadOnlyList<T> where T : unmanaged
    {
        public static readonly TypedNativeMemoryBlock<T> Empty = new(null, 0);

        private readonly T* _nativePointer;
        private readonly nuint _length;

        public bool IsValid => _nativePointer != null;

        public nuint Length => _length;

        public T* NativePointer => _nativePointer;

        int IReadOnlyCollection<T>.Count => (int)MathHelper.MakeSigned(_length);

        public T this[nuint index]
        {
            get
            {
                if (index >= _length)
                    throw new IndexOutOfRangeException();
                return _nativePointer[index];
            }
            set
            {
                if (index >= _length)
                    throw new IndexOutOfRangeException();
                _nativePointer[index] = value;
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException();
                return this[(nuint)index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryBlock(T* nativePointer, nuint length)
        {
            _nativePointer = nativePointer;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryBlock<TConvert> As<TConvert>() where TConvert : unmanaged 
            => new TypedNativeMemoryBlock<TConvert>((TConvert*)_nativePointer, _length * UnsafeHelper.SizeOf<T>() / UnsafeHelper.SizeOf<TConvert>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeMemoryBlock ToNativeMemoryBlock()
            => new NativeMemoryBlock(_nativePointer, _length * UnsafeHelper.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypedNativeMemoryEnumerator<T> GetEnumerator()
            => new TypedNativeMemoryEnumerator<T>(_nativePointer, _length);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
