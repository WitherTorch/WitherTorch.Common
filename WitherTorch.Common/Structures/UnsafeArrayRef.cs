using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Auto)]
    public readonly ref struct UnsafeArrayRef<T>
    {
        private readonly T[] _array;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeArrayRef(T[] array) => _array = array;

        public ref T FirstElement
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref UnsafeHelper.GetArrayDataReference(_array);
        }

        public ref T LastElement
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_array), _array.Length - 1);
        }

        public ref T this[nint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_array), index);
        }

        public ref T this[nuint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_array), index);
        }
    }
}
