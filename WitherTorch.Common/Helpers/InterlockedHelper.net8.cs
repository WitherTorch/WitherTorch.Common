#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Threading;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class InterlockedHelper
    {
        /// <inheritdoc cref="Interlocked.Or(ref int, int)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial int Or(ref int location, int value) => Interlocked.Or(ref location, value);

        /// <inheritdoc cref="Interlocked.Or(ref uint, uint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint Or(ref uint location, uint value) => Interlocked.Or(ref location, value);

        /// <inheritdoc cref="Interlocked.Or(ref long, long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long Or(ref long location, long value) => Interlocked.Or(ref location, value);

        /// <inheritdoc cref="Interlocked.Or(ref ulong, ulong)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial ulong Or(ref ulong location, ulong value) => Interlocked.Or(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint Or(ref nint location, nint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => Or(ref UnsafeHelper.As<nint, int>(ref location), (int)value),
                sizeof(long) => (nint)Or(ref UnsafeHelper.As<nint, long>(ref location), value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => Or(ref UnsafeHelper.As<nint, int>(ref location), (int)value),
                    sizeof(long) => (nint)Or(ref UnsafeHelper.As<nint, long>(ref location), value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint Or(ref nuint location, nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => Or(ref UnsafeHelper.As<nuint, uint>(ref location), (uint)value),
                sizeof(ulong) => (nuint)Or(ref UnsafeHelper.As<nuint, ulong>(ref location), value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => Or(ref UnsafeHelper.As<nuint, uint>(ref location), (uint)value),
                    sizeof(ulong) => (nuint)Or(ref UnsafeHelper.As<nuint, ulong>(ref location), value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        /// <inheritdoc cref="Interlocked.And(ref int, int)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial int And(ref int location, int value) => Interlocked.And(ref location, value);

        /// <inheritdoc cref="Interlocked.And(ref uint, uint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint And(ref uint location, uint value) => Interlocked.And(ref location, value);

        /// <inheritdoc cref="Interlocked.And(ref long, long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long And(ref long location, long value) => Interlocked.And(ref location, value);

        /// <inheritdoc cref="Interlocked.And(ref ulong, ulong)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial ulong And(ref ulong location, ulong value) => Interlocked.And(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint And(ref nint location, nint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => And(ref UnsafeHelper.As<nint, int>(ref location), (int)value),
                sizeof(long) => (nint)And(ref UnsafeHelper.As<nint, long>(ref location), value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => And(ref UnsafeHelper.As<nint, int>(ref location), (int)value),
                    sizeof(long) => (nint)And(ref UnsafeHelper.As<nint, long>(ref location), value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint And(ref nuint location, nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => And(ref UnsafeHelper.As<nuint, uint>(ref location), (uint)value),
                sizeof(ulong) => (nuint)And(ref UnsafeHelper.As<nuint, ulong>(ref location), value),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => And(ref UnsafeHelper.As<nuint, uint>(ref location), (uint)value),
                    sizeof(ulong) => (nuint)And(ref UnsafeHelper.As<nuint, ulong>(ref location), value),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int Min(ref int location, int value) => MinCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial uint Min(ref uint location, uint value) => MinCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial long Min(ref long location, long value) => MinCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ulong Min(ref ulong location, ulong value) => MinCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint Min(ref nint location, nint value) => MinCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint Min(ref nuint location, nuint value) => MinCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial float Min(ref float location, float value) => MinCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial double Min(ref double location, double value) => MinCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int Max(ref int location, int value) => MaxCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial uint Max(ref uint location, uint value) => MaxCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial long Max(ref long location, long value) => MaxCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ulong Max(ref ulong location, ulong value) => MaxCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint Max(ref nint location, nint value) => MaxCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint Max(ref nuint location, nuint value) => MaxCoreUnsigned(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial float Max(ref float location, float value) => MaxCore(ref location, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial double Max(ref double location, double value) => MaxCore(ref location, value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int Read(ref readonly int location)
            => Interlocked.CompareExchange(ref UnsafeHelper.AsRefIn(in location), 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial uint Read(ref readonly uint location)
            => UnsafeHelper.As<int, uint>(Interlocked.CompareExchange(ref UnsafeHelper.As<uint, int>(ref UnsafeHelper.AsRefIn(in location)), 0, 0));

        /// <inheritdoc cref="Interlocked.Read(ref readonly long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long Read(ref readonly long location)
            => Interlocked.Read(ref UnsafeHelper.AsRefIn(in location));

        /// <inheritdoc cref="Interlocked.Read(ref readonly long)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ulong Read(ref readonly ulong location)
            => UnsafeHelper.As<long, ulong>(Interlocked.Read(ref UnsafeHelper.As<ulong, long>(ref UnsafeHelper.AsRefIn(in location))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint Read(ref readonly nint location) => UnsafeHelper.PointerSizeConstant switch
        {
            sizeof(int) => UnsafeHelper.As<int, nint>(Interlocked.CompareExchange(ref UnsafeHelper.As<nint, int>(ref UnsafeHelper.AsRefIn(in location)), 0, 0)),
            sizeof(long) => UnsafeHelper.As<long, nint>(Interlocked.Read(ref UnsafeHelper.As<nint, long>(ref UnsafeHelper.AsRefIn(in location)))),
            _ => UnsafeHelper.PointerSize switch
            {
                sizeof(int) => UnsafeHelper.As<int, nint>(Interlocked.CompareExchange(ref UnsafeHelper.As<nint, int>(ref UnsafeHelper.AsRefIn(in location)), 0, 0)),
                sizeof(long) => UnsafeHelper.As<long, nint>(Interlocked.Read(ref UnsafeHelper.As<nint, long>(ref UnsafeHelper.AsRefIn(in location)))),
                _ => throw new PlatformNotSupportedException()
            }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint Read(ref readonly nuint location) => UnsafeHelper.PointerSizeConstant switch
        {
            sizeof(uint) => UnsafeHelper.As<int, nuint>(Interlocked.CompareExchange(ref UnsafeHelper.As<nuint, int>(ref UnsafeHelper.AsRefIn(in location)), 0, 0)),
            sizeof(ulong) => UnsafeHelper.As<long, nuint>(Interlocked.Read(ref UnsafeHelper.As<nuint, long>(ref UnsafeHelper.AsRefIn(in location)))),
            _ => UnsafeHelper.PointerSize switch
            {
                sizeof(uint) => UnsafeHelper.As<int, nuint>(Interlocked.CompareExchange(ref UnsafeHelper.As<nuint, int>(ref UnsafeHelper.AsRefIn(in location)), 0, 0)),
                sizeof(ulong) => UnsafeHelper.As<long, nuint>(Interlocked.Read(ref UnsafeHelper.As<nuint, long>(ref UnsafeHelper.AsRefIn(in location)))),
                _ => throw new PlatformNotSupportedException()
            }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial float Read(ref readonly float location)
            => Interlocked.CompareExchange(ref UnsafeHelper.AsRefIn(in location), 0.0f, 0.0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial double Read(ref readonly double location)
             => Interlocked.CompareExchange(ref UnsafeHelper.AsRefIn(in location), 0.0, 0.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial object? Read(ref readonly object? location)
            => Interlocked.CompareExchange(ref UnsafeHelper.AsRefIn(in location), null, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T Read<T>(ref readonly T location) where T : class?
             => Interlocked.CompareExchange(ref UnsafeHelper.AsRefIn(in location)!, default, default)!;

        /// <inheritdoc cref="Interlocked.Exchange(ref int, int)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial int Exchange(ref int location, int value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref uint, uint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint Exchange(ref uint location, uint value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref long, long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long Exchange(ref long location, long value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref ulong, ulong)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial ulong Exchange(ref ulong location, ulong value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref nint, nint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial nint Exchange(ref nint location, nint value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref nuint, nuint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial nuint Exchange(ref nuint location, nuint value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref float, float)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial float Exchange(ref float location, float value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref double, double)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial double Exchange(ref double location, double value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange(ref object?, object?)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial object? Exchange(ref object? location, object? value)
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.Exchange{T}(ref T, T)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial T Exchange<T>(ref T location, T value) where T : class?
            => Interlocked.Exchange(ref location, value);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref int, int, int)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial int CompareExchange(ref int location, int value, int comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref uint, uint, uint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint CompareExchange(ref uint location, uint value, uint comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref long, long, long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long CompareExchange(ref long location, long value, long comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref ulong, ulong, ulong)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial ulong CompareExchange(ref ulong location, ulong value, ulong comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref nint, nint, nint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial nint CompareExchange(ref nint location, nint value, nint comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref nuint, nuint, nuint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial nuint CompareExchange(ref nuint location, nuint value, nuint comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref float, float, float)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial float CompareExchange(ref float location, float value, float comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref double, double, double)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial double CompareExchange(ref double location, double value, double comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange(ref object?, object?, object?)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial object? CompareExchange(ref object? location, object? value, object? comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial T CompareExchange<T>(ref T location, T value, T comparand) where T : class?
            => Interlocked.CompareExchange(ref location, value, comparand);

        /// <inheritdoc cref="Interlocked.Increment(ref int)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial int Increment(ref int location) => Interlocked.Increment(ref location);

        /// <inheritdoc cref="Interlocked.Increment(ref uint)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial uint Increment(ref uint location) => Interlocked.Increment(ref location);

        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial long Increment(ref long location) => Interlocked.Increment(ref location);

        /// <inheritdoc cref="Interlocked.Increment(ref ulong)"/>
        [Inline(InlineBehavior.Keep, export: true)]
        public static partial ulong Increment(ref ulong location) => Interlocked.Increment(ref location);

        /// <inheritdoc cref="Interlocked.Increment(ref long)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nint Increment(ref nint location)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => Increment(ref UnsafeHelper.As<nint, int>(ref location)),
                sizeof(long) => (nint)Increment(ref UnsafeHelper.As<nint, long>(ref location)),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => Increment(ref UnsafeHelper.As<nint, int>(ref location)),
                    sizeof(long) => (nint)Increment(ref UnsafeHelper.As<nint, long>(ref location)),
                    _ => throw new PlatformNotSupportedException()
                }
            };

        /// <inheritdoc cref="Interlocked.Increment(ref ulong)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint Increment(ref nuint location)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => Increment(ref UnsafeHelper.As<nuint, uint>(ref location)),
                sizeof(ulong) => (nuint)Increment(ref UnsafeHelper.As<nuint, ulong>(ref location)),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => Increment(ref UnsafeHelper.As<nuint, uint>(ref location)),
                    sizeof(ulong) => (nuint)Increment(ref UnsafeHelper.As<nuint, ulong>(ref location)),
                    _ => throw new PlatformNotSupportedException()
                }
            };
    }
}
#endif