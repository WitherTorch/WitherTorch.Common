using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Collections
{
    public delegate void BeforeListAddOrRemoveEventHandler<T>(object? sender, BeforeListAddOrRemoveEventArgs<T> e);
    public delegate void BeforeListModifyEventHandler<T>(object? sender, BeforeListModifyEventArgs<T> e);

    public sealed class ObservableList<T> : ObservableList<IList<T>, T>
    {
        public ObservableList() : base(new List<T>()) { }

        public ObservableList(IList<T> list) : base(list) { }
    }

    public class ObservableList<TList, T> : IList<T>, IReadOnlyList<T> where TList : IList<T>
    {
        private readonly TList _list;
        private readonly bool _isReadOnly;

        public event BeforeListAddOrRemoveEventHandler<T>? BeforeAdd;
        public event BeforeListModifyEventHandler<T>? BeforeInsert;
        public event BeforeListModifyEventHandler<T>? BeforeModify;
        public event BeforeListAddOrRemoveEventHandler<T>? BeforeRemove;
        public event BeforeListModifyEventHandler<T>? BeforeRemoveAt;
        public event CancelEventHandler? BeforeClear;
        public event EventHandler? Updated;

        public ObservableList(TList list)
        {
            _list = list;
            _isReadOnly = list.IsReadOnly;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TList GetUnderlyingList() => _list;

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list[index];
            set
            {
                if (_isReadOnly)
                    return;
                IList<T> list = _list;
                if (index < 0 || index >= list.Count)
                    throw new IndexOutOfRangeException();
                BeforeListModifyEventHandler<T>? handler = BeforeModify;
                if (handler is not null)
                {
                    BeforeListModifyEventArgs<T> args = new BeforeListModifyEventArgs<T>(index, value);
                    handler.Invoke(this, args);
                    if (args.Cancel)
                        return;
                }
                list[index] = value;
                Updated?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list.Count;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isReadOnly;
        }

        public void Add(T item)
        {
            if (_isReadOnly)
                return;

            BeforeListAddOrRemoveEventHandler<T>? handler = BeforeAdd;
            if (handler is not null)
            {
                BeforeListAddOrRemoveEventArgs<T> args = new BeforeListAddOrRemoveEventArgs<T>(item);
                handler.Invoke(this, args);
                if (args.Cancel)
                    return;
            }
            _list.Add(item);

            Updated?.Invoke(this, EventArgs.Empty);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (_isReadOnly)
                return;

            IList<T> list = _list;
            BeforeListAddOrRemoveEventHandler<T>? handler = BeforeAdd;
            if (handler is null)
            {
                switch (list)
                {
                    case List<T> castedList:
                        castedList.AddRange(items);
                        break;
                    case CustomListBase<T> castedList:
                        castedList.AddRange(items);
                        break;
                    default:
                        foreach (T item in items)
                            list.Add(item);
                        break;
                }
            }
            else
            {
                foreach (T item in items)
                {
                    BeforeListAddOrRemoveEventArgs<T> args = new BeforeListAddOrRemoveEventArgs<T>(item);
                    handler.Invoke(this, args);
                    if (args.Cancel)
                        continue;
                    list.Add(item);
                }
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }

        public void Clear()
        {
            if (_isReadOnly)
                return;

            CancelEventHandler? handler = BeforeClear;
            if (handler is not null)
            {
                CancelEventArgs args = new CancelEventArgs();
                handler.Invoke(this, args);
                if (args.Cancel)
                    return;
            }
            _list.Clear();

            Updated?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => _list.Contains(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (_isReadOnly)
                return;

            IList<T> list = _list;
            if (index < 0 || index > list.Count)
                throw new IndexOutOfRangeException();

            BeforeListModifyEventHandler<T>? handler = BeforeInsert;
            if (handler is not null)
            {
                BeforeListModifyEventArgs<T> args = new BeforeListModifyEventArgs<T>(index, item);
                handler.Invoke(this, args);
                if (args.Cancel)
                    return;
            }
            _list.Insert(index, item);

            Updated?.Invoke(this, EventArgs.Empty);
        }

        public bool Remove(T item)
        {
            if (_isReadOnly)
                return false;

            BeforeListAddOrRemoveEventHandler<T>? handler = BeforeRemove;
            if (handler is not null)
            {
                BeforeListAddOrRemoveEventArgs<T> args = new BeforeListAddOrRemoveEventArgs<T>(item);
                handler.Invoke(this, args);
                if (args.Cancel)
                    return false;
            }
            bool result = _list.Remove(item);

            Updated?.Invoke(this, EventArgs.Empty);
            return result;
        }

        public void RemoveAt(int index)
        {
            if (_isReadOnly)
                return;

            IList<T> list = _list;
            if (index < 0 || index > list.Count)
                return;

            BeforeListModifyEventHandler<T>? handler = BeforeRemoveAt;
            if (handler is not null)
            {
                BeforeListModifyEventArgs<T> args = new BeforeListModifyEventArgs<T>(index, list[index]);
                handler.Invoke(this, args);
                if (args.Cancel)
                    return;
            }

            _list.RemoveAt(index);
            Updated?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => _list.ToArray();
    }
}
