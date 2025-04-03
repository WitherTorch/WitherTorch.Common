namespace WitherTorch.Common
{
    public interface IArrayPool<T>
    {
        T[] Rent(int length);

        void Return(T[] array);
    }
}
