using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Extensions;

public static class ArrayPoolExtensions2
{
    extension<T>(ArrayPool<T>)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T> CreateFromCLRArrayPool(System.Buffers.ArrayPool<T> pool)
        {
            if (pool == System.Buffers.ArrayPool<T>.Shared)
                return ArrayPool<T>.Shared;
            return new ArrayPool<T>.SystemBufferImpl(pool);
        }
    }
}
