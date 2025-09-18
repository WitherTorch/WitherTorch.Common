using System;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [SuppressUnmanagedCodeSecurity]
    public unsafe class ModalWindow : ComObject
    {
        protected new enum MethodTable
        {
            _Start = ComObject.MethodTable._End,
            Show = _Start,
            _End
        }

        public ModalWindow() : base() { }

        public ModalWindow(void* nativePointer, ReferenceType pointerType) : base(nativePointer, pointerType) { }

        [Inline(InlineBehavior.Keep, export: true)]
        public bool Show() => Show(IntPtr.Zero);

        public unsafe bool Show(nint hwndOwner)
        {
            const int E_CANCEL = unchecked((int)0x800704C7U);

            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.Show);
            int hr = ((delegate*<void*, nint, int>)functionPointer)(nativePointer, hwndOwner);
            if (hr >= 0)
                return true;
            if (hr != E_CANCEL)
                ThrowHelper.ThrowExceptionForHR(hr);
            return false;
        }
    }
}
