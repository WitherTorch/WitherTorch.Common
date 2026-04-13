using System;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Threading
{
    public sealed class ProcessorLocal<T> : IDisposable where T : class
    {
        private static readonly int _logicalProcessorCount = Environment.ProcessorCount;
        private static readonly int _maximumIndex = _logicalProcessorCount - 1;

        private readonly LazyTiny<T>[] _values;

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int processorId = NativeMethods.GetCurrentProcessorId();
                return UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_values), MathHelper.Clamp(processorId, 0, _maximumIndex)).Value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? GetValueDirectly()
        {
            int processorId = NativeMethods.GetCurrentProcessorId();
            return UnsafeHelper.AddTypedOffset(ref UnsafeHelper.GetArrayDataReference(_values), MathHelper.Clamp(processorId, 0, _maximumIndex)).GetValueDirectly();
        }

        public ProcessorLocal(Func<T> factory)
        {
            int count = _logicalProcessorCount;
            LazyTiny<T>[] values = new LazyTiny<T>[count];
            ref LazyTiny<T> valuesRef = ref UnsafeHelper.GetArrayDataReference(values);
            for (int i = 0; i < count; i++)
                UnsafeHelper.AddTypedOffset(ref valuesRef, i) = new LazyTiny<T>(factory, LazyThreadSafetyMode.None);
            _values = values;
        }

        public void Dispose()
        {
            Array.Clear(_values, 0, _logicalProcessorCount);
            GC.SuppressFinalize(this);
        }
    }
}
