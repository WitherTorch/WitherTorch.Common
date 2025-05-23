﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Threading
{
    /// <inheritdoc cref="Lazy{T}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public ref struct LazyTinyRefStruct<T> where T : class
    {
        private readonly Func<T>? _factory;

        private T? _value;

        /// <inheritdoc cref="Lazy{T}.Lazy(Func{T})"/>
        public LazyTinyRefStruct(Func<T>? factory)
        {
            _factory = factory;
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
            get => _value ?? LazyTinyHelper.InitializeAndReturn(ref _value, _factory, false, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T? GetValueDirectly() => _value;
    }
}
