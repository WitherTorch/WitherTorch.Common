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

        public static ArrayPool<T> Empty => EmptyArrayPoolImpl.Instance;
        public static ArrayPool<T> Shared => _sharedLazy.Value;

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent() => Rent(MinimumArraySize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(long capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(nint capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(uint capacity)
        {
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > (uint)Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(ulong capacity)
        {
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > (ulong)Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(unchecked((nuint)capacity));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Rent(nuint capacity)
        {
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity > (nuint)Limits.MaxArrayLength)
                return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
            return RentCore(capacity);
        }

        protected abstract T[] RentCore(nuint capacity);

        public unsafe void Return(T[] array)
        {
            int length = array.Length;
            if (length <= 0)
                return;
            if (!UnsafeHelper.IsUnmanagedType<T>())
            {
#pragma warning disable CS8500
                fixed (void* ptr = array)
                    UnsafeHelper.InitBlock(ptr, 0, unchecked((uint)length * UnsafeHelper.SizeOf<T>()));
#pragma warning restore CS8500
            }
            ReturnCore(array);
        }

        public unsafe void Return(T[] array, bool clearArray = false)
        {
            int length = array.Length;
            if (length <= 0)
                return;
            if (clearArray)
            {
#pragma warning disable CS8500
                fixed (void* ptr = array)
                    UnsafeHelper.InitBlock(ptr, 0, unchecked((uint)length * UnsafeHelper.SizeOf<T>()));
#pragma warning restore CS8500
            }
            ReturnCore(array);
        }

        protected abstract void ReturnCore(T[] array);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList() => new FixedArrayList<T>(Rent(), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList(int capacity) => new FixedArrayList<T>(Rent(capacity), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list) => Return(list.AsArray());

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list, bool clearArray) => Return(list.AsArray(), clearArray);

        private static partial ArrayPool<T> CreateSharedPool();
    }
}