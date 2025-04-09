using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using LocalsInit;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows
{
    [LocalsInit(false)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe partial class ComObject : NativeObject
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
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.QueryInterface);
            int hr = ((delegate* unmanaged[Stdcall]<void*, Guid*, void**, int>)functionPointer)(nativePointer, UnsafeHelper.AsPointerIn(in iid), &nativePointer);
            if (throwWhenQueryFailed)
                ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
            else
                ThrowHelper.ResetPointerForHR(hr, ref nativePointer);
            return FromNativePointer<T>(nativePointer, ReferenceType.Owned);
        }

        protected ulong AddRef() => AddRefCore(NativePointer);

        protected ulong Release() => ReleaseCore(NativePointer);

        protected override void AfterPointerCopied() => AddRefCore(NativePointer);

        protected override unsafe void ReleasePointer(void* pointer) => ReleaseCore(pointer);
    }
}
