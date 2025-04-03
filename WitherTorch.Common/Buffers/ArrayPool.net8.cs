#if NET8_0_OR_GREATER
using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Collections
{
    internal sealed class SharedArrayPool<T> : ArrayPool<T>
    {
        private static readonly SharedArrayPool<T> _instance = new SharedArrayPool<T>(System.Buffers.ArrayPool<T>.Shared);

        private readonly System.Buffers.ArrayPool<T> _pool;

        public static SharedArrayPool<T> Instance => _instance;

        private SharedArrayPool(System.Buffers.ArrayPool<T> pool)
        {
            _pool = pool;
        }

        public override T[] Rent(int capacity)
        {
            return _pool.Rent(capacity);
        }

        public override void Return(T[] obj, bool clearArray)
        {
            _pool.Return(obj, clearArray);
        }
    }
}
#endif