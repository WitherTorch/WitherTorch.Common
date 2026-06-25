using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Threading;

#if NET8_0_OR_GREATER
using System.Collections.Immutable;
#endif

namespace WitherTorch.Common.Extensions;

partial class ArrayPoolExtensions
{
    private static partial class CaptureCore<T> where T : class?
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope IndirectDispatch<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable)
            where TEnumerable : IEnumerable<T>
               => enumerable switch
               {
                   T[] array => FromArray(_this, array),
                   ReadOnlyCollection<T> collection => FromReadOnlyCollection(_this, collection),
                   LockableEnumerable<T> lockable => FromLockableEnumerable(_this, lockable),
                   MonitorLockableEnumerable<T> lockable => FromMonitorLockableEnumerable(_this, lockable),
                   ICollection<T> collection => FromCollection(_this, collection),
#if NET8_0_OR_GREATER
                   IImmutableStack<T> _ or IImmutableQueue<T> _ => FromEnumerable_Core(_this, enumerable),
#endif
                   ILockable lockable => FromEnumerable_ModernLock(_this, enumerable, lockable),
                   IMonitorLockable lockable => FromEnumerable_MonitorLock(_this, enumerable, lockable),
                   IEnumerator<T> _ => FromEnumerable_MayBeSingleshot(_this, enumerable),
                   _ => FromEnumerable_MonitorLock(_this, enumerable)
               };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromArray(ArrayPool<T> _this, T[] array)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            scope.Resize(array.Length, moveArray: false);
            scope.CopyFrom(array, startIndex: 0);
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromReadOnlyCollection(ArrayPool<T> _this, ReadOnlyCollection<T> collection)
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
        public static ArrayPool<T>.RentScope FromLockableEnumerable(ArrayPool<T> _this, LockableEnumerable<T> enumerable)
        {
            using Lock.Scope scope = enumerable.EnterLockScope();
            return IndirectDispatch_NoThreadSafe(_this, enumerable.Items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromMonitorLockableEnumerable(ArrayPool<T> _this, MonitorLockableEnumerable<T> enumerable)
        {
            using var scope = enumerable.EnterLockScope();
            return IndirectDispatch_NoThreadSafe(_this, enumerable.Items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromCollection(ArrayPool<T> _this, ICollection<T> collection)
        {
#if NET8_0_OR_GREATER
            if (collection is IImmutableList<T> || collection is IImmutableSet<T>)
                return FromNormalCollection_Core(_this, collection);
#endif

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
        private static ArrayPool<T>.RentScope FromAtomicCollection(ArrayPool<T> _this, ICollection<T> collection)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            int count = collection.Count;
            do
            {
                scope.Resize(count, moveArray: false);
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
        private static ArrayPool<T>.RentScope FromNormalCollection_ModernLock(ArrayPool<T> _this, ICollection<T> collection, ILockable lockable)
        {
            using Lock.Scope scope = lockable.EnterLockScope();
            return FromNormalCollection_Core(_this, collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromNormalCollection_MonitorLock(ArrayPool<T> _this, ICollection<T> collection, object lockObj)
        {
            lock (lockObj)
                return FromNormalCollection_Core(_this, collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromNormalCollection_Core(ArrayPool<T> _this, ICollection<T> collection)
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            int count = collection.Count;
            scope.Resize(count, moveArray: false);
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
        private static ArrayPool<T>.RentScope FromEnumerable_MayBeSingleshot<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable)
            where TEnumerable : IEnumerable<T>
        {
            IEnumerator<T> enumerator;
            try
            {
                enumerator = enumerable.GetEnumerator();
            }
            catch (InvalidOperationException)
            {
                goto Fallback;
            }
            if (!ReferenceEquals(enumerable, enumerator))
                goto Fallback;

            return FromEnumerator(_this, enumerator);

        Fallback:
            return FromEnumerable_MonitorLock(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromEnumerator(ArrayPool<T> _this, IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext())
                return _this.EnterRentScope();
            using PooledList<T> list = new PooledList<T>(_this, capacity: 1);
            list.Add(enumerator.Current);
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);
            (T[] array, int count) = list;
            return new ArrayPool<T>.RentScope(_this, array, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromEnumerable_ModernLock<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable, ILockable lockable)
            where TEnumerable : IEnumerable<T>
        {
            using Lock.Scope scope = lockable.EnterLockScope();
            return FromEnumerable_Core(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromEnumerable_MonitorLock<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable)
            where TEnumerable : IEnumerable<T>
        {
            lock (enumerable)
                return FromEnumerable_Core(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromEnumerable_MonitorLock<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable, IMonitorLockable lockable)
            where TEnumerable : IEnumerable<T>
        {
            using MonitorLockScope scope = lockable.EnterLockScope();
            return FromEnumerable_Core(_this, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromEnumerable_Core<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable)
            where TEnumerable : IEnumerable<T>
        {
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();
            return FromEnumerator(_this, enumerator);
        }
    }
}
