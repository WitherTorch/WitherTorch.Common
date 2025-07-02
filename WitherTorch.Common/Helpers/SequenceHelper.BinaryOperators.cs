using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        #region Or
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                OrCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                OrCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                OrCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            OrCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region And
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                AndCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                AndCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                AndCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            AndCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Xor
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                XorCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                XorCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                XorCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            XorCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Add
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                AddCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                AddCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                AddCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            AddCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Substract
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                SubstractCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                SubstractCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                SubstractCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            SubstractCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Multiply
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                MultiplyCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                MultiplyCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                MultiplyCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            MultiplyCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Divide
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                DivideCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                DivideCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                DivideCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T* ptr, T* ptrEnd, T value)
        {
            if (ptrEnd <= ptr)
                return;
            DivideCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value);
        }
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OrCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Or((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Or((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Or((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Or((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Or((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Or((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Or((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Or((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Or((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Or((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Or((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Or((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            OrCoreSlow(ptr, value, length);
        }

        private static void OrCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Or((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Or((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Or((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Or((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Or((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Or((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Or((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Or((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Or((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Or((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Or(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Or(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AndCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.And((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.And((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.And((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.And((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.And((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.And((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.And((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.And((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.And((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.And((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.And((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.And((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            AndCoreSlow(ptr, value, length);
        }

        private static void AndCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.And((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.And((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.And((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.And((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.And((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.And((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.And((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.And((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.And((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.And((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.And(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.And(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void XorCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Xor((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Xor((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Xor((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Xor((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Xor((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Xor((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Xor((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Xor((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Xor((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Xor((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Xor((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Xor((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            XorCoreSlow(ptr, value, length);
        }

        private static void XorCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Xor((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Xor((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Xor((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Xor((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Xor((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Xor((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Xor((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Xor((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Xor((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Xor((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Xor(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Xor(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Add((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Add((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Add((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Add((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Add((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Add((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Add((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Add((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Add((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Add((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Add((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Add((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            AddCoreSlow(ptr, value, length);
        }

        private static void AddCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Add((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Add((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Add((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Add((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Add((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Add((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Add((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Add((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Add((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Add((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Add(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Add(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SubstractCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Substract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Substract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Substract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Substract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Substract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Substract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Substract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Substract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Substract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Substract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Substract((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Substract((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            SubstractCoreSlow(ptr, value, length);
        }

        private static void SubstractCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Substract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Substract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Substract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Substract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Substract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Substract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Substract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Substract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Substract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Substract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Substract(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Substract(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MultiplyCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Multiply((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Multiply((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Multiply((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Multiply((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Multiply((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Multiply((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Multiply((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Multiply((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Multiply((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Multiply((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Multiply((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Multiply((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            MultiplyCoreSlow(ptr, value, length);
        }

        private static void MultiplyCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Multiply((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Multiply((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Multiply((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Multiply((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Multiply((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Multiply((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Multiply((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Multiply((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Multiply((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Multiply((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Multiply(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Multiply(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DivideCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Divide((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Divide((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Divide((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Divide((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Divide((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Divide((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Divide((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Divide((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Divide((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Divide((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Divide((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Divide((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            DivideCoreSlow(ptr, value, length);
        }

        private static void DivideCoreSlow<T>(T* ptr, T value, nuint length)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Divide((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Divide((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Divide((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Divide((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Divide((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Divide((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Divide((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Divide((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Divide((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Divide((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Divide(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Divide(ptr, length, value);
        }
        #endregion
    }
}
