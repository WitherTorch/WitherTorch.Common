using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WitherTorch.Common.Threading
{
    internal static class LazyTinyHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T InitializeAndReturn<T>(ref T? location, Func<T>? factory, bool threadSafe, object? syncRoot) where T : class
        {
            T result;
            if (!threadSafe)
            {
                result = InitializeOrThrow(factory);
                location = result;
                return result;
            }
            if (syncRoot is null)
            {
                result = InitializeOrThrow(factory);
                T? oldValue = Interlocked.CompareExchange(ref location, result, null);
                if (oldValue is null)
                    return result;
                (result as IDisposable)?.Dispose();
                return oldValue;
            }
            Monitor.Enter(syncRoot);
            try
            {
                result = InitializeOrThrow(factory);
            }
            catch (Exception)
            {
                Monitor.Exit(syncRoot);
                throw;
            }
            Monitor.Exit(syncRoot);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InitializeOrThrow<T>(Func<T>? factory) where T : class
        {
            if (factory is null)
                return Activator.CreateInstance<T>();
            T result = factory.Invoke();
            if (result is null)
                throw new InvalidOperationException();
            return result;
        }
    }
}
