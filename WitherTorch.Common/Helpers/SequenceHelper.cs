using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8500
namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Clear<T>(T[] array)
        {
            int length = array.Length;
            if (length <= 0)
                return;
            fixed (void* ptr = array)
                UnsafeHelper.InitBlock(ptr, 0, unchecked((uint)(length * UnsafeHelper.SizeOf<T>())));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Clear<T>(T[] array, int startIndex, int count)
        {
            if (count <= 0)
                return;
            if (startIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (void* ptr = array)
                UnsafeHelper.InitBlock((byte*)ptr + startIndex * UnsafeHelper.SizeOf<T>(), 0, unchecked((uint)(count * UnsafeHelper.SizeOf<T>())));
        }
    }
}
