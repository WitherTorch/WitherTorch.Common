using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions;

public static class ArrayPoolExtensions
{
    public static ArrayPool<T>.RentScope EnterRentScopeAndCapture<T, TEnumerable>(this ArrayPool<T> _this, TEnumerable enumerable)
        where T : class? where TEnumerable : IEnumerable<T>
    {
        // direct type check (for dead code ellimination)
        if (typeof(TEnumerable) == typeof(T[]))
            return FromArray(_this, UnsafeHelper.As<TEnumerable, T[]>(enumerable));
        if (typeof(TEnumerable) == typeof(ReadOnlyCollection<T>))
            return FromReadOnlyCollection(_this, UnsafeHelper.As<TEnumerable, ReadOnlyCollection<T>>(enumerable));

        // indirect type cast and check
        return IndirectDispatch(_this, enumerable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope IndirectDispatch(ArrayPool<T> _this, TEnumerable enumerable)
            => enumerable switch
            {
                T[] array => FromArray(_this, array),
                ReadOnlyCollection<T> collection => FromReadOnlyCollection(_this, collection),
                ICollection<T> collection => FromCollection(_this, collection),
                ILockable lockable => FromEnumerable_ModernLock(_this, enumerable, lockable),
                _ => FromEnumerable_MonitorLock(_this, enumerable)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromArray(ArrayPool<T> _this, T[] array)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            scope.Resize(array.Length);
            scope.CopyFrom(array, startIndex: 0);
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromReadOnlyCollection(ArrayPool<T> _this, ReadOnlyCollection<T> collection)
        {
            if (ReadOnlyCollectionUnwrapper<T>.TryUnwrap(collection, out IList<T>? result))
            {
                if (result is T[] array)
                    return FromArray(_this, array);
                else
                    return FromCollection(_this, result);
            }
            return FromCollection(_this, collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromCollection(ArrayPool<T> _this, ICollection<T> collection)
        {
            if (collection is IProducerConsumerCollection<T>)
                goto Atomic;

            if (collection is ICollection legacyCollection)
            {
                if (legacyCollection.IsSynchronized)
                    goto Atomic;
                return FromNormalCollection_MonitorLock(_this, collection, legacyCollection.SyncRoot);
            }

            if (collection is ILockable lockable)
                return FromNormalCollection_ModernLock(_this, collection, lockable);
            else
                return FromNormalCollection_MonitorLock(_this, collection, collection);

        Atomic:
            return FromAtomicCollection(_this, collection);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromAtomicCollection(ArrayPool<T> _this, ICollection<T> collection)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            int count = collection.Count;
            do
            {
                scope.Resize(count);
                try
                {
                    scope.CopyFrom(collection, startIndex: 0);
                }
                catch (ArgumentException)
                {
                    int newCount = collection.Count;
                    if (count == newCount)
                    {
                        scope.Dispose();
                        throw;
                    }
                    count = newCount;
                    continue;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
                {
                    int newCount = collection.Count;
                    if (count == newCount)
                        break;
                    count = newCount;
                    continue;
                }
            } while (true);
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromNormalCollection_ModernLock(ArrayPool<T> _this, ICollection<T> collection, ILockable lockable)
        {
            using Lock.Scope scope = lockable.EnterLockScope();
            return FromNormalCollection_Core(_this, collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromNormalCollection_MonitorLock(ArrayPool<T> _this, ICollection<T> collection, object lockObj)
        {
            lock (lockObj)
                return FromNormalCollection_Core(_this, collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromNormalCollection_Core(ArrayPool<T> _this, ICollection<T> collection)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            int count = collection.Count;
            scope.Resize(count);
            try
            {
                scope.CopyFrom(collection, startIndex: 0);
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromEnumerable_ModernLock(ArrayPool<T> _this, TEnumerable enumerable, ILockable lockable)
        {
            using Lock.Scope scope = lockable.EnterLockScope();
            return FromEnumerable_Core(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromEnumerable_MonitorLock(ArrayPool<T> _this, TEnumerable enumerable)
        {
            lock (enumerable)
                return FromEnumerable_Core(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ArrayPool<T>.RentScope FromEnumerable_Core(ArrayPool<T> _this, TEnumerable enumerable)
        {
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
                return _this.EnterRentScope();
            using PooledList<T> list = new PooledList<T>(_this, capacity: 1);
            list.Add(enumerator.Current);
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);
            (T[] array, int count) = list;
            return new ArrayPool<T>.RentScope(_this, array, count);
        }
    }

    private static unsafe class ReadOnlyCollectionUnwrapper<T>
    {
        private static readonly void* _unwrapFunc;
        private static readonly bool _isEnabled;

        static ReadOnlyCollectionUnwrapper()
        {
            void* func = (void*)ReflectionHelper.GetPropertyGetterPointer(typeof(ReadOnlyCollection<T>), "Items", typeof(IList<T>),
                flags: BindingFlags.Instance | BindingFlags.NonPublic);
            _isEnabled = func is not null;
            _unwrapFunc = func;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUnwrap(ReadOnlyCollection<T> collection, [NotNullWhen(true)] out IList<T>? result)
        {
            if (!_isEnabled)
            {
                result = default;
                return false;
            }
            result = ((delegate* managed<ReadOnlyCollection<T>, IList<T>>)_unwrapFunc)(collection);
            return true;
        }
    }
}
