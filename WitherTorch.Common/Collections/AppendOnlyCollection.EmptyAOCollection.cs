using System;
using System.Collections;
using System.Collections.Generic;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Collections
{
    partial class AppendOnlyCollection
    {
        private sealed class EmptyAOCollection<T> : IAppendOnlyCollection<T>
        {
            public static readonly EmptyAOCollection<T> Instance = new EmptyAOCollection<T>();

            private EmptyAOCollection() { }

            public T this[int index]
            {
                get => throw new IndexOutOfRangeException(nameof(index));
                set => throw new IndexOutOfRangeException(nameof(index));
            }

            public int Count => 0;

            public int Capacity => 0;

            public void Append(T item) { }

            public void Append(IEnumerable<T> items) { }

            public int BinarySearch(T item) => -1;

            public int BinarySearch(T item, IComparer<T> comparer) => -1;

            public void Clear() { }

            public bool Contains(T item) => false;

            public bool Contains(T item, IEqualityComparer<T> comparer) => false;

            public IEnumerator<T> GetEnumerator() => EnumeratorHelper.CreateEmptyEnumerator<T>();

            public int IndexOf(T item) => -1;

            public int IndexOf(T item, IEqualityComparer<T> comparer) => -1;

            IEnumerator IEnumerable.GetEnumerator() => EnumeratorHelper.CreateEmptyEnumerator<T>();
        }
    }
}