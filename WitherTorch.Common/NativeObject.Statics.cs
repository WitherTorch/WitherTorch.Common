using System.Runtime.CompilerServices;

namespace WitherTorch.Common
{
    unsafe partial class NativeObject
    {
        public static T? FromNativePointer<T>(void* nativePointer, ReferenceType referenceType) where T : NativeObject, new()
        {
            if (nativePointer == null)
                return null;
            T result = new T();
            result.LateBind(nativePointer, referenceType);
            return result;
        }

        public static T? CopyReference<T>(T obj) where T : NativeObject, new()
        {
            if (obj is null)
                return null;
            if (obj.IsEmpty)
                return null;
            if (obj.IsDisposed)
                return null;
            return CopyReferenceCore(obj, obj.ReferenceType);
        }

        public static T? CopyReference<T>(T obj, ReferenceType referenceType) where T : NativeObject, new()
        {
            if (obj is null)
                return null;
            if (obj.IsEmpty)
                return null;
            if (obj.IsDisposed)
                return null;
            return CopyReferenceCore(obj, referenceType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T? CopyReferenceCore<T>(T obj, ReferenceType pointerType) where T : NativeObject, new()
        {
            T? newObj = FromNativePointer<T>(obj._nativePointer, pointerType);
            if (newObj is null)
                return null;
            if (pointerType == ReferenceType.Owned)
                newObj.AfterPointerCopied();
            return newObj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator void*(NativeObject comObject) 
            => comObject is not null ? comObject.NativePointer : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator nint(NativeObject comObject) 
            => comObject is not null ? (nint)comObject._nativePointer : 0;
    }
}
