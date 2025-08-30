using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Extensions
{
    public static class ArrayPoolExtensions
    {
        extension<T>(ArrayPool<T>)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ArrayPool<T> CreateFromCLRArrayPool(System.Buffers.ArrayPool<T> pool)
                => new ArrayPool<T>.SystemArrayPoolWrapper(pool);
        }
    }
}
