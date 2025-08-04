using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static partial class InterlockedHelper
    {
        public static partial int Or(ref int location, int value);
        public static partial uint Or(ref uint location, uint value);
        public static partial long Or(ref long location, long value);
        public static partial ulong Or(ref ulong location, ulong value);
        public static partial nint Or(ref nint location, nint value);
        public static partial nuint Or(ref nuint location, nuint value);

        public static partial int And(ref int location, int value);
        public static partial uint And(ref uint location, uint value);
        public static partial long And(ref long location, long value);
        public static partial ulong And(ref ulong location, ulong value);
        public static partial nint And(ref nint location, nint value);
        public static partial nuint And(ref nuint location, nuint value);

        public static partial int Min(ref int location, int value);
        public static partial uint Min(ref uint location, uint value);
        public static partial long Min(ref long location, long value);
        public static partial ulong Min(ref ulong location, ulong value);
        public static partial nint Min(ref nint location, nint value);
        public static partial nuint Min(ref nuint location, nuint value);
        public static partial float Min(ref float location, float value);
        public static partial double Min(ref double location, double value);

        public static partial int Max(ref int location, int value);
        public static partial uint Max(ref uint location, uint value);
        public static partial long Max(ref long location, long value);
        public static partial ulong Max(ref ulong location, ulong value);
        public static partial nint Max(ref nint location, nint value);
        public static partial nuint Max(ref nuint location, nuint value);
        public static partial float Max(ref float location, float value);
        public static partial double Max(ref double location, double value);

        public static partial int Read(ref readonly int location);
        public static partial uint Read(ref readonly uint location);
        public static partial long Read(ref readonly long location);
        public static partial ulong Read(ref readonly ulong location);
        public static partial nint Read(ref readonly nint location);
        public static partial nuint Read(ref readonly nuint location);
        public static partial float Read(ref readonly float location);
        public static partial double Read(ref readonly double location);
        public static partial object? Read(ref readonly object? location);
        public static partial T Read<T>(ref readonly T location) where T : class?;

        public static partial int Exchange(ref int location, int value);
        public static partial uint Exchange(ref uint location, uint value);
        public static partial long Exchange(ref long location, long value);
        public static partial ulong Exchange(ref ulong location, ulong value);
        public static partial nint Exchange(ref nint location, nint value);
        public static partial nuint Exchange(ref nuint location, nuint value);
        public static partial float Exchange(ref float location, float value);
        public static partial double Exchange(ref double location, double value);

        [return: NotNullIfNotNull(nameof(location))]
        public static partial object? Exchange([NotNullIfNotNull(nameof(value))] ref object? location, object? value);
        [return: NotNullIfNotNull(nameof(location))]
        public static partial T Exchange<T>([NotNullIfNotNull(nameof(value))] ref T location, T value) where T : class?;

        public static partial int CompareExchange(ref int location, int value, int comparand);
        public static partial uint CompareExchange(ref uint location, uint value, uint comparand);
        public static partial long CompareExchange(ref long location, long value, long comparand);
        public static partial ulong CompareExchange(ref ulong location, ulong value, ulong comparand);
        public static partial nint CompareExchange(ref nint location, nint value, nint comparand);
        public static partial nuint CompareExchange(ref nuint location, nuint value, nuint comparand);
        public static partial float CompareExchange(ref float location, float value, float comparand);
        public static partial double CompareExchange(ref double location, double value, double comparand);

        [return: NotNullIfNotNull(nameof(location))]
        public static partial object? CompareExchange([NotNullIfNotNull(nameof(value))] ref object? location, object? value, object? comparand);

        [return: NotNullIfNotNull(nameof(location))]
        public static partial T CompareExchange<T>([NotNullIfNotNull(nameof(value))] ref T location, T value, T comparand) where T : class?;

        /// <inheritdoc cref="Interlocked.Increment(ref int)"/>
        public static partial int Increment(ref int location);
        /// <inheritdoc cref="Interlocked.Increment(ref int)"/>
        public static partial uint Increment(ref uint location);
        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        public static partial long Increment(ref long location);
        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        public static partial ulong Increment(ref ulong location);
        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        public static partial nint Increment(ref nint location);
        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        public static partial nuint Increment(ref nuint location);

        // Slow routines
        [Inline(InlineBehavior.Remove)]
        private static T OrCore<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = CompareExchangeCore(ref location, value, default);
            if (UnsafeHelper.Equals(oldValue, default) || UnsafeHelper.Equals(UnsafeHelper.And(oldValue, value), value))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, UnsafeHelper.Or(oldValue, value), oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
        }

        [Inline(InlineBehavior.Remove)]
        private static T AndCore<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = ReadCore(ref location);
            if (UnsafeHelper.Equals(oldValue, value) || UnsafeHelper.Equals(UnsafeHelper.And(oldValue, UnsafeHelper.Not(value)), default))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, UnsafeHelper.And(oldValue, value), oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
        }

        [Inline(InlineBehavior.Remove)]
        private static T MinCore<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = ReadCore(ref location);
            if (UnsafeHelper.IsLessOrEqualsThan(oldValue, value))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, value, oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    break;
                oldValue = currentValue;
            }
            while (UnsafeHelper.IsGreaterThan(oldValue, value));
            return oldValue;
        }

        [Inline(InlineBehavior.Remove)]
        private static T MinCoreUnsigned<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = ReadCore(ref location);
            if (UnsafeHelper.IsLessOrEqualsThanUnsigned(oldValue, value))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, value, oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    break;
                oldValue = currentValue;
            }
            while (UnsafeHelper.IsGreaterThanUnsigned(oldValue, value));
            return oldValue;
        }

        [Inline(InlineBehavior.Remove)]
        private static T MaxCore<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = ReadCore(ref location);
            if (UnsafeHelper.IsGreaterOrEqualsThan(oldValue, value))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, value, oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    break;
                oldValue = currentValue;
            }
            while (UnsafeHelper.IsLessThan(oldValue, value));
            return oldValue;
        }

        [Inline(InlineBehavior.Remove)]
        private static T MaxCoreUnsigned<T>(ref T location, T value) where T : unmanaged
        {
            T oldValue = ReadCore(ref location);
            if (UnsafeHelper.IsGreaterOrEqualsThanUnsigned(oldValue, value))
                return oldValue;
            do
            {
                T currentValue = CompareExchangeCore(ref location, value, oldValue);
                if (UnsafeHelper.Equals(currentValue, oldValue))
                    break;
                oldValue = currentValue;
            }
            while (UnsafeHelper.IsLessThanUnsigned(oldValue, value));
            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ReadCore<T>(ref T location) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<int, T>(Read(ref UnsafeHelper.As<T, int>(ref location)));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<uint, T>(Read(ref UnsafeHelper.As<T, uint>(ref location)));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<long, T>(Read(ref UnsafeHelper.As<T, long>(ref location)));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<ulong, T>(Read(ref UnsafeHelper.As<T, ulong>(ref location)));
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<nint, T>(Read(ref UnsafeHelper.As<T, nint>(ref location)));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<nuint, T>(Read(ref UnsafeHelper.As<T, nuint>(ref location)));
            if (typeof(T) == typeof(float))
                return UnsafeHelper.As<float, T>(Read(ref UnsafeHelper.As<T, float>(ref location)));
            if (typeof(T) == typeof(double))
                return UnsafeHelper.As<double, T>(Read(ref UnsafeHelper.As<T, double>(ref location)));
            throw new PlatformNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ExchangeCore<T>(ref T location, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<int, T>(Exchange(ref UnsafeHelper.As<T, int>(ref location),
                    UnsafeHelper.As<T, int>(value)));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<uint, T>(Exchange(ref UnsafeHelper.As<T, uint>(ref location),
                    UnsafeHelper.As<T, uint>(value)));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<long, T>(Exchange(ref UnsafeHelper.As<T, long>(ref location),
                    UnsafeHelper.As<T, long>(value)));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<ulong, T>(Exchange(ref UnsafeHelper.As<T, ulong>(ref location),
                    UnsafeHelper.As<T, ulong>(value)));
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<nint, T>(Exchange(ref UnsafeHelper.As<T, nint>(ref location),
                    UnsafeHelper.As<T, nint>(value)));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<nuint, T>(Exchange(ref UnsafeHelper.As<T, nuint>(ref location),
                    UnsafeHelper.As<T, nuint>(value)));
            if (typeof(T) == typeof(float))
                return UnsafeHelper.As<float, T>(Exchange(ref UnsafeHelper.As<T, float>(ref location),
                    UnsafeHelper.As<T, float>(value)));
            if (typeof(T) == typeof(double))
                return UnsafeHelper.As<double, T>(Exchange(ref UnsafeHelper.As<T, double>(ref location),
                    UnsafeHelper.As<T, double>(value)));
            throw new PlatformNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T CompareExchangeCore<T>(ref T location, T value, T comparand) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<int, T>(CompareExchange(ref UnsafeHelper.As<T, int>(ref location),
                    UnsafeHelper.As<T, int>(value), UnsafeHelper.As<T, int>(comparand)));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<uint, T>(CompareExchange(ref UnsafeHelper.As<T, uint>(ref location),
                    UnsafeHelper.As<T, uint>(value), UnsafeHelper.As<T, uint>(comparand)));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<long, T>(CompareExchange(ref UnsafeHelper.As<T, long>(ref location),
                    UnsafeHelper.As<T, long>(value), UnsafeHelper.As<T, long>(comparand)));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<ulong, T>(CompareExchange(ref UnsafeHelper.As<T, ulong>(ref location),
                    UnsafeHelper.As<T, ulong>(value), UnsafeHelper.As<T, ulong>(comparand)));
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<nint, T>(CompareExchange(ref UnsafeHelper.As<T, nint>(ref location),
                    UnsafeHelper.As<T, nint>(value), UnsafeHelper.As<T, nint>(comparand)));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<nuint, T>(CompareExchange(ref UnsafeHelper.As<T, nuint>(ref location),
                    UnsafeHelper.As<T, nuint>(value), UnsafeHelper.As<T, nuint>(comparand)));
            if (typeof(T) == typeof(float))
                return UnsafeHelper.As<float, T>(CompareExchange(ref UnsafeHelper.As<T, float>(ref location),
                    UnsafeHelper.As<T, float>(value), UnsafeHelper.As<T, float>(comparand)));
            if (typeof(T) == typeof(double))
                return UnsafeHelper.As<double, T>(CompareExchange(ref UnsafeHelper.As<T, double>(ref location),
                    UnsafeHelper.As<T, double>(value), UnsafeHelper.As<T, double>(comparand)));
            throw new PlatformNotSupportedException();
        }
    }
}
