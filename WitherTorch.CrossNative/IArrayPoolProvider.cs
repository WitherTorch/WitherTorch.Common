namespace WitherTorch.CrossNative
{
    public interface IArrayPoolProvider
    {
        IArrayPool<T> GetArrayPool<T>();
    }
}
