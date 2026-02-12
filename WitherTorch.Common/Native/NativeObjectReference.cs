using System;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Auto)]
    public unsafe ref struct NativeObjectReference<T> : ICheckableDisposable where T : NativeObject, new()
    {
        private readonly ReferenceType _type;
        private void* _handle;

        internal NativeObjectReference(void* handle, ReferenceType type)
        {
            _handle = handle;
            _type = type;   
        }

        public readonly bool IsDisposed => _handle == null;

        public T GetAndDispose()
        {
            void* handle = ReferenceHelper.Exchange(ref _handle, null);
            T? result = NativeObject.FromNativePointer<T>(handle, _type);
            if (result is null)
                throw new ObjectDisposedException(nameof(NativeObjectReference<>));
            return result;
        }

        public void Dispose()
        {
            void* handle = ReferenceHelper.Exchange(ref _handle, null);
            if (handle == null)
                return;

            if (_type == ReferenceType.Owned)
                NativeObject.FromNativePointer<T>(handle, ReferenceType.Owned)?.Dispose();
        }
    }
}
