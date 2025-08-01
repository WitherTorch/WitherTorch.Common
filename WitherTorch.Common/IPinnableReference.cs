namespace WitherTorch.Common
{
    public interface IPinnableReference<T> where T : unmanaged
    {
        public ref readonly T GetPinnableReference();
    }
}
