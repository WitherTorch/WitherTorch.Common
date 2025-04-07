#if NET472_OR_GREATER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Collections
{
    internal sealed class SharedArrayPool<T> : ArrayPool<T>
    {
        private static readonly SharedArrayPool<T> _instance = new SharedArrayPool<T>();

        private const int LocalArrayQueueCount = 4;
        private const int LocalArraySizeLimit = 1 << (LocalArrayQueueCount + 3);
        private const int GlobalArrayQueueCount = 20 - LocalArrayQueueCount - 3;
        private const int GlobalArraySizeLimit = 1 << 20;

        private sealed class DelayedArray : DelayedCollectingObject
        {
            private readonly uint _capacity;
            private T[]? _array;

            public T[] Array => NullSafetyHelper.ThrowIfNull(_array);

            public DelayedArray(uint capacity)
            {
                _capacity = capacity;
            }

            protected override void DestroyObject()
            {
                _array = null;
            }

            protected override void GenerateObject()
            {
                _array = new T[_capacity];
            }
        }

        private readonly ThreadLocal<Queue<DelayedArray>>[] _smallQueueLocal;
        private readonly Lazy<ConcurrentQueue<DelayedArray>>[] _largeQueueLazy;
        private readonly ThreadLocal<List<DelayedArray>> _restoreListLocal;

        public static SharedArrayPool<T> Instance => _instance;

        private SharedArrayPool()
        {
            ThreadLocal<Queue<DelayedArray>>[] smallQueueLocal = new ThreadLocal<Queue<DelayedArray>>[LocalArrayQueueCount];
            for (int i = 0; i < LocalArrayQueueCount; i++)
            {
                smallQueueLocal[i] = new ThreadLocal<Queue<DelayedArray>>(() => new Queue<DelayedArray>(), false);
            }
            Lazy<ConcurrentQueue<DelayedArray>>[] largeQueueLazy = new Lazy<ConcurrentQueue<DelayedArray>>[GlobalArrayQueueCount];
            for (int i = 0; i < GlobalArrayQueueCount; i++)
            {
                largeQueueLazy[i] = new Lazy<ConcurrentQueue<DelayedArray>>(() => new ConcurrentQueue<DelayedArray>(), LazyThreadSafetyMode.ExecutionAndPublication);
            }
            _smallQueueLocal = smallQueueLocal;
            _largeQueueLazy = largeQueueLazy;
            _restoreListLocal = new ThreadLocal<List<DelayedArray>>(() => new List<DelayedArray>(2), false);
        }

        public override T[] Rent(uint capacity)
        {
            if (capacity == 0)
                return Array.Empty<T>();
            if (capacity <= LocalArraySizeLimit)
                return RentSmall(capacity);
            if (capacity <= GlobalArrayQueueCount)
                return RentLarge(capacity);
            return new T[capacity];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T[] RentSmall(uint capacity)
        {
            int index;
            if (capacity <= MinimumArraySize)
                index = 0;
            else if (capacity <= MinimumArraySize << 1)
                index = 1;
            else if (capacity <= MinimumArraySize << 2)
                index = 2;
            else
                index = 3;
            if (!_smallQueueLocal[index].Value.TryDequeue(out DelayedArray? result) || result is null)
                result = new DelayedArray(MinimumArraySize << index);
            result.AddRef();
            _restoreListLocal.Value.Add(result);
            return result.Array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T[] RentLarge(uint capacity)
        {
            int index = unchecked((int)MathHelper.Log2(capacity - 1 | 15) - 3 - LocalArrayQueueCount);
            if (!_largeQueueLazy[index].Value.TryDequeue(out DelayedArray result))
            {
                result = new DelayedArray(1U << (index + 4 + LocalArrayQueueCount));
            }
            result.AddRef();
            _restoreListLocal.Value.Add(result);
            return result.Array;
        }

        public override void Return(T[] obj, bool clearArray)
        {
            if (obj is null)
                return;
            int length = obj.Length;
            if (length <= 0 || length > GlobalArraySizeLimit)
                return;
            List<DelayedArray> restoreList = _restoreListLocal.Value;
            int count = restoreList.Count;
            if (count <= 0)
                return;
            DelayedArray? array = null;
            int i;
            for (i = 0; i < count; i++)
            {
                DelayedArray item = restoreList[i];
                if (ReferenceEquals(item.Array, obj))
                {
                    array = item;
                    break;
                }
            }
            if (i >= count)
                return;
            restoreList.RemoveAt(i);
            if (clearArray)
                Array.Clear(obj, 0, length);
            if (length <= LocalArraySizeLimit)
            {
                ReturnSmall(array!, obj);
                return;
            }
            ReturnLarge(array!, obj);
            return;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReturnSmall(DelayedArray array, T[] obj)
        {
            int index;
            uint capacity = unchecked((uint)obj.Length);
            switch (capacity)
            {
                case MinimumArraySize:
                    index = 0;
                    break;
                case MinimumArraySize << 1:
                    index = 1;
                    break;
                case MinimumArraySize << 2:
                    index = 2;
                    break;
                case MinimumArraySize << 3:
                    index = 3;
                    break;
                default:
                    return;
            }
            _smallQueueLocal[index].Value.Enqueue(array);
            array.RemoveRef();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReturnLarge(DelayedArray array, T[] obj)
        {
            int index = unchecked((int)MathHelper.Log2((uint)obj.Length - 1 | 15) - 3 - LocalArrayQueueCount);
            if (index < 0)
                return;
            _largeQueueLazy[index].Value.Enqueue(array);
            array.RemoveRef();
        }
    }
}
#endif