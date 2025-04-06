using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Collections
{
    public sealed class BeforeListAddOrRemoveEventArgs<T> : CancelEventArgs
    {
        private readonly T _item;

        public T Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _item;
        }

        public BeforeListAddOrRemoveEventArgs(T item)
        {
            _item = item;
        }
    }

    public sealed class BeforeListModifyEventArgs<T> : CancelEventArgs
    {
        private readonly T _item;
        private readonly int _index;

        public int Index
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _index;
        }

        public T Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _item;
        }

        public BeforeListModifyEventArgs(int index, T item)
        {
            _index = index;
            _item = item;
        }
    }
}