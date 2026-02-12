using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Native
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

        public static NativeObject? FromNativePointer(Type objectType, void* nativePointer, ReferenceType referenceType)
        {
            if (nativePointer == null || !typeof(NativeObject).IsAssignableFrom(objectType))
                return null;
            object? rawResult = Activator.CreateInstance(objectType);
            if (rawResult is null)
                return null;
            if (rawResult is not NativeObject result)
            {
                (rawResult as IDisposable)?.Dispose();
                return null;
            }
            result.LateBind(nativePointer, referenceType);
            return result;
        }

        public static NativeObject? CopyReference(NativeObject? obj)
        {
            if (obj is null)
                return null;
            lock (obj)
            {
                if (obj.IsEmpty || obj.IsDisposed)
                    return null;
                return CopyReferenceCore(obj, obj.ReferenceType);
            }
        }

        public static T? CopyReference<T>(T? obj) where T : NativeObject, new()
        {
            if (obj is null)
                return null;
            lock (obj)
            {
                if (obj.IsEmpty || obj.IsDisposed)
                    return null;
                return CopyReferenceCore(obj, obj.ReferenceType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static NativeObject? CopyReferenceCore(NativeObject obj, ReferenceType pointerType)
        {
            NativeObject? newObj = FromNativePointer(obj.GetType(), obj._nativePointer, pointerType);
            if (newObj is null)
                return null;
            if (pointerType == ReferenceType.Owned)
                newObj.AfterPointerCopied();
            return newObj;
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

        public static NativeObjectReference<T> CopyReferenceLater<T>(T? obj) where T : NativeObject, new()
        {
            if (obj is null)
                return default;
            lock (obj)
            {
                if (obj.IsEmpty || obj.IsDisposed)
                    return default;
                return CopyReferenceLaterCore(obj, obj.ReferenceType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static NativeObjectReference<T> CopyReferenceLaterCore<T>(T obj, ReferenceType pointerType) where T : NativeObject, new()
        {
            void* nativePointer = obj._nativePointer;   
            if (nativePointer is null)
                return default;
            if (pointerType == ReferenceType.Owned)
                obj.AfterPointerCopied();
            return new NativeObjectReference<T>(nativePointer, pointerType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator void*(NativeObject comObject)
            => comObject is not null ? comObject.NativePointer : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator nint(NativeObject comObject)
            => comObject is not null ? (nint)comObject._nativePointer : default;
    }
}
