using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOf<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOf((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOf((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOf((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOf(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfExclude<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfExclude((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfExclude((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfExclude((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOfExclude(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfGreaterThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfGreaterThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfGreaterThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOfGreaterThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfGreaterOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfGreaterOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfGreaterOrEqualsThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfGreaterOrEqualsThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOfGreaterOrEqualsThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfLessThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfLessThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfLessThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOfLessThan(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* PointerIndexOfLessOrEqualsThan<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(char))
                return (T*)Core<ushort>.PointerIndexOfLessOrEqualsThan((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value)); 
            if (typeof(T) == typeof(IntPtr))
                return (T*)Core.PointerIndexOfLessOrEqualsThan((IntPtr*)ptr, (IntPtr*)ptrEnd, UnsafeHelper.As<T, IntPtr>(value));
            if (typeof(T) == typeof(UIntPtr))
                return (T*)Core.PointerIndexOfLessOrEqualsThan((UIntPtr*)ptr, (UIntPtr*)ptrEnd, UnsafeHelper.As<T, UIntPtr>(value));
            return Core<T>.PointerIndexOfLessOrEqualsThan(ptr, ptrEnd, value);
        }
    }
}
