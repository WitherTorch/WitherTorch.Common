using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Buffers
{
    public sealed class Pool<T> : IPool<T>, IDisposable where T : class, new()
    {
        private static readonly bool _needDisposing = typeof(IDisposable).IsAssignableFrom(typeof(T));
        private readonly ConcurrentBag<T> _bag;

        private bool _disposed;

        public Pool(int initialLength)
        {
            _disposed = false;
            if (initialLength > 0)
            {
                T[] array = new T[initialLength];
                for (int i = 0; i < initialLength; i++)
                    array[i] = new T();
                _bag = new ConcurrentBag<T>(array);
            }
            else
            {
                _bag = new ConcurrentBag<T>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Rent()
        {
            if (_bag.TryTake(out T? result))
                return result;
            return new T();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T obj)
        {
            _bag.Add(obj);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            if (disposing)
            {
                if (!_needDisposing)
                    return;
                ConcurrentBag<T> bag = _bag;
                while (bag.TryTake(out T? obj))
                {
                    if (obj is null)
                        continue;
                    ((IDisposable)obj).Dispose();
                }
            }
            else
            {
#if NET8_0_OR_GREATER
                _bag.Clear();
#endif
            }
        }

        ~Pool()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }
    }
}
