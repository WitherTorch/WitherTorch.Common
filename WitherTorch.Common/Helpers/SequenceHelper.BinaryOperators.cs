using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Structures;

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
        private static partial Unit LeftCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit RightCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit OrCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit AndCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit XorCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit AddCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit SubtractCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit MultiplyCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit DivideCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit MinCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static partial Unit MaxCore<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OperateCore<T>(T* ptr, nuint length, T value, BinaryOperation<T> operation)
            => SlowCore<T>.BinaryOperationCore(ptr, length, value, operation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OperateCore<T>(T* ptr, nuint length, T value, IBinaryOperator<T> @operator)
        {
            if (@operator is IDefaultBinaryOperator<T> defaultOperator)
            {
                _ = defaultOperator.Type switch
                {
                    BinaryOperatorType.Left => LeftCore(ptr, length, value),
                    BinaryOperatorType.Right => RightCore(ptr, length, value),
                    BinaryOperatorType.Or => OrCore(ptr, length, value),
                    BinaryOperatorType.And => AndCore(ptr, length, value),
                    BinaryOperatorType.Xor => XorCore(ptr, length, value),
                    BinaryOperatorType.Add => AddCore(ptr, length, value),
                    BinaryOperatorType.Subtract => SubtractCore(ptr, length, value),
                    BinaryOperatorType.Multiply => MultiplyCore(ptr, length, value),
                    BinaryOperatorType.Divide => DivideCore(ptr, length, value),
                    BinaryOperatorType.Min => MinCore(ptr, length, value),
                    BinaryOperatorType.Max => MaxCore(ptr, length, value),
                    _ => Unit.Default
                };
                return;
            }
            SlowCore<T>.BinaryOperationCore(ptr, length, value, @operator);
        }
        #endregion
    }
}
