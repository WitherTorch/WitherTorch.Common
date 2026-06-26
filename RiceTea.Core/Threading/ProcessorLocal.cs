using System;
using System.Runtime.CompilerServices;
using System.Threading;

using RiceTea.Core.Helpers;
using RiceTea.Core.Native;

namespace RiceTea.Core.Threading;

public sealed class ProcessorLocal<T> : IDisposable where T : class
{
    private static readonly uint _logicalProcessorCount = MathHelper.MakeUnsigned(Environment.ProcessorCount);
    private static readonly uint _maximumIndex = _logicalProcessorCount - 1;

    private readonly LazyTiny<T>[] _values;

    public T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            uint processorId = NativeMethods.GetCurrentProcessorId();
            return UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_values), MathHelper.Clamp(processorId, 0, _maximumIndex)).Value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetValueDirectly()
    {
        uint processorId = NativeMethods.GetCurrentProcessorId();
        return UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_values), MathHelper.Clamp(processorId, 0, _maximumIndex)).GetValueDirectly();
    }

    public ProcessorLocal(Func<T> factory)
    {
        uint count = _logicalProcessorCount;
        LazyTiny<T>[] values = new LazyTiny<T>[count];
        ref LazyTiny<T> valuesRef = ref UnsafeHelper.GetArrayDataReference(values);
        for (uint i = 0; i < count; i++)
            UnsafeHelper.AddTypedOffset(ref valuesRef, i) = new LazyTiny<T>(factory, LazyThreadSafetyMode.None);
        _values = values;
    }

    public void Dispose()
    {
        Array.Clear(_values, 0, (int)_logicalProcessorCount);
        GC.SuppressFinalize(this);
    }
}
