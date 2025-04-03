using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string str, string str2)
        {
            int length = str.Length;
            if (length != str2.Length)
                return false;
            fixed (char* ptr = str, ptr2 = str2)
                return Equals(ptr, ptr + length, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T* ptr, T* ptrEnd, T* ptr2) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.Equals((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2);
            if (typeof(T) == typeof(char))
                return Core<ushort>.Equals((ushort*)ptr, (ushort*)ptrEnd, (ushort*)ptr2);
            if (typeof(T) == typeof(nint))
                return Core.Equals((IntPtr*)ptr, (IntPtr*)ptrEnd, (IntPtr*)ptr2);
            if (typeof(T) == typeof(nuint))
                return Core.Equals((UIntPtr*)ptr, (UIntPtr*)ptrEnd, (UIntPtr*)ptr2);
            return Core<T>.Equals(ptr, ptrEnd, ptr2);
        }
    }
}
