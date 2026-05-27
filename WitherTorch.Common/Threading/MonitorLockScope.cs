using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    [StructLayout(LayoutKind.Auto)]
    public ref struct MonitorLockScope : IDisposable
    {
        private object? _syncLock;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MonitorLockScope(object syncLock)
        {
            _syncLock = syncLock;
            Monitor.Enter(syncLock);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MonitorLockScope Enter(object syncLock) => new MonitorLockScope(syncLock);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            object? syncLock = ReferenceHelper.Exchange(ref _syncLock, null);
            if (syncLock is null)
                return;
            Monitor.Exit(syncLock);
        }
    }
}
