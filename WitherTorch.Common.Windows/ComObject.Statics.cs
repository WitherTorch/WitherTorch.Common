using InlineMethod;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using WitherTorch.Common;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.CrossNative.Windows
{
    unsafe partial class ComObject
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? CoCreateInstance<T>(in Guid clsid, in Guid iid, bool throwWhenFailed = true) where T : ComObject, new()
        {
            void* nativePointer = CoCreateInstanceCore(clsid, iid, throwWhenFailed);
            return FromNativePointer<T>(nativePointer, ReferenceType.Owned);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ComObject? CoCreateInstance(in Guid clsid, bool throwWhenFailed = true)
        {
            void* nativePointer = CoCreateInstanceCore(clsid, IID_IUnknown, throwWhenFailed);
            return nativePointer == null ? null : new ComObject(nativePointer, ReferenceType.Owned);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void* CoCreateInstanceCore(in Guid clsid, in Guid iid, bool throwWhenFailed)
            => CoCreateInstanceCore(UnsafeHelper.AsPointerIn(in clsid), UnsafeHelper.AsPointerIn(in iid), throwWhenFailed);

        [Inline(InlineBehavior.Remove)]
        private static unsafe void* CoCreateInstanceCore(Guid* rclsid, Guid* riid, bool throwWhenFailed)
        {
            void* nativePointer;
            int hr = Ole32.CoCreateInstance(rclsid, null, ClassContextFlags.InProcessServer, riid, &nativePointer);
            if (hr >= 0)
                return nativePointer;
            if (!throwWhenFailed)
                return null;
            throw Marshal.GetExceptionForHR(hr)!;
        }
    }
}
