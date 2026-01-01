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
            private static readonly int DefaultNodeSize;
            private readonly int _nodeSize;

            private object _treeHeadNode;
            private int _count, _depth;

            static unsafe UnlimitedAOCollection()
            {
#pragma warning disable CS8500
                DefaultNodeSize = Environment.SystemPageSize / sizeof(T);
#pragma warning restore CS8500
            }

            public UnlimitedAOCollection() : this(DefaultNodeSize) { }

            public UnlimitedAOCollection(int nodeSize)
            {
                _nodeSize = nodeSize;
                _treeHeadNode = new T[nodeSize];
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
                int depth = _depth;
                object headNode = _treeHeadNode;

                if (!TryDecodePath(count, _nodeSize, depth, out ArrayPool<int>? pool, out int[]? pathBuffer))
                {
                    // Increase depth
                    depth++;
                    _depth = depth;

                    object[] newHeadNode = new object[_nodeSize];
                    newHeadNode[0] = headNode;
                    _treeHeadNode = newHeadNode;

                    pathBuffer = pool.Rent(depth);
                    Array.Clear(pathBuffer, 0, depth);
                    pathBuffer[0] = 1;
                }

                GetNodeReference(pool, pathBuffer) = item;

                _count = count + 1;
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

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

            public void Clear()
            {
                _treeHeadNode = new T[_nodeSize];
                _depth = 1;
                _count = 0;
                GC.Collect();
            }

#pragma warning disable CS8500
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe TResult GetValueFromPointerSpecial<TResult>(TResult* ptr, TResult valueForMinPtr, TResult valueForMaxPtr)
            {
                if (ptr == UnsafeHelper.PointerMinValue)
                    return valueForMinPtr;
                if (ptr == UnsafeHelper.PointerMaxValue)
                    return valueForMaxPtr;
                return *ptr;
            }
#pragma warning restore CS8500

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
                        T[] newNode = new T[_nodeSize];
                        node[*path] = newNode;
                        return ref GetNodeReference(newNode, path[1]);
                    }
                    else
                    {
                        object[] newNode = new object[_nodeSize];
                        node[*path] = newNode;
                        return ref GetNodeReference(newNode, path + 1, depth - 1);
                    }
                }
                if (nearLeafNode)
                    return ref GetNodeReference((T[])childNode, path[1]);

                return ref GetNodeReference((object[])childNode, path + 1, depth - 1);
            }

            private sealed class Enumerator : IEnumerator<T>
            {
                private readonly UnlimitedAOCollection<T> _collection;
                private int _index;

                public Enumerator(UnlimitedAOCollection<T> list)
                {
                    _collection = list;
                    _index = -1;
                }

                public T Current => _collection[_index];

                object? IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (_index + 1 < _collection.Count)
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