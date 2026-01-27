using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security;

using LocalsInit;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [LocalsInit(false)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe partial class ComObject : NativeObject, IUnknown, IWin32HandleHolder
    {
        // IID_IUnknown = {00000000-0000-0000-C000-000000000046}
        public static readonly Guid IID_IUnknown = new Guid(0x00000000, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

        protected enum MethodTable
        {
            _Start = 0,
            QueryInterface = _Start,
            AddRef,
            Release,
            _End
        }

        public ComObject() : base() { }

        public ComObject(void* nativePointer, ReferenceType referenceType) : base(nativePointer, referenceType) { }

        public ComObject(nint handle, ReferenceType referenceType) : base(handle, referenceType) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void* GetFunctionPointerOrThrow(void* nativePointer, int offset)
        {
            if (nativePointer == null)
                throw new NullReferenceException(nameof(nativePointer));
            return GetFunctionPointerCore(nativePointer, offset);
        }

        public T? QueryInterface<T>(in Guid iid, bool throwWhenQueryFailed = true) where T : ComObject, new()
        {
            void* nativePointer = NativePointer;
            int hr = QueryInterfaceCore(ref nativePointer, iid);
            if (throwWhenQueryFailed)
                ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
            else
                ThrowHelper.ResetPointerForHR(hr, ref nativePointer);
            return FromNativePointer<T>(nativePointer, ReferenceType.Owned);
        }

        public bool TryQueryInterface(in Guid guid, [NotNullWhen(true)] out IUnknown? queriedObject)
        {
            void* nativePointer = NativePointer;
            int hr = QueryInterfaceCore(ref nativePointer, guid);
            if (hr < 0)
            {
                queriedObject = null;
                return false;
            }
            queriedObject = new ComObject(nativePointer, ReferenceType.Owned);
            return true;
        }

        public uint AddRef() => AddRefCore(NativePointer);

        public uint Release() => ReleaseCore(NativePointer);

        protected override void AfterPointerCopied() => AddRefCore(NativePointer);

        protected override unsafe void ReleasePointer(void* pointer) => ReleaseCore(pointer);

        unsafe void* IWin32HandleHolder.GetWin32Handle() => NativePointer;
    }
}
