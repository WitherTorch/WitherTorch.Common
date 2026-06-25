using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Extensions;

partial class ArrayPoolExtensions
{
    partial class CaptureCore<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope IndirectDispatch_NoThreadSafe<TEnumerable>(ArrayPool<T> _this, TEnumerable enumerable)
            where TEnumerable : IEnumerable<T>
               => enumerable switch
               {
                   T[] array => FromArray(_this, array),
                   LockableEnumerable<T> lockable => FromLockableEnumerable_Recursive(_this, lockable),
                   MonitorLockableEnumerable<T> lockable => FromMonitorLockableEnumerable_Recursive(_this, lockable),
                   ICollection<T> collection => FromCollection_NoThreadSafe(_this, collection),
                   ILockable lockable => FromEnumerable_ModernLock(_this, enumerable, lockable),
                   IMonitorLockable lockable => FromEnumerable_MonitorLock(_this, enumerable, lockable),
                   _ => FromEnumerable_Core(_this, enumerable)
               };

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArrayPool<T>.RentScope FromLockableEnumerable_Recursive(ArrayPool<T> _this, LockableEnumerable<T> enumerable)
            => FromLockableEnumerable(_this, enumerable);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArrayPool<T>.RentScope FromMonitorLockableEnumerable_Recursive(ArrayPool<T> _this, MonitorLockableEnumerable<T> enumerable)
            => FromMonitorLockableEnumerable(_this, enumerable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArrayPool<T>.RentScope FromCollection_NoThreadSafe(ArrayPool<T> _this, ICollection<T> collection)
        {
            switch (collection)
            {
                case ILockable lockable:
                    {
                        using (lockable.EnterLockScope())
                            return FromNormalCollection_Core(_this, collection);
                    }
                case IMonitorLockable lockable:
                    {
                        using (lockable.EnterLockScope())
                            return FromNormalCollection_Core(_this, collection);
                    }
                default:
                    return FromNormalCollection_Core(_this, collection);
            }
        }
    }
}
