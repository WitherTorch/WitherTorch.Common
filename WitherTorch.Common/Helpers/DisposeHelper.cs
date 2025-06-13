﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WitherTorch.Common.Helpers
{
    public static class DisposeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeAll<T>(T?[]? array) where T : IDisposable
        {
            if (array is null)
                return;
            for (int i = 0, length = array.Length; i < length; i++)
                array[i]?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeAllWeak<T>(T[]? array)
        {
            if (array is null)
                return;
            for (int i = 0, length = array.Length; i < length; i++)
                (array[i] as IDisposable)?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDispose<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value = null) where T : class?, IDisposable?
        {
            T? oldObject = location;
            location = value;
            if (ReferenceEquals(oldObject, value) || oldObject is null)
                return;
            oldObject.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeWeak<T>([NotNullIfNotNull(nameof(value))] ref T? location, T? value = null) where T : class?
        {
            T? oldObject = location;
            location = value;
            if (ReferenceEquals(oldObject, value) || oldObject is not IDisposable disposable)
                return;
            disposable.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDispose<T>([NotNullIfNotNull(nameof(value))] ref T[]? location, T[]? value = null) where T : class?, IDisposable?
        {
            T[]? oldObject = location;
            location = value;
            if (ReferenceEquals(oldObject, value) || oldObject is null)
                return;
            for (int i = 0, count = oldObject.Length; i < count; i++)
            {
                oldObject[i]?.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeWeak<T>([NotNullIfNotNull(nameof(value))] ref T[]? location, T[]? value = null) where T : class?
        {
            T[]? oldObject = location;
            location = value;
            if (ReferenceEquals(oldObject, value) || oldObject is null)
                return;
            for (int i = 0, count = oldObject.Length; i < count; i++)
            {
                (oldObject[i] as IDisposable)?.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeInterlocked<T>(ref T? location, T? value = null) where T : class?, IDisposable?
        {
            T? oldObject = Interlocked.Exchange(ref location, value);
            if (ReferenceEquals(oldObject, value) || oldObject is null)
                return;
            oldObject.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeInterlockedWeak<T>(ref T? location, T? value = null) where T : class?
        {
            T? oldObject = Interlocked.Exchange(ref location, value);
            if (ReferenceEquals(oldObject, value) || oldObject is not IDisposable disposable)
                return;
            disposable.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeInterlocked<T>(ref T[]? location, T[]? value = null) where T : class?, IDisposable?
        {
            T[]? disposingObject = Interlocked.Exchange(ref location, value);
            if (disposingObject is null)
                return;
            for (int i = 0, count = disposingObject.Length; i < count; i++)
            {
                disposingObject[i]?.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapDisposeInterlockedWeak<T>(ref T[]? location, T[]? value = null) where T : class?
        {
            T[]? disposingObject = Interlocked.Exchange(ref location, value);
            if (disposingObject is null)
                return;
            for (int i = 0, count = disposingObject.Length; i < count; i++)
            {
                (disposingObject[i] as IDisposable)?.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NullSwapOrDispose<T>(ref T? location, T value) where T : class, IDisposable
        {
            T? oldObject = Interlocked.CompareExchange(ref location, value, null);
            if (oldObject is null || value is null || ReferenceEquals(oldObject, value))
                return;
            value.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NullSwapOrDispose<T>(ref T[]? location, T[] value) where T : class, IDisposable
        {
            T[]? oldObject = Interlocked.CompareExchange(ref location, value, null);
            if (oldObject is null || value is null || ReferenceEquals(oldObject, value))
                return;
            foreach (T item in value)
                item?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NullSwapOrDisposeWeak<T>(ref T? location, T value) where T : class
        {
            T? oldObject = Interlocked.CompareExchange(ref location, value, null);
            if (oldObject is null || ReferenceEquals(oldObject, value))
                return;
            (value as IDisposable)?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NullSwapOrDisposeWeak<T>(ref T[]? location, T[] value) where T : class
        {
            T[]? oldObject = Interlocked.CompareExchange(ref location, value, null);
            if (oldObject is null || ReferenceEquals(oldObject, value))
                return;
            foreach (T item in value)
                (item as IDisposable)?.Dispose();
        }
    }
}
