using InlineMethod;

using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Buffers
{
    public abstract partial class ArrayPool<T> : IPool<T[]>
    {
        protected const uint MinimumArraySize = 16;

        public static ArrayPool<T> Shared => SharedArrayPool<T>.Instance;

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent() => Rent(MinimumArraySize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            return Rent(capacity);
        }

        public abstract T[] Rent(uint capacity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T[] obj) => Return(obj, !UnsafeHelper.IsUnmanagedType<T>());

        public abstract void Return(T[] obj, bool clearArray);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList() => new FixedArrayList<T>(Rent(), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList(int capacity) => new FixedArrayList<T>(Rent(capacity), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list) => Return(list.AsArray());

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list, bool clearArray) => Return(list.AsArray(), clearArray);
    }
}