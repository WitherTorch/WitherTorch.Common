using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    public unsafe struct NativeDataHolder : IDisposable
    {
        private readonly void* _methodTable;
        private readonly GCHandle _handle;

        private ulong _refCount;

        public NativeDataHolder(void* methodTable, CustomUnknownAdapterBase adapter)
        {
            _methodTable = methodTable;
            _handle = GCHandle.Alloc(adapter, GCHandleType.Normal);
            _refCount = 1;
        }

        public ulong AddRef()
        {
            ulong refCount = _refCount;
            if (refCount == ulong.MaxValue)
                throw new OverflowException();
            refCount++;
            _refCount = refCount;
            return refCount;
        }

        public ulong Release()
        {
            ulong refCount = _refCount;
            if (refCount == 0)
                return 0;
            refCount--;
            _refCount = refCount;
            if (refCount == 0)
                Dispose();
            return refCount;
        }

        public readonly bool TryGetAdapter<T>([NotNullWhen(true)] out T? adapter) where T : CustomUnknownAdapterBase
        {
            adapter = _handle.Target as T;
            return adapter is not null;
        }

        public readonly void Dispose()
        {
            NativeMethods.FreeMemory(_methodTable);
            _handle.Free();
        }
    }
}
