using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

using WitherTorch.Common.Native;

namespace WitherTorch.Common.PolyFills;

internal sealed class AutoResetEventLite : CriticalFinalizerObject, IDisposable
{
    private IntPtr _handle;

    public IntPtr Handle => _handle;

    public AutoResetEventLite() => _handle = NativeMethods.CreateWaitingHandle(autoReset: true);

    public void Set() => NativeMethods.SetWaitingHandle(_handle);

    public void Reset() => NativeMethods.ResetWaitingHandle(_handle);

    public void Wait() => NativeMethods.WaitForWaitingHandle(_handle);

    public bool Wait(int millisecondsTimeout)
    {
        if (millisecondsTimeout < -1)
            Throw();

        if (millisecondsTimeout == -1)
        {
            Wait();
            return true;
        }
        else
            return NativeMethods.WaitForWaitingHandle(_handle, (uint)millisecondsTimeout);

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Throw() => ArgumentOutOfRangeException.Throw(nameof(millisecondsTimeout));
    }

    private void DisposeCore()
    {
        IntPtr oldHandle = Interlocked.Exchange(ref _handle, IntPtr.Zero);
        if (oldHandle == IntPtr.Zero)
            return;
        NativeMethods.DestroyWaitingHandle(oldHandle);
    }

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }

    ~AutoResetEventLite() => DisposeCore();
}
