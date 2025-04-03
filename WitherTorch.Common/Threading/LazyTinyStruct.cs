using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace WitherTorch.Common.Threading
{
    /// <inheritdoc cref="Lazy{T}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct LazyTinyStruct<T> where T : class
    {
        private readonly bool _isThreadSafe;
        private readonly Func<T>? _factory;
        private readonly object? _syncRoot;

        private T? _value;

        /// <inheritdoc cref="Lazy{T}.Lazy(LazyThreadSafetyMode)"/>
        public LazyTinyStruct(bool isThreadSafe) : this(null, isThreadSafe) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(LazyThreadSafetyMode)"/>
        public LazyTinyStruct(LazyThreadSafetyMode mode) : this(null, mode) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T})"/>
        public LazyTinyStruct(Func<T>? factory) : this(factory, false, null) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T}, bool)"/>
        public LazyTinyStruct(Func<T>? factory, bool isThreadSafe) : this(factory, isThreadSafe, isThreadSafe ? new object() : null) { }

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T}, LazyThreadSafetyMode)"/>
        public LazyTinyStruct(Func<T>? factory, LazyThreadSafetyMode mode) :
            this(factory, mode != LazyThreadSafetyMode.None, mode == LazyThreadSafetyMode.ExecutionAndPublication ? new object() : null)
        { }

        private LazyTinyStruct(Func<T>? factory, bool isThreadSafe, object? syncRoot)
        {
            _isThreadSafe = isThreadSafe;
            _factory = factory;
            _syncRoot = syncRoot;
            _value = null;
        }

        /// <inheritdoc cref="Lazy{T}.IsValueCreated"/>
        public readonly bool IsValueCreated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value is object;
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value ?? LazyTinyHelper.InitializeAndReturn(ref _value, _factory, _isThreadSafe, _syncRoot);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T? GetValueDirectly() => _value;
    }
}
