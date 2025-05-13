using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;

using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    public abstract unsafe partial class ComObjectAdapter : CriticalFinalizerObject, IUnknown
    {
        private readonly Lazy<nint> _handleLazy;

        private bool _disposed;

        public ComObjectAdapter()
        {
            _handleLazy = new Lazy<nint>(CreateUnsafeHandle);
        }

        public void* GetWin32Handle() => _disposed ? null : (void*)_handleLazy.Value;

        public bool TryQueryInterface(in Guid guid, [NotNullWhen(true)] out IUnknown? queriedObject)
        {
            if (IsGuidSupported(guid))
            {
                queriedObject = this;
                return true;
            }
            queriedObject = null;
            return false;
        }

        public ulong AddRef() => ((NativeDataHolder*)_handleLazy.Value)->AddRef();

        public ulong Release()
        {
            Lazy<nint> handleLazy = _handleLazy;
            if (!handleLazy.IsValueCreated)
                return 0UL;
            return ((NativeDataHolder*)handleLazy.Value)->Release();
        }

        protected virtual bool IsGuidSupported(in Guid guid) => guid == ComObject.IID_IUnknown;

        protected virtual int GetMethodTableLength() => 3;

        protected virtual void FillMethodTable(ref VTableStack table)
        {
            table.Push((delegate*<NativeDataHolder*, Guid*, void**, uint>)&QueryInterface);
            table.Push((delegate*<NativeDataHolder*, ulong>)&AddRef);
            table.Push((delegate*<NativeDataHolder*, ulong>)&Release);
        }

        private nint CreateUnsafeHandle()
        {
            int length = GetMethodTableLength();
            void* methodTable = NativeMethods.AllocMemory(length * sizeof(void*));
            VTableStack stack = new VTableStack((void**)methodTable, length);
            FillMethodTable(ref stack);
            return (nint)NativeMethods.AllocUnmanagedStructure(new NativeDataHolder(methodTable, this));
        }

        private static uint QueryInterface(NativeDataHolder* nativePointer, Guid* riid, void** ppvObject)
        {
            const uint S_OK = 0x00000000;
            const uint E_NOINTERFACE = 0x80004002;
            const uint E_POINTER = 0x80004003;

            if (ppvObject == null)
                return E_POINTER;

            if (nativePointer->TryGetAdapter(out ComObjectAdapter? adapter) && adapter.IsGuidSupported(in *riid))
            {
                *ppvObject = nativePointer;
                return S_OK;
            }

            return E_NOINTERFACE;
        }

        private static ulong AddRef(NativeDataHolder* nativePointer) => nativePointer->AddRef();

        private static ulong Release(NativeDataHolder* nativePointer) => nativePointer->Release();

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            DisposeCore(disposing);
        }

        protected virtual void DisposeCore(bool disposing)
        {
            Release();
        }

        ~ComObjectAdapter()
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
