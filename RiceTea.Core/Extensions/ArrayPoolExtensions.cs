using System.Collections.Generic;
using System.Collections.ObjectModel;

using RiceTea.Core.Buffers;
using RiceTea.Core.Helpers;
using RiceTea.Core.Threading;
using RiceTea.Core.Collections;
using System.Runtime.CompilerServices;


#if NET8_0_OR_GREATER
using System.Collections.Immutable;
#endif

namespace RiceTea.Core.Extensions;

public static partial class ArrayPoolExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArrayPool<T>.RentScope EnterRentScopeAndCapture<T, TEnumerable>(this ArrayPool<T> _this, TEnumerable enumerable)
        where T : class? where TEnumerable : IEnumerable<T>
    {
        // direct type check (for dead code ellimination)
        if (typeof(TEnumerable) == typeof(T[]))
            return CaptureCore<T>.FromArray(_this, UnsafeHelper.As<TEnumerable, T[]>(enumerable));
        if (typeof(TEnumerable) == typeof(LimitedImmutableArrayView<T>))
            return CaptureCore<T>.FromLimitedImmutableArrayView(_this, UnsafeHelper.As<TEnumerable, LimitedImmutableArrayView<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(ReadOnlyCollection<T>))
            return CaptureCore<T>.FromReadOnlyCollection(_this, UnsafeHelper.As<TEnumerable, ReadOnlyCollection<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(LockableEnumerable<T>))
            return CaptureCore<T>.FromLockableEnumerable(_this, UnsafeHelper.As<TEnumerable, LockableEnumerable<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(MonitorLockableEnumerable<T>))
            return CaptureCore<T>.FromMonitorLockableEnumerable(_this, UnsafeHelper.As<TEnumerable, MonitorLockableEnumerable<T>>(enumerable));

#if NET8_0_OR_GREATER
        if (typeof(TEnumerable) == typeof(ImmutableArray<T>))
            return CaptureCore<T>.FromImmutableArray(_this, ref UnsafeHelper.As<TEnumerable, ImmutableArray<T>>(ref enumerable));
        if (typeof(TEnumerable) == typeof(ImmutableList<T>))
            return CaptureCore<T>.FromImmutableList(_this, UnsafeHelper.As<TEnumerable, ImmutableList<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(ImmutableHashSet<T>))
            return CaptureCore<T>.FromImmutableSet(_this, UnsafeHelper.As<TEnumerable, ImmutableHashSet<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(ImmutableSortedSet<T>))
            return CaptureCore<T>.FromImmutableSet(_this, UnsafeHelper.As<TEnumerable, ImmutableSortedSet<T>>(enumerable));
        if (typeof(TEnumerable) == typeof(ImmutableStack<T>) || typeof(TEnumerable) == typeof(ImmutableQueue<T>))
            return CaptureCore<T>.FromEnumerable_Core(_this, enumerable);
#endif

        // indirect type cast and check
        return CaptureCore<T>.IndirectDispatch(_this, enumerable);

    }
}
