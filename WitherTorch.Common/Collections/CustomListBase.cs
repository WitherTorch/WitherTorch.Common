using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Collections
{
    public abstract class CustomListBase<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private static readonly byte _listType;
        // Standard List Structure
        protected int _count;
        protected T[] _array;

        static CustomListBase()
        {
            Type type = typeof(T);
            if (type.IsPrimitive || type.IsEnum)
            {
                _listType = 0;
                return;
            }
            if (type.IsValueType)
            {
                _listType = 1;
                return;
            }
            _listType = 2;
            return;
        }

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
            if (_listType == 2)
                Array.Clear(_array, 0, _count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_array, arrayIndex, array, 0, _count - arrayIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            switch (_listType)
            {
                case 0: //primitive or enum
                    return IndexOfForPrimitive(_array, _count, item);
                case 1: //structures
                    return IndexOfForValueType(_array, _count, item);
                default:
                    break;
            }
            return IndexOfForObject(_array, _count, item);
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
        private static unsafe int IndexOfForPrimitive(T[] array, int count, T item)
        {
            fixed (T* ptr = array)
            {
                T* iterator = ptr;
                int i = 0;
                IL.MarkLabel("LoopStart");
                IL.Push(i);
                IL.Push(count);
                IL.Emit.Bge_S("LoopEnd");
                T iteratedItem = *iterator;
                IL.EnsureLocal(iteratedItem);
                IL.Push(iteratedItem);
                IL.Push(item);
                IL.Emit.Beq_S("ReturnVal");
                iterator++;
                i++;
                IL.Emit.Br("LoopStart");
                IL.MarkLabel("ReturnVal");
                IL.Push(i);
                IL.Emit.Ret();
                IL.MarkLabel("LoopEnd");
            }
            return -1;
        }
#pragma warning restore CS8500

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IndexOfForValueType(T[] array, int count, T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; i++)
            {
                if (comparer.Equals(array[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IndexOfForObject(T[] array, int count, T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (Equals(array[i], item))
                {
                    return i;
                }
            }
            return -1;
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
