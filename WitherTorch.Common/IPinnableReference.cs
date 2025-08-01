namespace WitherTorch.Common
{
    public interface IPinnableReference<T> where T : unmanaged
    {
        ref readonly T GetPinnableReference();

        nuint GetPinnedLength();
    }
}
