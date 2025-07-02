using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        #region Not
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array) 
        {
            fixed (T* ptr = array)
                NotCore(ptr, MathHelper.MakeUnsigned(array.Length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array, int startIndex) 
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                NotCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T[] array, int startIndex, int count) 
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                NotCore(ptr + startIndex, unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not<T>(T* ptr, nuint length) => NotCore(ptr, length);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void NotCore<T>(T* ptr, nuint length)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Not((byte*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Not((sbyte*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Not((short*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Not((ushort*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Not((int*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Not((uint*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Not((long*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Not((ulong*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Not((float*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Not((double*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Not((nint*)ptr, length);
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Not((nuint*)ptr, length);
                return;
            }
            NotCoreSlow(ptr, length);
        }

        private static void NotCoreSlow<T>(T* ptr, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Not((byte*)ptr, length);
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Not((sbyte*)ptr, length);
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Not((short*)ptr, length);
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Not((ushort*)ptr, length);
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Not((int*)ptr, length);
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Not((uint*)ptr, length);
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Not((long*)ptr, length);
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Not((ulong*)ptr, length);
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Not((float*)ptr, length);
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Not((double*)ptr, length);
                        return;
                    default:
                        SlowCore<T>.Not(ptr, length);
                        return;
                }
            }
            SlowCore<T>.Not(ptr, length);
        }
        #endregion
    }
}
