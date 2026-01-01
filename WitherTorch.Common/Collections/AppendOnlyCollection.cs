using System;
using System.Collections.Generic;

namespace WitherTorch.Common.Collections
{
    public static partial class AppendOnlyCollection
    {
        public static IAppendOnlyCollection<T> CreateLimitedCollection<T>(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0)
                return EmptyAOCollection<T>.Instance;
            return new LimitedAOCollection<T>(capacity);
        }

        public static IAppendOnlyCollection<T> CreateUnlimitedCollection<T>() => new UnlimitedAOCollection<T>();
    }

    public interface IAppendOnlyCollection<T> : IEnumerable<T>
    {
        int Count { get; }

        int Capacity { get; }

        T this[int index] { get; set; }

        void Append(T item);

        int BinarySearch(T item);

        int BinarySearch(T item, IComparer<T> comparer);

        void Clear();
    }
}