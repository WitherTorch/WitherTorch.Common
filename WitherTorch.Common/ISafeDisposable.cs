using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common
{
    public interface ISafeDisposable : IDisposable
    {
        bool IsDisposed { get; }

        bool MarkAsDisposed();

        void DisposeCore(bool disposing);

#if NET8_0_OR_GREATER
        void IDisposable.Dispose() => SafeDisposableDefaults.Dispose(this);
#endif
    }

    public interface ISafeDisposable<TState> : IDisposable where TState : Enum
    {
        bool IsDisposed { get; }

        TState MarkAsDisposed();

        void DisposeCore(TState oldState, bool disposing);
    }

    public static class SafeDisposableDefaults
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dispose<T>(T disposable) where T : ISafeDisposable
        {
            if (disposable.MarkAsDisposed())
            {
                GC.SuppressFinalize(disposable);
                return;
            }
            try
            {
                disposable.DisposeCore(disposing: true);
            }
            finally
            {
                GC.SuppressFinalize(disposable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dispose<T, TState>(T disposable, TState disposedState) where T : ISafeDisposable<TState> where TState : Enum
        {
            TState oldState = disposable.MarkAsDisposed();
            if (UnsafeHelper.Equals(oldState, disposedState))
            {
                GC.SuppressFinalize(disposable);
                return;
            }
            try
            {
                disposable.DisposeCore(oldState, disposing: true);
            }
            finally
            {
                GC.SuppressFinalize(disposable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Finalize<T>(T disposable) where T : ISafeDisposable
        {
            if (disposable.MarkAsDisposed())
                return;
            disposable.DisposeCore(disposing: false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Finalize<T, TState>(T disposable, TState disposedState) where T : ISafeDisposable<TState> where TState : Enum
        {
            TState oldState = disposable.MarkAsDisposed();
            if (UnsafeHelper.Equals(oldState, disposedState))
                return;
            disposable.DisposeCore(oldState, disposing: false);
        }
    }
}
