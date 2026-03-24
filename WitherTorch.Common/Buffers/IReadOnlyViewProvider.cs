namespace WitherTorch.Common.Buffers
{
    public interface IReadOnlyViewProvider<T> where T : unmanaged
    {
        ReadOnlyView<T> CreateView();
    }
}
