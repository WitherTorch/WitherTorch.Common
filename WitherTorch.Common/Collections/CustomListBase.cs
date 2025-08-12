using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Collections
{
#pragma warning disable CS8500
    public abstract unsafe class CustomListBase<T> : IList<T>, IReadOnlyList<T>
    {
        // Standard List Structure
        protected int _count;
        protected T[] _array;

        public CustomListBase(T[] array)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            _array = array;
            _count = array.Length;
        }

        public CustomListBase(T[] array, int initialCount)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            _array = array;
            _count = MathHelper.Clamp(initialCount, 0, array.Length);
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                return UnsafeHelper.AddByteOffset(ref _array[0], UnsafeHelper.SizeOf<T>() * unchecked((nuint)index)); // 忽略邊界檢查
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                UnsafeHelper.AddByteOffset(ref _array[0], UnsafeHelper.SizeOf<T>() * unchecked((nuint)index)) = value; // 忽略邊界檢查
            }
        }

        public int Count => _count;

        public int Capacity => _array.Length;

        public bool IsReadOnly => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            int index = _count++;
            EnsureCapacity();
            _array[index] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(T[] array)
        {
            int length = array.Length;
            int index = _count;
            _count = index + length;
            EnsureCapacity();
            T[] _array = this._array;
            for (int i = 0, j = index; i < length; i++, j++)
            {
                _array[j] = array[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(IList<T> array)
        {
            int length = array.Count;
            int index = _count;
            _count = index + length;
            EnsureCapacity();
            T[] _array = this._array;
            for (int i = 0, j = index; i < length; i++, j++)
            {
                _array[j] = array[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(IEnumerable<T> items)
        {
            switch (items)
            {
                case T[] array:
                    AddRange(array);
                    return;
                case IList<T> list:
                    AddRange(list);
                    return;
                default:
                    IEnumerator<T> enumerator = items.GetEnumerator();
                    while (enumerator.MoveNext())
                        Add(enumerator.Current);
                    enumerator.Dispose();
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(T* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0)
                return;

            int index = _count;
            _count = index + count;
            EnsureCapacity();
            T[] array = _array;
            fixed (T* destination = array)
                UnsafeHelper.CopyBlockUnaligned(destination + index, ptr + startIndex, (uint)count * UnsafeHelper.SizeOf<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(T* ptr, nuint startIndex, nuint count)
        {
            if (count == 0)
                return;

            nuint index = MathHelper.MakeUnsigned(_count);
            _count = unchecked((int)MathHelper.MakeSigned(index + count));
            EnsureCapacity();
            T[] array = _array;
            fixed (T* destination = array)
                UnsafeHelper.CopyBlockUnaligned(destination + index, ptr + startIndex, count * UnsafeHelper.SizeOf<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinarySearch(T item) => Array.BinarySearch(_array, 0, _count, item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinarySearchForNextGreaterOrEquals(T item)
        {
            int index = BinarySearch(item);
            return index < 0 ? ~index : index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinarySearchForNextLessOrEquals(T item)
        {
            int index = BinarySearch(item);
            return index < 0 ? (~index) - 1 : index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            _count = 0;
            if (UnsafeHelper.IsUnmanagedType<T>())
                return;
            SequenceHelper.Clear(_array, 0, _count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            if (UnsafeHelper.IsPrimitiveType<T>())
                return ContainsCoreFast(_array, _count, item);
            return ContainsCoreSlow(_array, _count, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_array, 0, array, arrayIndex, _count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            int count = _count;
            if (count <= 0)
                return -1;
            if (UnsafeHelper.IsPrimitiveType<T>())
                return IndexOfCoreFast(_array, count, item);
            return IndexOfCoreSlow(_array, count, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            int oldCount = _count;
            int newCount = oldCount + 1;
            index = MathHelper.Clamp(index, 0, oldCount);
            if (index == oldCount)
            {
                Add(item);
                return;
            }
            _count = newCount;
            EnsureCapacity();
            T[] array = _array;
            MoveArray(array, index, index + 1, oldCount - index);
            array[index] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            int oldCount = _count;
            int newCount = oldCount - 1;
            if (index < 0 || index >= oldCount)
                return;
            _count = newCount;
            _array[index] = default!;
            if (index < newCount)
            {
                MoveArray(_array, index + 1, index, oldCount - index - 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(_array, _count);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? Find(Predicate<T> predicate)
        {
            if (predicate is null)
                return default;
            T[] array = _array;
            for (int i = 0, count = _count; i < count; i++)
            {
                T obj = array[i];
                if (predicate(obj))
                    return obj;
            }
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sort() => Array.Sort(_array, 0, _count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sort(IComparer<T>? comparer) => Array.Sort(_array, 0, _count, comparer);

        public virtual T[] ToArray()
        {
            int count = _count;
            if (count <= 0)
                return Array.Empty<T>();
            T[] array = new T[count];
            Array.Copy(_array, array, count);
            return array;
        }

        protected abstract void EnsureCapacity();

#pragma warning disable CS8500
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IndexOfCoreFast(T[] array, int count, T item)
        {
            fixed (T* ptr = array)
                return SequenceHelper.IndexOf(ptr, count, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCoreFast(T[] array, int count, T item)
        {
            fixed (T* ptr = array)
                return SequenceHelper.Contains(ptr, MathHelper.MakeUnsigned(count), item);
        }
#pragma warning restore CS8500

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IndexOfCoreSlow(T[] array, int count, T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; i++)
            {
                if (comparer.Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCoreSlow(T[] array, int count, T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; i++)
            {
                if (comparer.Equals(array[i], item))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveArray(T[] array, int oldIndex, int newIndex, int count)
        {
            if (newIndex == oldIndex || count <= 0)
                return;
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                MoveArrayFast(array, oldIndex, newIndex, count);
                return;
            }
            MoveArraySlow(array, oldIndex, newIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveArrayFast(T[] array, int oldIndex, int newIndex, int count)
        {
#pragma warning disable CS8500
            fixed (T* ptr = array)
            {
                if (MathHelper.Abs(oldIndex - newIndex) < count)
                {
                    NativeMethods.MoveMemory(ptr + newIndex, ptr + oldIndex, count * sizeof(T));
                    return;
                }
                NativeMethods.CopyMemory(ptr + newIndex, ptr + oldIndex, count * sizeof(T));
            }
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveArraySlow(T[] array, int oldIndex, int newIndex, int count)
        {
            Array.Copy(array, oldIndex, array, newIndex, count);
            if (newIndex < oldIndex)
                Array.Clear(array, newIndex + count, oldIndex - newIndex);
            else
                Array.Clear(array, oldIndex, newIndex - oldIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveArrayCoreSlow(T[] array, int oldIndex, int newIndex, int count)
        {
            Array.Copy(array, oldIndex, array, newIndex, count);
#pragma warning disable CS8500
            fixed (T* ptr = array)
                UnsafeHelper.InitBlock(ptr + oldIndex, 0, unchecked((uint)(count * sizeof(T))));
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveArrayCoreVerySlow(T[] array, int oldIndex, int newIndex, int count)
        {
            for (int i = oldIndex + count - 1, j = newIndex + count - 1; i >= oldIndex; i--, j--)
                (array[j], array[i]) = (array[i], default!);
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _count;

            private int _index;

            public Enumerator(T[] array, int count)
            {
                _array = array;
                _count = count;
                _index = -1;
            }

            public readonly T Current
            {
                get
                {
                    int index = _index;
                    if (index < 0 || index >= _count)
                        throw new InvalidOperationException();
                    return _array[index];
                }
            }

            readonly object? IEnumerator.Current => Current;

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                int newIndex = _index + 1;
                if (newIndex < _count)
                {
                    _index = newIndex;
                    return true;
                }
                else
                {
                    _index = _count;
                    return false;
                }
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }

}
