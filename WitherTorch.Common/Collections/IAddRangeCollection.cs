using System.Collections.Generic;

namespace WitherTorch.Common.Collections
{
    public interface IAddRangeCollection<T> : ICollection<T>
    {
        void AddRange(IEnumerable<T> collection);
    }

    public interface IAddRangeCollectionGenerics<T> : IAddRangeCollection<T>
    {
        void AddRange<TEnumerable>(TEnumerable collection) where TEnumerable : IEnumerable<T>;

#if NET8_0_OR_GREATER
        void IAddRangeCollection<T>.AddRange(IEnumerable<T> collection) => AddRange(collection);
#endif
    }
}
