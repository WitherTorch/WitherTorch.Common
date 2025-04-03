namespace WitherTorch.Common
{
    public interface IArrayPoolProvider
    {
        IArrayPool<T> GetArrayPool<T>();
    }
}
