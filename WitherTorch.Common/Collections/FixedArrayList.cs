using System;

namespace WitherTorch.Common.Collections
{
    public sealed class FixedArrayList<T> : CustomListBase<T>
    {
        public FixedArrayList(T[] array) : base(array) { }

        public FixedArrayList(T[] array, int initialCount) : base(array, initialCount) { }

        public override void EnsureCapacity(int capacityAtLeast)
        {
            if (_array.Length < capacityAtLeast)
                throw new InvalidOperationException(nameof(FixedArrayList<>) + " cannot resizing!");
        }

        public T[] AsArray() => _array;
    }
}
