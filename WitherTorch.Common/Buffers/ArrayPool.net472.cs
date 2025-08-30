#if NET472_OR_GREATER
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private static partial ArrayPool<T> CreateSharedPool()
        {
            if (WTCommon.SystemBuffersExists)
                return UnsafeCreateWrappedSystemArrayPool();
            return new SharedArrayPoolImpl();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static SystemArrayPoolWrapper UnsafeCreateWrappedSystemArrayPool()
        {
            System.Buffers.ArrayPool<T> pool = System.Buffers.ArrayPool<T>.Shared;
            return new SystemArrayPoolWrapper(pool);
        }
    }
}
#endif