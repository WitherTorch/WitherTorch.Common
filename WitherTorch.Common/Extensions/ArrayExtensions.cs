using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
#pragma warning disable CS8500
    public static class ArrayExtensions
    {
        extension(Array)
        {
            /// <inheritdoc cref="Array.Clear(Array, int, int)"/>
            [Inline(InlineBehavior.Keep, export: true)]
            public static unsafe void Clear(Array array)
                => Array.Clear(array, 0, array.Length);

            /// <inheritdoc cref="Array.Clear(Array, int, int)"/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Clear<T>(T[] array)
            {
                fixed (T* ptr = array)
                    UnsafeHelper.InitBlock(ptr, default, MathHelper.MakeUnsigned(array.Length) * UnsafeHelper.SizeOf<T>());
            }

            /// <inheritdoc cref="Array.Clear(Array, int, int)"/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Clear<T>(T[] array, int index, int length)
            {
                int arrayLength = array.Length;
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (length < 0)
                    throw new ArgumentOutOfRangeException(nameof(length));
                if (index + length > arrayLength)
                    throw new ArgumentOutOfRangeException(index >= arrayLength ? nameof(index) : nameof(length));
                fixed (T* ptr = array)
                    UnsafeHelper.InitBlockUnaligned(ptr + index, default, unchecked((nuint)length) * UnsafeHelper.SizeOf<T>());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Fill<T>(T[] array, T value)
            {
                if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
                {
                    fixed (T* ptr = array)
                        UnsafeHelper.InitBlock(ptr, UnsafeHelper.As<T, byte>(value), MathHelper.MakeUnsigned(array.Length));
                    return;
                }
                for (int i = 0, length = array.Length; i < length; i++)
                    array[i] = value;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Fill<T>(T[] array, T value, int startIndex, int count)
            {
                int length = array.Length;
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));
                int limit = startIndex + count;
                if (limit > length)
                    throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
                if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
                {
                    fixed (T* ptr = array)
                        UnsafeHelper.InitBlockUnaligned(ptr + startIndex, UnsafeHelper.As<T, byte>(value), unchecked((nuint)count));
                    return;
                }
                for (int i = startIndex; i < limit; i++)
                    array[i] = value;
            }
        }
    }
}
