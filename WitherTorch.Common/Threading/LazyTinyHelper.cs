using System;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    internal static class LazyTinyHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T InitializeAndReturn<T>(ref T? location, Func<T>? factory, bool threadSafe, object? syncLock) where T : class
        {
            T? result;
            if (!threadSafe) // 對應 LazyThreadSafetyMode.None
            {
                result = InitializeOrThrow(factory);
                location = result;
                return result;
            }
            if (syncLock is null) // 對應 LazyThreadSafetyMode.PublicationOnly
            {
                result = Volatile.Read(ref location);
                if (result is null)
                {
                    result = InitializeOrThrow(factory);
                    T? oldResult = Interlocked.CompareExchange(ref location, result, null);
                    if (oldResult is not null)
                    {
                        (result as IDisposable)?.Dispose();
                        result = oldResult;
                    }
                }
                return result;
            }
            // 對應 LazyThreadSafetyMode.ExecutionAndPublication
            Monitor.Enter(syncLock);
            result = location;
            if (result is null)
            {
                try
                {
                    result = InitializeOrThrow(factory);
                }
                catch (Exception)
                {
                    Monitor.Exit(syncLock);
                    throw;
                }
                location = result;
            }
            Monitor.Exit(syncLock);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InitializeOrThrow<T>(Func<T>? factory) where T : class
        {
            if (factory is null)
                return Activator.CreateInstance<T>();
            return NullSafetyHelper.ThrowIfNull(factory.Invoke());
        }
    }
}
