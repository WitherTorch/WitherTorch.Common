using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Collections
{
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
                return _array[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                _array[index] = value;
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
            Array.Copy(_array, arrayIndex, array, 0, _count - arrayIndex);
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
            }
            else
            {
                _count = newCount;
                EnsureCapacity();
                MoveArray(_array, index, index + 1, oldCount);
                _array[index] = item;
            }
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
                MoveArray(_array, index + 1, index, oldCount);
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
            if (count == 0)
                return _array;
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
            {
                if (typeof(T) == typeof(sbyte))
                    return SequenceHelper.IndexOf((sbyte*)ptr, (sbyte*)ptr + count, UnsafeHelper.As<T, sbyte>(item));
                if (typeof(T) == typeof(byte))
                    return SequenceHelper.IndexOf((byte*)ptr, (byte*)ptr + count, UnsafeHelper.As<T, byte>(item));
                if (typeof(T) == typeof(short))
                    return SequenceHelper.IndexOf((short*)ptr, (short*)ptr + count, UnsafeHelper.As<T, short>(item));
                if (typeof(T) == typeof(ushort))
                    return SequenceHelper.IndexOf((ushort*)ptr, (ushort*)ptr + count, UnsafeHelper.As<T, ushort>(item));
                if (typeof(T) == typeof(char))
                    return SequenceHelper.IndexOf((char*)ptr, (char*)ptr + count, UnsafeHelper.As<T, char>(item));
                if (typeof(T) == typeof(int))
                    return SequenceHelper.IndexOf((int*)ptr, (int*)ptr + count, UnsafeHelper.As<T, int>(item));
                if (typeof(T) == typeof(uint))
                    return SequenceHelper.IndexOf((uint*)ptr, (uint*)ptr + count, UnsafeHelper.As<T, uint>(item));
                if (typeof(T) == typeof(long))
                    return SequenceHelper.IndexOf((long*)ptr, (long*)ptr + count, UnsafeHelper.As<T, long>(item));
                if (typeof(T) == typeof(ulong))
                    return SequenceHelper.IndexOf((ulong*)ptr, (ulong*)ptr + count, UnsafeHelper.As<T, ulong>(item));
                if (typeof(T) == typeof(float))
                    return SequenceHelper.IndexOf((float*)ptr, (float*)ptr + count, UnsafeHelper.As<T, float>(item));
                if (typeof(T) == typeof(double))
                    return SequenceHelper.IndexOf((double*)ptr, (double*)ptr + count, UnsafeHelper.As<T, double>(item));
                if (typeof(T) == typeof(nint))
                    return SequenceHelper.IndexOf((nint*)ptr, (nint*)ptr + count, UnsafeHelper.As<T, nint>(item));
                if (typeof(T) == typeof(nuint))
                    return SequenceHelper.IndexOf((nuint*)ptr, (nuint*)ptr + count, UnsafeHelper.As<T, nuint>(item));
                if (typeof(T) == typeof(decimal))
                    return SequenceHelper.IndexOf((decimal*)ptr, (decimal*)ptr + count, UnsafeHelper.As<T, decimal>(item));
            }
            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsCoreFast(T[] array, int count, T item)
        {
            fixed (T* ptr = array)
            {
                if (typeof(T) == typeof(sbyte))
                    return SequenceHelper.Contains((sbyte*)ptr, (sbyte*)ptr + count, UnsafeHelper.As<T, sbyte>(item));
                if (typeof(T) == typeof(byte))
                    return SequenceHelper.Contains((byte*)ptr, (byte*)ptr + count, UnsafeHelper.As<T, byte>(item));
                if (typeof(T) == typeof(short))
                    return SequenceHelper.Contains((short*)ptr, (short*)ptr + count, UnsafeHelper.As<T, short>(item));
                if (typeof(T) == typeof(ushort))
                    return SequenceHelper.Contains((ushort*)ptr, (ushort*)ptr + count, UnsafeHelper.As<T, ushort>(item));
                if (typeof(T) == typeof(char))
                    return SequenceHelper.Contains((char*)ptr, (char*)ptr + count, UnsafeHelper.As<T, char>(item));
                if (typeof(T) == typeof(int))
                    return SequenceHelper.Contains((int*)ptr, (int*)ptr + count, UnsafeHelper.As<T, int>(item));
                if (typeof(T) == typeof(uint))
                    return SequenceHelper.Contains((uint*)ptr, (uint*)ptr + count, UnsafeHelper.As<T, uint>(item));
                if (typeof(T) == typeof(long))
                    return SequenceHelper.Contains((long*)ptr, (long*)ptr + count, UnsafeHelper.As<T, long>(item));
                if (typeof(T) == typeof(ulong))
                    return SequenceHelper.Contains((ulong*)ptr, (ulong*)ptr + count, UnsafeHelper.As<T, ulong>(item));
                if (typeof(T) == typeof(float))
                    return SequenceHelper.Contains((float*)ptr, (float*)ptr + count, UnsafeHelper.As<T, float>(item));
                if (typeof(T) == typeof(double))
                    return SequenceHelper.Contains((double*)ptr, (double*)ptr + count, UnsafeHelper.As<T, double>(item));
                if (typeof(T) == typeof(nint))
                    return SequenceHelper.Contains((nint*)ptr, (nint*)ptr + count, UnsafeHelper.As<T, nint>(item));
                if (typeof(T) == typeof(nuint))
                    return SequenceHelper.Contains((nuint*)ptr, (nuint*)ptr + count, UnsafeHelper.As<T, nuint>(item));
                if (typeof(T) == typeof(decimal))
                    return SequenceHelper.Contains((decimal*)ptr, (decimal*)ptr + count, UnsafeHelper.As<T, decimal>(item));
            }
            throw new InvalidOperationException();
        }
#pragma warning restore CS8500

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IndexOfCoreSlow(T[] array, int count, T item)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
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
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; i++)
            {
                if (comparer.Equals(array[i], item))
                    return true;
            }
            return false;
        }

        [Inline(InlineBehavior.Remove)]
        private static void MoveArray(T[] array, int oldIndex, int newIndex, int oldArraySize)
        {
            int indexDiff = newIndex - oldIndex;
            if (indexDiff == 0)
                return;
            else if (indexDiff > 0)
            {
                for (int i = oldArraySize - 1, j = i + indexDiff; i >= oldIndex; i--, j--)
                {
                    array[j] = array[i];
                    array[i] = default!;
                }
            }
            else
            {
                for (int i = oldIndex, j = newIndex; i < oldArraySize; i++, j++)
                {
                    array[j] = array[i];
                    array[i] = default!;
                }
            }
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
