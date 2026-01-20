using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Collections
{
    partial class AppendOnlyCollection
    {
        private sealed class UnlimitedAOCollection<T> : IAppendOnlyCollection<T>
        {
            private const int MinimumNodeSize = 16;
            private static readonly int DefaultNodeSize;
            private readonly int _nodeSize;

            private object _treeHeadNode;
            private int _count, _depth;

            static unsafe UnlimitedAOCollection()
            {
#pragma warning disable CS8500
                DefaultNodeSize = MathHelper.Max((Environment.SystemPageSize - UnsafeHelper.PointerSize * 3) / sizeof(T), MinimumNodeSize);
#pragma warning restore CS8500
            }

            public UnlimitedAOCollection() : this(DefaultNodeSize) { }

            public UnlimitedAOCollection(int nodeSize)
            {
                _nodeSize = nodeSize;
                _treeHeadNode = ArrayHelper.CreateUninitializedArray<T>(nodeSize);
                _depth = 1;
                _count = 0;
            }

            public int Count => _count;

            public int Capacity => int.MaxValue;

            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= _count)
                        throw new IndexOutOfRangeException();

                    int depth = _depth;

                    if (!TryDecodePath(index, _nodeSize, depth, out ArrayPool<int>? pool, out int[]? pathBuffer))
                        throw new InvalidOperationException("Cannot decode path!");

                    return GetNodeReference(pool, pathBuffer);
                }
                set
                {
                    if (index < 0 || index >= _count)
                        throw new IndexOutOfRangeException();

                    int depth = _depth;

                    if (!TryDecodePath(index, _nodeSize, depth, out ArrayPool<int>? pool, out int[]? pathBuffer))
                        throw new InvalidOperationException("Cannot decode path!");

                    GetNodeReference(pool, pathBuffer) = value;
                }
            }

            public unsafe void Append(T item)
            {
                int count = _count;
                AppendCore(item, count);
                _count = count + 1;
            }

            public unsafe void Append(IEnumerable<T> items)
            {
                int index = _count;
                switch (items)
                {
                    case T[] array:
                        {
                            int length = array.Length;
                            if (length <= 0)
                                break;
                            ref T arrayRef = ref array[0];
                            for (int i = 0; i < length; i++)
                                AppendCore(UnsafeHelper.AddTypedOffset(ref arrayRef, i), index + i);
                            index += length;
                        }
                        break;
                    case IList<T> list:
                        {
                            int count = list.Count;
                            if (count <= 0)
                                break;
                            for (int i = 0; i < count; i++)
                                AppendCore(list[i], index + i);
                            index += count;
                        }
                        break;
                    case IReadOnlyList<T> list:
                        {
                            int count = list.Count;
                            if (count <= 0)
                                break;
                            for (int i = 0; i < count; i++)
                                AppendCore(list[i], index + i);
                            index += count;
                        }
                        break;
                    default:
                        {
                            using IEnumerator<T> enumerator = items.GetEnumerator();
                            for (; enumerator.MoveNext(); index++)
                                AppendCore(enumerator.Current, index);
                        }
                        break;
                }
                _count = index;
            }

            public int BinarySearch(T item)
            {
                int count = _count;
                if (count <= 0)
                    return -1;

                // Time complexity: O[(log n)^2]
                return UnsafeHelper.IsPrimitiveType<T>() ?
                    BinarySearchCoreFast(item, 0, count - 1) :
                    BinarySearchCoreSlow(item, 0, count - 1, Comparer<T>.Default);
            }

            public int BinarySearch(T item, IComparer<T> comparer)
            {
                int count = _count;
                if (count <= 0)
                    return -1;
                int upperBound = count - 1;

                // Time complexity: O[(log n)^2]
                if (comparer is Comparer<T> instancedComparer)
                {
                    if (UnsafeHelper.IsPrimitiveType<T>() && ReferenceEquals(instancedComparer, Comparer<T>.Default))
                        return BinarySearchCoreFast(item, 0, upperBound);
                    else
                        return BinarySearchCoreSlow(item, 0, upperBound, instancedComparer);
                }
                return BinarySearchCoreSlow(item, 0, upperBound, comparer);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int BinarySearchCoreFast(T value, int lowerBound, int higherBound) // Fast route: only for primitive type
            {
                while (lowerBound <= higherBound)
                {
                    int middleIndex = GetMiddleIndex(lowerBound, higherBound);
                    T middle = this[middleIndex];

                    if (UnsafeHelper.Equals(middle, value))
                        return middleIndex;
                    if (UnsafeHelper.IsLessThan(middle, value))
                        lowerBound = middleIndex + 1;
                    else
                        higherBound = middleIndex - 1;
                }
                return ~lowerBound;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int BinarySearchCoreSlow<TComparer>(T value, int lowerBound, int higherBound, TComparer comparer) where TComparer : IComparer<T> // Slow route
            {
                while (lowerBound <= higherBound)
                {
                    int middleIndex = GetMiddleIndex(lowerBound, higherBound);

                    int comparison = comparer.Compare(this[middleIndex], value);

                    if (comparison == 0)
                        return middleIndex;

                    if (comparison < 0)
                        lowerBound = middleIndex + 1;
                    else
                        higherBound = middleIndex - 1;
                }
                return ~lowerBound;
            }

            [Inline(InlineBehavior.Remove)]
            private static int GetMiddleIndex(int lowerBound, int higherBound) => lowerBound + ((higherBound - lowerBound) >> 1);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator<T>(this);

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator<T>(this);

            IEnumerator<T> IReversibleEnumerable<T>.GetReversedEnumerator() => new ReversedEnumerator<T>(this);

            public void Clear()
            {
                int count = _count;
                if (count <= 0)
                    return;
                _count = 0;
                object node = _treeHeadNode;
                int depth = _depth;
                int nodeSize = _nodeSize;
                if (depth <= 0)
                {
                    _depth = 1;
                    _treeHeadNode = new T[nodeSize];
                    return;
                }
                ClearCore(node, depth, nodeSize);
            }

            public bool Contains(T item) => IndexOf(item) >= 0;

            public bool Contains(T item, IEqualityComparer<T> comparer) => IndexOf(item, comparer) >= 0;

            public int IndexOf(T item)
            {
                int count = _count;
                if (count <= 0)
                    return -1;
                object node = _treeHeadNode;
                int depth = _depth;
                int nodeSize = _nodeSize;
                if (depth <= 0 || nodeSize <= 0)
                    return -1;
                int index = IndexOfCore(item, node, depth, nodeSize);
                if (index < 0 || index >= count)
                    return -1;
                return index;
            }

            public int IndexOf(T item, IEqualityComparer<T> comparer)
            {
                int count = _count;
                if (count <= 0)
                    return -1;
                object node = _treeHeadNode;
                int depth = _depth;
                int nodeSize = _nodeSize;
                if (depth <= 0 || nodeSize <= 0)
                    return -1;
                int index;
                if (comparer is EqualityComparer<T> equalityComparer)
                {
                    if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                        index = IndexOfCore(item, node, depth, nodeSize);
                    else
                        index = IndexOfCore(item, node, equalityComparer, depth, nodeSize);
                }
                else
                    index = IndexOfCore(item, node, comparer, depth, nodeSize);
                if (index < 0 || index >= count)
                    return -1;
                return index;
            }

            private unsafe void AppendCore(T item, int index)
            {
                int depth = _depth;
                object headNode = _treeHeadNode;

                if (!TryDecodePath(index, _nodeSize, depth, out ArrayPool<int> pool, out int[]? pathBuffer))
                {
                    // Increase depth
                    depth++;
                    _depth = depth;

                    object[] newHeadNode = new object[_nodeSize];
                    newHeadNode[0] = headNode;
                    _treeHeadNode = newHeadNode;

                    pathBuffer = pool.Rent(depth);
                    SequenceHelper.Clear(pathBuffer, 0, depth);
                    pathBuffer[0] = 1;
                }

                GetNodeReference(pool, pathBuffer) = item;
            }

            private static bool TryDecodePath(int index, int nodeSize, int depth,
                out ArrayPool<int> pool, [NotNullWhen(true)] out int[]? pathBuffer)
            {
                pool = ArrayPool<int>.Shared;
                pathBuffer = pool.Rent(depth);
                for (int i = depth - 1; i >= 0; i--)
                {
                    index = MathHelper.DivRem(index, nodeSize, out int pathIndex);
                    pathBuffer[i] = pathIndex;
                }
                if (index <= 0)
                    return true;
                pool.Return(pathBuffer);
                pathBuffer = null;
                return false;
            }

            private unsafe ref T GetNodeReference(ArrayPool<int> pool, int[] pathBuffer)
            {
                int depth = _depth;
                object headNode = _treeHeadNode;

                try
                {
                    DebugHelper.ThrowIf(depth < 1, "Depth cannot less than 1 in this case");

                    if (depth == 1)
                        return ref GetNodeReference((T[])headNode, pathBuffer[0]);

                    fixed (int* path = pathBuffer)
                        return ref GetNodeReference((object[])headNode, path, depth);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    pool.Return(pathBuffer);
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static ref T GetNodeReference(T[] node, int index) => ref node[index];

            private unsafe ref T GetNodeReference(object[] node, int* path, int depth)
            {
                DebugHelper.ThrowIf(depth <= 1, "Depth cannot less or equals than 1 in this case");

                bool nearLeafNode = depth == 2;
                object childNode = node[*path];
                if (childNode is null)
                {
                    if (nearLeafNode)
                    {
                        T[] newNode = ArrayHelper.CreateUninitializedArray<T>(_nodeSize);
                        node[*path] = newNode;
                        return ref GetNodeReference(newNode, path[1]);
                    }
                    else
                    {
                        object[] newNode = ArrayHelper.CreateUninitializedArray<object>(_nodeSize);
                        node[*path] = newNode;
                        return ref GetNodeReference(newNode, path + 1, depth - 1);
                    }
                }
                if (nearLeafNode)
                    return ref GetNodeReference((T[])childNode, path[1]);

                return ref GetNodeReference((object[])childNode, path + 1, depth - 1);
            }

            private static int IndexOfCore(T item, object node, int depth, int nodeSize)
            {
                if (depth > 1)
                {
                    if (node is not object[] array)
                        return -1;
                    ref object arrayRef = ref array[0];
                    for (int i = 0; i < nodeSize; i++)
                    {
                        int index = IndexOfCore(item, UnsafeHelper.AddTypedOffset(ref arrayRef, i), depth, nodeSize);
                        if (index >= 0)
                            return i * nodeSize + index;
                    }
                }
                if (node is not T[] items)
                    return -1;
                return SequenceHelper.IndexOf(items, item, 0, nodeSize);
            }

            private static int IndexOfCore<TEqualityComparer>(T item, object node, TEqualityComparer comparer, int depth, int nodeSize) where TEqualityComparer : IEqualityComparer<T>
            {
                if (depth > 1)
                {
                    if (node is not object[] array)
                        return -1;
                    ref object arrayRef = ref array[0];
                    for (int i = 0; i < nodeSize; i++)
                    {
                        int index = IndexOfCore(item, UnsafeHelper.AddTypedOffset(ref arrayRef, i), comparer, depth, nodeSize);
                        if (index >= 0)
                            return i * nodeSize + index;
                    }
                }
                if (node is not T[] items)
                    return -1;
                if (comparer is null)
                    return SequenceHelper.IndexOf(items, item, 0, nodeSize);
                else
                    return IndexOfCore(item, items, nodeSize, comparer);
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

            private static void ClearCore(object node, int depth, int nodeSize)
            {
                if (depth > 1)
                {
                    if (node is not object[] array)
                        return;
                    ref object arrayRef = ref array[0];
                    for (int i = 0; i < nodeSize; i++)
                        ClearCore(UnsafeHelper.AddTypedOffset(ref arrayRef, i), depth, nodeSize);
                }
                if (node is not T[] items)
                    return;
                SequenceHelper.Clear(items, 0, nodeSize);
            }
        }
    }
}