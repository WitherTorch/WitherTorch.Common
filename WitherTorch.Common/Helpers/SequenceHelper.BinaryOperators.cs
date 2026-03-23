using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        #region Fill/Right
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                RightCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                RightCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                RightCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T* ptr, nuint length, T value) => RightCore(ptr, length, value);
        #endregion

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
        public static void Or<T>(T* ptr, nuint length, T value) => OrCore(ptr, length, value);
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
        public static void And<T>(T* ptr, nuint length, T value) => AndCore(ptr, length, value);
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
        public static void Xor<T>(T* ptr, nuint length, T value) => XorCore(ptr, length, value);
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
        public static void Add<T>(T* ptr, nuint length, T value) => AddCore(ptr, length, value);
        #endregion

        #region Subtract
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                SubtractCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                SubtractCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                SubtractCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract<T>(T* ptr, nuint length, T value) => SubtractCore(ptr, length, value);
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
        public static void Multiply<T>(T* ptr, nuint length, T value) => MultiplyCore(ptr, length, value);
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
        public static void Divide<T>(T* ptr, nuint length, T value) => DivideCore(ptr, length, value);
        #endregion

        #region Min
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                MinCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                MinCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                MinCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min<T>(T* ptr, nuint length, T value) => MinCore(ptr, length, value);
        #endregion

        #region Max
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max<T>(T[] array, T value)
        {
            fixed (T* ptr = array)
                MaxCore(ptr, MathHelper.MakeUnsigned(array.Length), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max<T>(T[] array, T value, int startIndex)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                MaxCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max<T>(T[] array, T value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                MaxCore(ptr + startIndex, unchecked((nuint)count), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max<T>(T* ptr, nuint length, T value) => MaxCore(ptr, length, value);
        #endregion

        #region Operate (Binary Operation)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, IBinaryOperator<T> @operator)
        {
            fixed (T* ptr = array)
                OperateCore(ptr, MathHelper.MakeUnsigned(array.Length), value, @operator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, BinaryOperation<T> operation)
        {
            fixed (T* ptr = array)
                OperateCore(ptr, MathHelper.MakeUnsigned(array.Length), value, operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, int startIndex, IBinaryOperator<T> @operator)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                OperateCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value, @operator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, int startIndex, BinaryOperation<T> operation)
        {
            int length = array.Length;
            if (startIndex < 0 || startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                OperateCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), value, operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, int startIndex, int count, IBinaryOperator<T> @operator)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                OperateCore(ptr + startIndex, unchecked((nuint)count), value, @operator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T[] array, T value, int startIndex, int count, BinaryOperation<T> operation)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                OperateCore(ptr + startIndex, unchecked((nuint)count), value, operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T* ptr, nuint length, T value, IBinaryOperator<T> @operator) => OperateCore(ptr, length, value, @operator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Operate<T>(T* ptr, nuint length, T value, BinaryOperation<T> operation) => OperateCore(ptr, length, value, operation);
        #endregion

        #region Core Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RightCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Right((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Right((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Right((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Right((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Right((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Right((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Right((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Right((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Right((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Right((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Right((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Right((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            RightCoreSlow(ptr, length, value);
        }

        private static void RightCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Right((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Right((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Right((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Right((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Right((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Right((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Right((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Right((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Right((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Right((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Right(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Right(ptr, length, value);
        }

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
            OrCoreSlow(ptr, length, value);
        }

        private static void OrCoreSlow<T>(T* ptr, nuint length, T value)
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
            AndCoreSlow(ptr, length, value);
        }

        private static void AndCoreSlow<T>(T* ptr, nuint length, T value)
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
            XorCoreSlow(ptr, length, value);
        }

        private static void XorCoreSlow<T>(T* ptr, nuint length, T value)
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
            AddCoreSlow(ptr, length, value);
        }

        private static void AddCoreSlow<T>(T* ptr, nuint length, T value)
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
        private static void SubtractCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Subtract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Subtract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Subtract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Subtract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Subtract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Subtract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Subtract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Subtract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Subtract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Subtract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Subtract((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Subtract((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            SubtractCoreSlow(ptr, length, value);
        }

        private static void SubtractCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Subtract((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Subtract((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Subtract((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Subtract((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Subtract((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Subtract((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Subtract((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Subtract((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Subtract((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Subtract((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Subtract(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Subtract(ptr, length, value);
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
            MultiplyCoreSlow(ptr, length, value);
        }

        private static void MultiplyCoreSlow<T>(T* ptr, nuint length, T value)
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
            DivideCoreSlow(ptr, length, value);
        }

        private static void DivideCoreSlow<T>(T* ptr, nuint length, T value)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MinCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Min((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Min((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Min((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Min((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Min((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Min((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Min((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Min((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Min((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Min((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Min((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Min((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            MinCoreSlow(ptr, length, value);
        }

        private static void MinCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Min((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Min((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Min((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Min((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Min((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Min((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Min((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Min((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Min((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Min((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Min(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Min(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MaxCore<T>(T* ptr, nuint length, T value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
            {
                FastCore<byte>.Max((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(sbyte))
            {
                FastCore<sbyte>.Max((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                return;
            }
            if (typeof(T) == typeof(short))
            {
                FastCore<short>.Max((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                return;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(ushort))
            {
                FastCore<ushort>.Max((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(int))
            {
                FastCore<int>.Max((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                return;
            }
            if (typeof(T) == typeof(uint))
            {
                FastCore<uint>.Max((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                return;
            }
            if (typeof(T) == typeof(long))
            {
                FastCore<long>.Max((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                return;
            }
            if (typeof(T) == typeof(ulong))
            {
                FastCore<ulong>.Max((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                return;
            }
            if (typeof(T) == typeof(float))
            {
                FastCore<float>.Max((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                return;
            }
            if (typeof(T) == typeof(double))
            {
                FastCore<double>.Max((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                FastCore.Max((nint*)ptr, length, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nuint))
            {
                FastCore.Max((nuint*)ptr, length, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            MaxCoreSlow(ptr, length, value);
        }

        private static void MaxCoreSlow<T>(T* ptr, nuint length, T value)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        FastCore<byte>.Max((byte*)ptr, length, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        FastCore<sbyte>.Max((sbyte*)ptr, length, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        FastCore<short>.Max((short*)ptr, length, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        FastCore<ushort>.Max((ushort*)ptr, length, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        FastCore<int>.Max((int*)ptr, length, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        FastCore<uint>.Max((uint*)ptr, length, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        FastCore<long>.Max((long*)ptr, length, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        FastCore<ulong>.Max((ulong*)ptr, length, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        FastCore<float>.Max((float*)ptr, length, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        FastCore<double>.Max((double*)ptr, length, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        SlowCore<T>.Max(ptr, length, value);
                        return;
                }
            }
            SlowCore<T>.Max(ptr, length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OperateCore<T>(T* ptr, nuint length, T value, BinaryOperation<T> operation) 
            => SlowCore<T>.BinaryOperationCore(ptr, length, value, operation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OperateCore<T>(T* ptr, nuint length, T value, IBinaryOperator<T> @operator)
        {
            if (@operator is IDefaultBinaryOperator<T> defaultOperator)
            {
                switch (defaultOperator.Type)
                {
                    case BinaryOperatorType.Left:
                        return;
                    case BinaryOperatorType.Right:
                        RightCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Or:
                        OrCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.And:
                        AndCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Xor:
                        XorCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Add:
                        AddCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Subtract:
                        SubtractCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Multiply:
                        MultiplyCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Divide:
                        DivideCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Min:
                        MinCore(ptr, length, value);
                        return;
                    case BinaryOperatorType.Max:
                        MaxCore(ptr, length, value);
                        return;
                    default:
                        break;
                }
            }
            SlowCore<T>.BinaryOperationCore(ptr, length, value, @operator);
        }
        #endregion
    }
}
