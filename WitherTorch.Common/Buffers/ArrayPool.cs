using System;
using System.Runtime.CompilerServices;
using System.Threading;

using InlineMethod;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Buffers
{
    public abstract partial class ArrayPool<T> : IPool<T[]>
    {
        protected const uint MinimumArraySize = 16;

        private static readonly LazyTiny<ArrayPool<T>> _sharedLazy = new LazyTiny<ArrayPool<T>>(CreateSharedPool, LazyThreadSafetyMode.ExecutionAndPublication);

        public static ArrayPool<T> Shared => _sharedLazy.Value;

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent() => Rent(MinimumArraySize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            return Rent(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(long capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            return Rent(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(nint capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            return Rent(unchecked((nuint)capacity));
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent(uint capacity) => Rent(unchecked((nuint)capacity));

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent(ulong capacity) => Rent(unchecked((nuint)capacity));

        public abstract T[] Rent(nuint capacity);

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

        private static ArrayPool<T> CreateSharedPool()
        {
            if (WTCommon.SystemBuffersExists)
                return UnsafeCreateWrappedSystemArrayPool();
            return new SharedArrayPoolImpl();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static SystemArrayPoolWrapper UnsafeCreateWrappedSystemArrayPool()
        {
            System.Buffers.ArrayPool<T> pool = System.Buffers.ArrayPool<T>.Shared;
            return new SystemArrayPoolWrapper(pool);
        }
    }
}