using System.Collections.Generic;

namespace WitherTorch.CrossNative
{
    public interface IUnwrappableList<T> : IList<T>
    {
        T?[] Unwrap();
    }
}
