using System.Collections.Generic;

namespace WitherTorch.Common.Collections
{
    public interface IReversibleEnumerable<T> : IEnumerable<T>
    {
        IEnumerator<T> GetReversedEnumerator();
    }
}
