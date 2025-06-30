using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        #region Divide
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array) where T : unmanaged
        {
            fixed (T* ptr = array)
                NotCore(ptr, ptr + array.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                NotCore(ptr + startIndex, ptr + length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                NotCore(ptr + startIndex, ptr + length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T* ptr, T* ptrEnd) where T : unmanaged
            => NotCore(ptr, ptrEnd);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void NotCore<T>(T* ptr, T* ptrEnd) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Not((byte*)ptr, (byte*)ptrEnd);
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Not((ushort*)ptr, (ushort*)ptrEnd);
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Not((nint*)ptr, (nint*)ptrEnd);
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Not((nuint*)ptr, (nuint*)ptrEnd);
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Not(ptr, ptrEnd);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Not((byte*)ptr, (byte*)ptrEnd);
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Not((sbyte*)ptr, (sbyte*)ptrEnd);
                        return;
                    case TypeCode.Int16:
                        Core<short>.Not((short*)ptr, (short*)ptrEnd);
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Not((ushort*)ptr, (ushort*)ptrEnd);
                        return;
                    case TypeCode.Int32:
                        Core<int>.Not((int*)ptr, (int*)ptrEnd);
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Not((uint*)ptr, (uint*)ptrEnd);
                        return;
                    case TypeCode.Int64:
                        Core<long>.Not((long*)ptr, (long*)ptrEnd);
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Not((ulong*)ptr, (ulong*)ptrEnd);
                        return;
                    case TypeCode.Single:
                        Core<float>.Not((float*)ptr, (float*)ptrEnd);
                        return;
                    case TypeCode.Double:
                        Core<double>.Not((double*)ptr, (double*)ptrEnd);
                        return;
                    default:
                        Core<T>.Not(ptr, ptrEnd);
                        return;
                }
            }
            Core<T>.Not(ptr, ptrEnd);
        }
        #endregion
    }
}
