namespace WitherTorch.Common.Buffers
{
    public interface IPool<T>
    {
        T Rent();

        void Return(T obj);
    }
}
