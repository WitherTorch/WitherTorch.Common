using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    public abstract unsafe partial class CustomUnknownAdapterBase : CriticalFinalizerObject, IUnknown
    {
        private readonly Lazy<nint> _handleLazy;

        private bool _disposed;

        public CustomUnknownAdapterBase()
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

        public uint AddRef() => ((NativeDataHolder*)_handleLazy.Value)->AddRef();

        public uint Release()
        {
            Lazy<nint> handleLazy = _handleLazy;
            if (!handleLazy.IsValueCreated)
                return 0U;
            return ((NativeDataHolder*)handleLazy.Value)->Release();
        }

        protected virtual bool IsGuidSupported(in Guid guid) => guid == ComObject.IID_IUnknown;

        protected virtual int GetMethodTableLength() => 3;

        protected virtual void FillMethodTable(ref VTableStack table)
        {
            table.Push((delegate* unmanaged[Stdcall]<NativeDataHolder*, Guid*, void**, uint>)&QueryInterface);
            table.Push((delegate* unmanaged[Stdcall]<NativeDataHolder*, uint>)&AddRef);
            table.Push((delegate* unmanaged[Stdcall]<NativeDataHolder*, uint>)&Release);
        }

        private nint CreateUnsafeHandle()
        {
            int length = GetMethodTableLength();
            void* methodTable = NativeMethods.AllocMemory(length * sizeof(void*));
            VTableStack stack = new VTableStack((void**)methodTable, length);
            FillMethodTable(ref stack);
            return (nint)NativeMethods.AllocUnmanagedStructure(new NativeDataHolder(methodTable, this));
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
        private static uint QueryInterface(NativeDataHolder* nativePointer, Guid* riid, void** ppvObject)
        {
            const uint S_OK = 0x00000000;
            const uint E_NOINTERFACE = 0x80004002;
            const uint E_POINTER = 0x80004003;

            if (ppvObject == null)
                return E_POINTER;

            if (nativePointer->TryGetAdapter(out CustomUnknownAdapterBase? adapter) && adapter.IsGuidSupported(in *riid))
            {
                *ppvObject = nativePointer;
                return S_OK;
            }

            return E_NOINTERFACE;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
        private static uint AddRef(NativeDataHolder* nativePointer) => nativePointer->AddRef();

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
        private static uint Release(NativeDataHolder* nativePointer) => nativePointer->Release();

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

        ~CustomUnknownAdapterBase()
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
