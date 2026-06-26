namespace RiceTea.Core;

public partial interface IPinnableReference<T> where T : unmanaged
{
    ref readonly T GetPinnableReference();

    nuint GetPinnedLength();
}
