using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

using WitherTorch.Common.Native;

namespace WitherTorch.Common
{
    /// <summary>
    /// Represents a native object
    /// </summary>
    public abstract unsafe partial class NativeObject : CriticalFinalizerObject, IDisposable
    {
        private ReferenceType _referenceType;
        private void* _nativePointer;

        public NativeObject()
        {
            _referenceType = ReferenceType.NeedBinding;
            _nativePointer = null;
        }

        public NativeObject(IntPtr handle, ReferenceType referenceType) : this(handle.ToPointer(), referenceType) { }

        public NativeObject(void* nativePointer, ReferenceType referenceType)
        {
            _nativePointer = referenceType switch
            {
                ReferenceType.NeedBinding => null,
                ReferenceType.Owned or ReferenceType.Weak => nativePointer,
                _ => throw new ArgumentException("Invalid reference type!", nameof(referenceType)),
            };
            _referenceType = referenceType;
        }

        public void* NativePointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _nativePointer;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _nativePointer == null;
        }

        public ReferenceType ReferenceType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _referenceType;
        }

        public bool IsDisposed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _referenceType == ReferenceType.Disposed;
        }

        protected void LateBind(IntPtr handle, ReferenceType referenceType)
            => LateBind(handle.ToPointer(), referenceType);

        protected void LateBind(void* handle, ReferenceType referenceType)
        {
            if (referenceType == ReferenceType.NeedBinding || _referenceType != ReferenceType.NeedBinding)
                return;
            _referenceType = referenceType;
            _nativePointer = handle;
        }

        protected abstract void AfterPointerCopied();

        protected abstract void ReleasePointer(void* pointer);

        protected virtual void DisposeManaged() { }

        protected virtual void Dispose(bool disposing)
        {
            ReferenceType referenceType = _referenceType;
            if (referenceType == ReferenceType.Disposed)
                return;
            _referenceType = ReferenceType.Disposed;

            if (disposing)
                DisposeManaged();

            void* nativePointer = _nativePointer;

            if (nativePointer == null)
                return;

            _nativePointer = null;

            if (referenceType == ReferenceType.Owned)
                ReleasePointer(nativePointer);
        }

        ~NativeObject()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
