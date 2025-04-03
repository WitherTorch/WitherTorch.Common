using System;
using System.Threading;

using WitherTorch.Common;

namespace WitherTorch.Common.Internals
{
    internal sealed class FallbackArrayPoolProvider : IArrayPoolProvider
    {
        private static readonly FallbackArrayPoolProvider _instance = new FallbackArrayPoolProvider();

        public static FallbackArrayPoolProvider Instance => _instance;

        private FallbackArrayPoolProvider() { }

        public IArrayPool<T> GetArrayPool<T>() => Pool<T>.Instance;

        private sealed class Pool<T> : IArrayPool<T>
        {
            private static readonly Pool<T> _instance = new Pool<T>();
            private static readonly bool _needClearArray = typeof(T).IsClass;

            private readonly ThreadLocal<WeakReference<T[]>?> _localCachedArray;

            public static Pool<T> Instance => _instance;

            private Pool()
            {
                _localCachedArray = new ThreadLocal<WeakReference<T[]>?>(static () => default, false);
            }

            public T[] Rent(int length)
            {
                WeakReference<T[]>? reference = _localCachedArray.Value;
                if (reference is null || !reference.TryGetTarget(out T[]? value) || value is null)
                    return new T[length];
                _localCachedArray.Value = null;
                if (value.Length < length)
                    return new T[length];
                return value;
            }

            public void Return(T[] array)
            {
                if (_needClearArray)
                    Array.Clear(array, 0, array.Length);
                WeakReference<T[]>? reference = _localCachedArray.Value;
                if (reference is null || !reference.TryGetTarget(out T[]? value) || value is null || value.Length < array.Length)
                    _localCachedArray.Value = new WeakReference<T[]>(array);
            }
        }
    }
}
