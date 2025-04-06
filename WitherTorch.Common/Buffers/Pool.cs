using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Buffers
{
    public sealed class Pool<T> : IPool<T>, IDisposable where T : class, new()
    {
        private static readonly bool _needDisposing = typeof(IDisposable).IsAssignableFrom(typeof(T));
        private readonly ConcurrentBag<T> _bag;

        private bool disposedValue;

        public Pool(int initialLength)
        {
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

        private void DisposeCore()
        {
            if (disposedValue)
                return;
            disposedValue = true;
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

        // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~Pool()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            DisposeCore();
        }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            DisposeCore();
            GC.SuppressFinalize(this);
        }
    }
}
