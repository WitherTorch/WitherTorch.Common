using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Collections
{
    partial class AppendOnlyCollection
    {
        private sealed class LimitedAOCollection<T> : IAppendOnlyCollection<T>
        {
            private readonly T[] _array;

            private int _startIndex, _count;

            public LimitedAOCollection(int capacity)
            {
                _array = new T[capacity];
                _startIndex = 0;
                _count = 0;
            }

            public int Count => _count;

            public int Capacity => _array.Length;

            public T this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => GetItemRef(index);
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => GetItemRef(index) = value;
            }

            public void Append(T item)
            {
                T[] array = _array;
                int length = array.Length;

                int startIndex = _startIndex;
                int count = _count;

                if (count < length)
                {
                    DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                    array[count++] = item;
                    _count = count;
                    return;
                }

                array[startIndex] = item;
                _startIndex = (++startIndex) % length;
            }

            public int BinarySearch(T item)
            {
                T[] array = _array;
                int length = array.Length;
                int startIndex = _startIndex;
                int count = _count;

                if (count < length || startIndex == 0)
                {
                    DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                    return Array.BinarySearch(array, 0, count, item);
                }

                int rightCount = length - startIndex;
                int result = Array.BinarySearch(array, startIndex, rightCount, item);
                if (result >= 0)
                    return result - startIndex;
                result = ~result;
                if (result == startIndex)
                    return ~0;
                if (result < length)
                    return ~(result - startIndex);
                result = Array.BinarySearch(array, 0, startIndex, item);
                if (result >= 0)
                    return result + rightCount;
                return ~(~result + rightCount);
            }

            public int BinarySearch(T item, IComparer<T> comparer)
            {
                T[] array = _array;
                int length = array.Length;
                int startIndex = _startIndex;
                int count = _count;

                if (count < length || startIndex == 0)
                {
                    DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                    return Array.BinarySearch(array, 0, count, item);
                }

                int rightCount = length - startIndex;
                int result = Array.BinarySearch(array, startIndex, rightCount, item, comparer);
                if (result >= 0)
                    return result - startIndex;
                result = ~result;
                if (result == startIndex)
                    return ~0;
                if (result < length)
                    return ~(result - startIndex);
                result = Array.BinarySearch(array, 0, startIndex, item, comparer);
                if (result >= 0)
                    return result + rightCount;
                return ~(~result + rightCount);
            }

            public void Clear()
            {
                if (!UnsafeHelper.IsUnmanagedType<T>())
                    Array.Clear(_array, 0, _array.Length);
                _startIndex = 0;
                _count = 0;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

            [Inline(InlineBehavior.Remove)]
            private ref T GetItemRef(int index)
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                T[] array = _array;
                int length = array.Length;
                int startIndex = _startIndex;
                int count = _count;

                if (count < length || startIndex == 0)
                    return ref array[index];

                return ref array[(startIndex + index) % length];
            }

            public sealed class Enumerator : IEnumerator<T>
            {
                private readonly LimitedAOCollection<T> _list;
                private int _index;

                public Enumerator(LimitedAOCollection<T> list)
                {
                    _list = list;
                    _index = -1;
                }

                public T Current => _list[_index];

                object? IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (_index + 1 < _list.Count)
                    {
                        _index++;
                        return true;
                    }
                    return false;
                }

                public void Reset() => _index = -1;
            }
        }
    }
}