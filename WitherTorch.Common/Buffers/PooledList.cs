using System;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Buffers
{
    public sealed class PooledList<T> : CustomListBase<T>, IDisposable
    {
        private readonly ArrayPool<T> _pool;

        private bool _disposed;

        public bool IsDisposed => _disposed;

        public PooledList() : this(ArrayPool<T>.Shared, capacity: 16) { }

        public PooledList(int capacity) : this(ArrayPool<T>.Shared, capacity) { }

        public PooledList(ArrayPool<T> pool) : this(pool, capacity: 16) { }

        public PooledList(ArrayPool<T> pool, int capacity) : base(pool.Rent(capacity), initialCount: 0)
        {
            _pool = pool;
        }

        protected override void EnsureCapacity()
        {
            T[] array = _array;
            int capacity = array.Length;
            int count = _count;
            if (capacity >= count)
                return;

            int newCapacity;
            if (capacity >= Limits.MaxArrayLength / 2)
            {
                if (capacity >= Limits.MaxArrayLength)
                    throw new OutOfMemoryException();
                newCapacity = Limits.MaxArrayLength;
            }
            else
                newCapacity = MathHelper.Max(capacity * 2, count);

            ArrayPool<T> pool = _pool;
            T[] newArray = pool.Rent(newCapacity);
            Array.Copy(array, newArray, capacity);
            _array = newArray;
            pool.Return(array);
        }

        public T[] DestructAndReturnBuffer()
        {
            if (_disposed)
                return Array.Empty<T>();
            _disposed = true;

            T[] array = _array;
            _array = Array.Empty<T>();
            return array;
        }

        ~PooledList() => DisposeCore(disposing: false);

        public void Dispose()
        {
            DisposeCore(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void DisposeCore(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            T[] array = _array;
            if (disposing)
            {
                _array = Array.Empty<T>();
                _count = 0;
            }
            _pool.Return(array);
        }
    }
}
