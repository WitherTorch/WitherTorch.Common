using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
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
                _array = ArrayHelper.CreateUninitializedArray<T>(capacity);
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

            public void Append(IEnumerable<T> items)
            {
                switch (items)
                {
                    case T[] itemsArray:
                        AppendCore(itemsArray, itemsArray.Length);
                        break;
                    case UnwrappableList<T> itemsList:
                        AppendCore(itemsList.Unwrap(), itemsList.Count);
                        break;
                    case PooledList<T> itemsList:
                        AppendCore(itemsList.GetBuffer(), itemsList.Count);
                        break;
                    case IList<T> itemsList:
                        AppendCore(itemsList);
                        break;
                    case IReadOnlyList<T> itemsList:
                        AppendCore(itemsList);
                        break;
                    default:
                        using (IEnumerator<T> enumerator = items.GetEnumerator())
                        {
                            if (items is ICollection<T> collection)
                            {
                                int count = collection.Count;
                                int capacity = _array.Length;
                                if (count > capacity)
                                {
                                    for (int i = 0, skipCount = count - capacity; i < skipCount; i++)
                                    {
                                        if (!enumerator.MoveNext())
                                            return;
                                    }
                                }
                            }
                            while (enumerator.MoveNext())
                                Append(enumerator.Current);
                        }
                        break;
                }
            }

            private void AppendCore(T[] items, int itemsCount)
            {
                if (itemsCount <= 0)
                    return;

                T[] array = _array;
                int capacity = array.Length;
                if (itemsCount >= capacity)
                {
                    _startIndex = 0;
                    _count = capacity;
                    Array.Copy(items, itemsCount - capacity, array, 0, capacity);
                    return;
                }
                else
                {
                    int startIndex = _startIndex;
                    int count = _count;
                    if (count < capacity)
                    {
                        DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                        int space = capacity - count;
                        if (space >= itemsCount)
                        {
                            Array.Copy(items, 0, array, count, itemsCount);
                            _count = count + itemsCount;
                        }
                        else
                        {
                            Array.Copy(items, 0, array, count, space);
                            startIndex = itemsCount - space;
                            Array.Copy(items, space, array, 0, startIndex);
                            _startIndex = startIndex;
                            _count = capacity;
                        }
                    }
                    else
                    {
                        int space = capacity - count - startIndex;
                        if (space >= itemsCount)
                        {
                            Array.Copy(items, 0, array, startIndex, itemsCount);
                            _startIndex = startIndex + itemsCount;
                        }
                        else
                        {
                            Array.Copy(items, 0, array, startIndex, space);
                            startIndex = itemsCount - space;
                            Array.Copy(items, space, array, 0, startIndex);
                            _startIndex = startIndex;
                        }
                    }
                }
            }

            private void AppendCore(IList<T> items)
            {
                T[] array = _array;
                int capacity = array.Length;
                int itemsCount = items.Count;
                if (itemsCount >= capacity)
                {
                    _startIndex = 0;
                    _count = capacity;
                    for (int i = 0, j = itemsCount - capacity; i < capacity; i++, j++)
                        array[i] = items[j];
                    return;
                }
                else
                {
                    int startIndex = _startIndex;
                    int count = _count;
                    if (count < capacity)
                    {
                        DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                        int space = capacity - count;
                        if (space >= itemsCount)
                        {
                            items.CopyTo(array, count);
                            _count = count + itemsCount;
                        }
                        else
                        {
                            for (int i = count, j = 0; j < space; i++, j++)
                                array[i] = items[j];
                            startIndex = itemsCount - space;
                            for (int i = 0, j = space; i < startIndex; i++, j++)
                                array[i] = items[j];
                            _startIndex = startIndex;
                            _count = capacity;
                        }
                    }
                    else
                    {
                        int space = capacity - count - startIndex;
                        if (space >= itemsCount)
                        {
                            items.CopyTo(array, startIndex);
                            _startIndex = startIndex + itemsCount;
                        }
                        else
                        {
                            for (int i = startIndex, j = 0; j < space; i++, j++)
                                array[i] = items[j];
                            startIndex = itemsCount - space;
                            for (int i = 0, j = space; i < startIndex; i++, j++)
                                array[i] = items[j];
                            _startIndex = startIndex;
                        }
                    }
                }
            }

            private void AppendCore(IReadOnlyList<T> items)
            {
                T[] array = _array;
                int capacity = array.Length;
                int itemsCount = items.Count;
                if (itemsCount >= capacity)
                {
                    _startIndex = 0;
                    _count = capacity;
                    for (int i = 0, j = itemsCount - capacity; i < capacity; i++, j++)
                        array[i] = items[j];
                    return;
                }
                else
                {
                    int startIndex = _startIndex;
                    int count = _count;
                    if (count < capacity)
                    {
                        DebugHelper.ThrowIf(startIndex != 0, "Start index must be zero in this case!");
                        int space = capacity - count;
                        if (space >= itemsCount)
                        {
                            for (int i = count, j = 0; j < itemsCount; i++, j++)
                                array[i] = items[j];
                            _count = count + itemsCount;
                        }
                        else
                        {
                            for (int i = count, j = 0; j < space; i++, j++)
                                array[i] = items[j];
                            startIndex = itemsCount - space;
                            for (int i = 0, j = space; i < startIndex; i++, j++)
                                array[i] = items[j];
                            _startIndex = startIndex;
                            _count = capacity;
                        }
                    }
                    else
                    {
                        int space = capacity - count - startIndex;
                        if (space >= itemsCount)
                        {
                            for (int i = startIndex, j = 0; j < itemsCount; i++, j++)
                                array[i] = items[j];
                            _startIndex = startIndex + itemsCount;
                        }
                        else
                        {
                            for (int i = startIndex, j = 0; j < space; i++, j++)
                                array[i] = items[j];
                            startIndex = itemsCount - space;
                            for (int i = 0, j = space; i < startIndex; i++, j++)
                                array[i] = items[j];
                            _startIndex = startIndex;
                        }
                    }
                }
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

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator<T>(this);

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator<T>(this);

            IEnumerator<T> IReversibleEnumerable<T>.GetReversedEnumerator() => new ReversedEnumerator<T>(this);

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

            public bool Contains(T item)
            {
                int count = _count;
                if (count <= 0)
                    return false;
                T[] array = _array;
                DebugHelper.ThrowIf(count > array.Length);
                return SequenceHelper.Contains(array, item, 0, count);
            }

            public bool Contains(T item, IEqualityComparer<T> comparer)
            {
                int count = _count;
                if (count <= 0)
                    return false;
                T[] array = _array;
                DebugHelper.ThrowIf(count > array.Length);
                if (comparer is EqualityComparer<T> equalityComparer)
                {
                    if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                        return SequenceHelper.Contains(array, item, 0, count);
                    return ContainsCore(item, array, count, equalityComparer);
                }
                return ContainsCore(item, array, count, comparer);
            }

            public int IndexOf(T item)
            {
                int count = _count;
                if (count <= 0)
                    return -1;
                T[] array = _array;
                int capacity = array.Length;
                DebugHelper.ThrowIf(count > capacity);
                int index = SequenceHelper.IndexOf(array, item, 0, count);
                if (index < 0 || index >= count || count < capacity)
                    return -1;
                int startIndex = _startIndex;
                return index >= startIndex ? index - startIndex : (capacity - startIndex + index);
            }

            public int IndexOf(T item, IEqualityComparer<T> comparer)
            {
                int count = _count;
                if (count <= 0)
                    return -1;
                T[] array = _array;
                int capacity = array.Length;
                DebugHelper.ThrowIf(count > capacity);
                int index;
                if (comparer is EqualityComparer<T> equalityComparer)
                {
                    if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                        index = SequenceHelper.IndexOf(array, item, 0, count);
                    else
                        index = IndexOfCore(item, array, count, equalityComparer);
                }
                else
                    index = IndexOfCore(item, array, count, comparer);
                if (index < 0 || index >= count || count < capacity)
                    return -1;
                int startIndex = _startIndex;
                return index >= startIndex ? index - startIndex : (capacity - startIndex + index);
            }

            private static bool ContainsCore<TEqualityComparer>(T item, T[] array, int count, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<T>
            {
                ref T arrayRef = ref array[0];
                for (int i = 0; i < count; i++)
                {
                    if (comparer.Equals(item, UnsafeHelper.AddTypedOffset(ref arrayRef, i)))
                        return true;
                }
                return false;
            }

            private static int IndexOfCore<TEqualityComparer>(T item, T[] array, int count, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<T>
            {
                ref T arrayRef = ref array[0];
                for (int i = 0; i < count; i++)
                {
                    if (comparer.Equals(item, UnsafeHelper.AddTypedOffset(ref arrayRef, i)))
                        return i;
                }
                return -1;
            }
        }
    }
}