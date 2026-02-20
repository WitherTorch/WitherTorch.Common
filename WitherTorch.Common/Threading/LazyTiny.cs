using System;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    /// <inheritdoc cref="Lazy{T}"/>
    public sealed class LazyTiny<T> where T : class
    {
        private readonly bool _isThreadSafe;
        private readonly Func<T>? _factory;
        private readonly object? _syncRoot;

        private T? _value;

        /// <inheritdoc cref="Lazy{T}.Lazy(LazyThreadSafetyMode)"/>
        public LazyTiny(bool isThreadSafe) : this(null, isThreadSafe) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(LazyThreadSafetyMode)"/>
        public LazyTiny(LazyThreadSafetyMode mode) : this(null, mode) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T})"/>
        public LazyTiny(Func<T>? factory) : this(factory, false, null) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T}, bool)"/>
        public LazyTiny(Func<T>? factory, bool isThreadSafe) : this(factory, isThreadSafe, isThreadSafe ? new object() : null) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T}, LazyThreadSafetyMode)"/>
        public LazyTiny(Func<T>? factory, LazyThreadSafetyMode mode) :
            this(factory, mode != LazyThreadSafetyMode.None, mode == LazyThreadSafetyMode.ExecutionAndPublication ? new object() : null)
        { }

        public LazyTiny(T value)
        {
            _isThreadSafe = true;
            _syncRoot = null;
            _factory = null;
            _value = value;
        }

        private LazyTiny(Func<T>? factory, bool isThreadSafe, object? syncRoot)
        {
            _isThreadSafe = isThreadSafe;
            _factory = factory;
            _syncRoot = syncRoot;
            _value = null;
        }

        /// <inheritdoc cref="Lazy{T}.IsValueCreated"/>
        public bool IsValueCreated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value is object;
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T? result;
                if (_isThreadSafe)
                    result = InterlockedHelper.Read(ref _value);
                else
                    result = _value;
                return result ?? LazyTinyHelper.InitializeAndReturn(ref _value, _factory, _isThreadSafe, _syncRoot);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? GetValueDirectly() => _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            if (_isThreadSafe)
                DisposeHelper.SwapDisposeInterlockedWeak(ref _value);
            else
                DisposeHelper.SwapDisposeWeak(ref _value);
        }

        ~LazyTiny()
        {
            (_value as IDisposable)?.Dispose();
        }
    }
}
