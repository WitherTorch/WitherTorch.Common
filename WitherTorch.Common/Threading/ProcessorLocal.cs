using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Threading
{
    public sealed class ProcessorLocal<T> : IDisposable where T : class
    {
        private static readonly int _logicalProcessorCount = Environment.ProcessorCount;

        private readonly LazyTiny<T>[] _values;

        public T Value => _values[NativeMethods.GetCurrentProcessorId()].Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? GetValueDirectly() => _values[NativeMethods.GetCurrentProcessorId()].Value;

        public ProcessorLocal(Func<T> factory)
        {
            int count = _logicalProcessorCount;
            LazyTiny<T>[] values = new LazyTiny<T>[count];
            for (int i = 0; i < count; i++)
                values[i] = new LazyTiny<T>(factory, isThreadSafe: false);
            _values = values;
        }

        public void Dispose()
        {
            Array.Clear(_values);
            GC.SuppressFinalize(this);
        }
    }
}
