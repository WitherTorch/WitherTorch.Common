using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using RiceTea.Core.Collections;
using RiceTea.Core.Helpers;

namespace RiceTea.Core.Buffers;

public abstract partial class ArrayPool<T> : IPool<T[]>
{
    protected const uint MinimumArraySize = 16;

    private static readonly ArrayPool<T> _shared = CreateSharedPool();
    private static readonly bool _isUnmanagedType = UnsafeHelper.IsUnmanagedType<T>();

    public static ArrayPool<T> Empty => EmptyImpl.Instance;
    public static ArrayPool<T> Shared => _shared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RentScope EnterRentScope() => new RentScope(this, Array.Empty<T>(), count: 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RentScope EnterRentScope(int capacity) => new RentScope(this, Rent(capacity), count: capacity);

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent() => Rent(MinimumArraySize);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(unchecked((nuint)capacity));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(long capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(unchecked((nuint)capacity));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(nint capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(unchecked((nuint)capacity));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(uint capacity)
    {
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > (uint)Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(capacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(ulong capacity)
    {
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > (ulong)Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(unchecked((nuint)capacity));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(nuint capacity)
    {
        if (capacity == 0)
            return Array.Empty<T>();
        if (capacity > (nuint)Limits.MaxArrayLength)
            return new T[capacity]; // Allocate a array larger than MaxArrayLength will throw exception
        return RentCore(capacity);
    }

    protected abstract T[] RentCore(nuint capacity);

    public void Return(T[] array)
    {
        int length = array.Length;
        if (length <= 0)
            return;
        if (!_isUnmanagedType)
            Array.Clear(array, 0, length);
        ReturnCore(array);
    }

    public unsafe void Return(T[] array, bool clearArray = false)
    {
        int length = array.Length;
        if (length <= 0)
            return;
        if (clearArray)
        {
            if (_isUnmanagedType)
            {
#pragma warning disable CS8500
                fixed (void* ptr = array)
                    UnsafeHelper.InitBlock(ptr, 0, unchecked((uint)length * UnsafeHelper.SizeOf<T>()));
#pragma warning restore CS8500
            }
            else
            {
                Array.Clear(array, 0, length);
            }
        }
        ReturnCore(array);
    }

    protected abstract void ReturnCore(T[] array);

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FixedArrayList<T> RentList() => new FixedArrayList<T>(Rent(), initialCount: 0);

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FixedArrayList<T> RentList(int capacity) => new FixedArrayList<T>(Rent(capacity), initialCount: 0);

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReturnList(FixedArrayList<T> list) => Return(list.AsArray());

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReturnList(FixedArrayList<T> list, bool clearArray) => Return(list.AsArray(), clearArray);

    private static partial ArrayPool<T> CreateSharedPool();
}