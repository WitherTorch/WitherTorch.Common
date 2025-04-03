using System.Collections.Generic;

namespace WitherTorch.Common
{
    public interface IUnwrappableList<T> : IList<T>
    {
        T?[] Unwrap();
    }
}
