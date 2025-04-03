using System;

namespace WitherTorch.Common.Collections
{
    public sealed class FixedArrayList<T> : CustomListBase<T>
    {
        public FixedArrayList(T[] array) : base(array) { }

        public FixedArrayList(T[] array, int initialCount) : base(array, initialCount) { }

        protected override void EnsureCapacity()
        {
            if (_array.Length < _count)
                throw new InvalidOperationException(nameof(FixedArrayList<T>) + " cannot resizing!");
        }

        public T[] AsArray() => _array;
    }
}
