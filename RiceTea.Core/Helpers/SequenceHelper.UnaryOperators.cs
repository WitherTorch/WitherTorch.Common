using System;
using System.Runtime.CompilerServices;

using RiceTea.Core.Structures;

namespace RiceTea.Core.Helpers;

unsafe partial class SequenceHelper
{
#pragma warning disable CS8500

    #region Or
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
            ArgumentOutOfRangeException.Throw(nameof(startIndex));
        fixed (T* ptr = array)
            NotCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not<T>(T[] array, int startIndex, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        int length = startIndex + count;
        if (length > array.Length)
            ArgumentOutOfRangeException.Throw(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
        fixed (T* ptr = array)
            NotCore(ptr + startIndex, unchecked((nuint)count));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not<T>(T* ptr, nuint length) => NotCore(ptr, length);
    #endregion

    #region Operate (Unary Operation)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, IUnaryOperator<T> @operator)
    {
        fixed (T* ptr = array)
            OperateCore(ptr, MathHelper.MakeUnsigned(array.Length), @operator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, UnaryOperation<T> operation)
    {
        fixed (T* ptr = array)
            OperateCore(ptr, MathHelper.MakeUnsigned(array.Length), operation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, int startIndex, IUnaryOperator<T> @operator)
    {
        int length = array.Length;
        if (startIndex < 0 || startIndex >= length)
            ArgumentOutOfRangeException.Throw(nameof(startIndex));
        fixed (T* ptr = array)
            OperateCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), @operator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, int startIndex, UnaryOperation<T> operation)
    {
        int length = array.Length;
        if (startIndex < 0 || startIndex >= length)
            ArgumentOutOfRangeException.Throw(nameof(startIndex));
        fixed (T* ptr = array)
            OperateCore(ptr + startIndex, MathHelper.MakeUnsigned(length - startIndex), operation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, int startIndex, int count, IUnaryOperator<T> @operator)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        int length = startIndex + count;
        if (length > array.Length)
            ArgumentOutOfRangeException.Throw(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
        fixed (T* ptr = array)
            OperateCore(ptr + startIndex, unchecked((nuint)count), @operator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T[] array, int startIndex, int count, UnaryOperation<T> operation)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        int length = startIndex + count;
        if (length > array.Length)
            ArgumentOutOfRangeException.Throw(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
        fixed (T* ptr = array)
            OperateCore(ptr + startIndex, unchecked((nuint)count), operation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T* ptr, nuint length, IUnaryOperator<T> @operator) => OperateCore(ptr, length, @operator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Operate<T>(T* ptr, nuint length, UnaryOperation<T> operation) => OperateCore(ptr, length, operation);
    #endregion

    #region Core Methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static partial Unit IdentityCore<T>(T* ptr, nuint length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static partial Unit NotCore<T>(T* ptr, nuint length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void OperateCore<T>(T* ptr, nuint length, UnaryOperation<T> operation)
        => SlowCore<T>.UnaryOperationCore(ptr, length, operation);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void OperateCore<T>(T* ptr, nuint length, IUnaryOperator<T> @operator)
    {
        if (@operator is IDefaultUnaryOperator<T> defaultOperator)
        {
            _ = defaultOperator.Type switch
            {
                UnaryOperatorType.Identity => IdentityCore(ptr, length),
                UnaryOperatorType.Not => NotCore(ptr, length),
                _ => Unit.Default
            };
            return;
        }
        SlowCore<T>.UnaryOperationCore(ptr, length, @operator);
    }
    #endregion
}
