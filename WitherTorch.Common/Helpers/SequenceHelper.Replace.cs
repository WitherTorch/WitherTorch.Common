using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        #region Replace
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Replace<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Replace<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Replace<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Replace<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Replace<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceCore(ptr, length, filter, replacement);
        #endregion

        #region ReplaceExclude
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceExclude<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceExcludeCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceExclude<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceExcludeCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceExclude<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceExcludeCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceExclude<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceExcludeCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceExclude<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceExcludeCore(ptr, length, filter, replacement);
        #endregion

        #region ReplaceGreaterThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterThan<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceGreaterThanCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterThan<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceGreaterThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterThan<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceGreaterThanCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterThan<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceGreaterThanCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterThan<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceGreaterThanCore(ptr, length, filter, replacement);
        #endregion

        #region ReplaceGreaterOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterOrEqualsThan<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceGreaterOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterOrEqualsThan<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceGreaterOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterOrEqualsThan<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceGreaterOrEqualsThanCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterOrEqualsThan<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceGreaterOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceGreaterOrEqualsThan<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceGreaterOrEqualsThanCore(ptr, length, filter, replacement);
        #endregion

        #region ReplaceLessThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessThan<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceLessThanCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessThan<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceLessThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessThan<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceLessThanCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessThan<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceLessThanCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessThan<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceLessThanCore(ptr, length, filter, replacement);
        #endregion

        #region ReplaceLessOrEqualsThan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessOrEqualsThan<T>(T[] array, T filter, T replacement)
        {
            fixed (T* ptr = array)
                ReplaceLessOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(array.Length), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessOrEqualsThan<T>(T[] array, T filter, T replacement, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                ReplaceLessOrEqualsThanCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessOrEqualsThan<T>(T[] array, T filter, T replacement, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                ReplaceLessOrEqualsThanCore(ptr + startIndex, unchecked((uint)count), filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessOrEqualsThan<T>(T* ptr, int length, T filter, T replacement)
            => ReplaceLessOrEqualsThanCore(ptr, MathHelper.MakeUnsigned(length), filter, replacement);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceLessOrEqualsThan<T>(T* ptr, nuint length, T filter, T replacement)
            => ReplaceLessOrEqualsThanCore(ptr, length, filter, replacement);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Replace((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Replace((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Replace((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Replace((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Replace((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Replace((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Replace((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Replace((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Replace((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Replace((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Replace((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Replace((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Replace((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Replace((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Replace((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Replace((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Replace((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Replace((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Replace((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Replace((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Replace((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Replace((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.Replace(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.Replace(ptr, length, filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceExcludeCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.ReplaceExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.ReplaceExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.ReplaceExclude((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.ReplaceExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.ReplaceExclude((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.ReplaceExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.ReplaceExclude((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.ReplaceExclude((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.ReplaceExclude((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.ReplaceExclude((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.ReplaceExclude((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceExcludeCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceExcludeCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.ReplaceExclude((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.ReplaceExclude((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.ReplaceExclude((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.ReplaceExclude((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.ReplaceExclude((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.ReplaceExclude((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.ReplaceExclude((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.ReplaceExclude((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.ReplaceExclude((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.ReplaceExclude(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.ReplaceExclude(ptr, length, filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceGreaterThanCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.ReplaceGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.ReplaceGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.ReplaceGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.ReplaceGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.ReplaceGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.ReplaceGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.ReplaceGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.ReplaceGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.ReplaceGreaterThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.ReplaceGreaterThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceGreaterThanCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceGreaterThanCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.ReplaceGreaterThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.ReplaceGreaterThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.ReplaceGreaterThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.ReplaceGreaterThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.ReplaceGreaterThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.ReplaceGreaterThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.ReplaceGreaterThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.ReplaceGreaterThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.ReplaceGreaterThan(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.ReplaceGreaterThan(ptr, length, filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceGreaterOrEqualsThanCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.ReplaceGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.ReplaceGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.ReplaceGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.ReplaceGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.ReplaceGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.ReplaceGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.ReplaceGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.ReplaceGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.ReplaceGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.ReplaceGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.ReplaceGreaterOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.ReplaceGreaterOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceGreaterOrEqualsThanCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceGreaterOrEqualsThanCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.ReplaceGreaterOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.ReplaceGreaterOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.ReplaceGreaterOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.ReplaceGreaterOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.ReplaceGreaterOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.ReplaceGreaterOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.ReplaceGreaterOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.ReplaceGreaterOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.ReplaceGreaterOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.ReplaceGreaterOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.ReplaceGreaterOrEqualsThan(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.ReplaceGreaterOrEqualsThan(ptr, length, filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceLessThanCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.ReplaceLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.ReplaceLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.ReplaceLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.ReplaceLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.ReplaceLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.ReplaceLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.ReplaceLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.ReplaceLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.ReplaceLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.ReplaceLessThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.ReplaceLessThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceLessThanCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceLessThanCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.ReplaceLessThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.ReplaceLessThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.ReplaceLessThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.ReplaceLessThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.ReplaceLessThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.ReplaceLessThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.ReplaceLessThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.ReplaceLessThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.ReplaceLessThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.ReplaceLessThan(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.ReplaceLessThan(ptr, length, filter, replacement);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceLessOrEqualsThanCore<T>(T* ptr, nuint length, T filter, T replacement)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.ReplaceLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.ReplaceLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.ReplaceLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.ReplaceLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.ReplaceLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.ReplaceLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.ReplaceLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.ReplaceLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.ReplaceLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.ReplaceLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.ReplaceLessOrEqualsThan((nint*)ptr, length, UnsafeHelper.As<T, nint>(filter), UnsafeHelper.As<T, nint>(replacement));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.ReplaceLessOrEqualsThan((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(filter), UnsafeHelper.As<T, nuint>(replacement));
                return;
            }
            ReplaceLessOrEqualsThanCoreSlow(ptr, length, filter, replacement);
        }

        private static void ReplaceLessOrEqualsThanCoreSlow<T>(T* ptr, nuint length, T filter, T replacement)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.ReplaceLessOrEqualsThan((byte*)ptr, length, UnsafeHelper.As<T, byte>(filter), UnsafeHelper.As<T, byte>(replacement));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.ReplaceLessOrEqualsThan((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(filter), UnsafeHelper.As<T, sbyte>(replacement));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.ReplaceLessOrEqualsThan((short*)ptr, length, UnsafeHelper.As<T, short>(filter), UnsafeHelper.As<T, short>(replacement));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.ReplaceLessOrEqualsThan((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(filter), UnsafeHelper.As<T, ushort>(replacement));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.ReplaceLessOrEqualsThan((int*)ptr, length, UnsafeHelper.As<T, int>(filter), UnsafeHelper.As<T, int>(replacement));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.ReplaceLessOrEqualsThan((uint*)ptr, length, UnsafeHelper.As<T, uint>(filter), UnsafeHelper.As<T, uint>(replacement));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.ReplaceLessOrEqualsThan((long*)ptr, length, UnsafeHelper.As<T, long>(filter), UnsafeHelper.As<T, long>(replacement));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.ReplaceLessOrEqualsThan((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(filter), UnsafeHelper.As<T, ulong>(replacement));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.ReplaceLessOrEqualsThan((float*)ptr, length, UnsafeHelper.As<T, float>(filter), UnsafeHelper.As<T, float>(replacement));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.ReplaceLessOrEqualsThan((double*)ptr, length, UnsafeHelper.As<T, double>(filter), UnsafeHelper.As<T, double>(replacement));
                        return;
                    default:
                        SlowCore<T>.ReplaceLessOrEqualsThan(ptr, length, filter, replacement);
                        return;
                }
            }
            SlowCore<T>.ReplaceLessOrEqualsThan(ptr, length, filter, replacement);
        }
        #endregion
    }
}
