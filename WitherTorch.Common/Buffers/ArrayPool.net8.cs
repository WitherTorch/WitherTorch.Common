#if NET8_0_OR_GREATER
namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private static partial ArrayPool<T> CreateSharedPool() => new SystemArrayPoolWrapper(System.Buffers.ArrayPool<T>.Shared);
    }
}
#endif