using System;
using System.Collections;
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

    public interface IAppendOnlyCollection<T> : IReadOnlyList<T>, IReversibleEnumerable<T>
    {
        int Capacity { get; }

        new T this[int index] { get; set; }

        void Append(T item);

        void Append(IEnumerable<T> items);

        int BinarySearch(T item);

        int BinarySearch(T item, IComparer<T> comparer);

        bool Contains(T item);

        bool Contains(T item, IEqualityComparer<T> comparer);

        int IndexOf(T item);

        int IndexOf(T item, IEqualityComparer<T> comparer);

        void Clear();

#if NET8_0_OR_GREATER
        T IReadOnlyList<T>.this[int index] => this[index];
#endif
    }
}