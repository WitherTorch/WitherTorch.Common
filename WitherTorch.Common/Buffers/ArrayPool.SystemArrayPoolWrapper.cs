using System;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        internal sealed class SystemArrayPoolWrapper : ArrayPool<T>
        {
            private readonly System.Buffers.ArrayPool<T> _pool;

            public SystemArrayPoolWrapper(System.Buffers.ArrayPool<T> pool)
            {
                _pool = pool;
            }

            public override T[] Rent(nuint capacity)
            {
                if (capacity == 0)
                    return Array.Empty<T>();
                if (capacity > int.MaxValue)
                    return new T[capacity];
                return _pool.Rent(unchecked((int)capacity));
            }

            public override void Return(T[] obj, bool clearArray)
            {
                _pool.Return(obj, clearArray);
            }
        }
    }
}