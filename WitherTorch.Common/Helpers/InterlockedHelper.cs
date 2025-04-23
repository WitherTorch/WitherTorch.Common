using System.Threading;

using InlineMethod;

#if !NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace WitherTorch.Common.Helpers
{
    public static class InterlockedHelper
    {
#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int Or(ref int location, int value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.Or(ref location, value);
#else
            int oldValue = Interlocked.CompareExchange(ref location, value, 0);
            if (oldValue == 0 || (oldValue & value) == value)
                return oldValue;
            do
            {
                int currentValue = Interlocked.CompareExchange(ref location, oldValue | value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static long Or(ref long location, long value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.Or(ref location, value);
#else
            long oldValue = Interlocked.CompareExchange(ref location, value, 0);
            if (oldValue == 0 || (oldValue & value) == value)
                return oldValue;
            do
            {
                long currentValue = Interlocked.CompareExchange(ref location, oldValue | value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static uint Or(ref uint location, uint value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.Or(ref location, value);
#else
            uint oldValue = CompareExchange(ref location, value, 0);
            if (oldValue == 0 || (oldValue & value) == value)
                return oldValue;
            do
            {
                uint currentValue = CompareExchange(ref location, oldValue | value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static ulong Or(ref ulong location, ulong value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.Or(ref location, value);
#else
            ulong oldValue = CompareExchange(ref location, value, 0);
            if (oldValue == 0 || (oldValue & value) == value)
                return oldValue;
            do
            {
                ulong currentValue = CompareExchange(ref location, oldValue | value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int And(ref int location, int value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.And(ref location, value);
#else
            int oldValue = Read(ref location);
            if (oldValue == value || (oldValue & ~value) == 0)
                return oldValue;
            do
            {
                int currentValue = Interlocked.CompareExchange(ref location, oldValue & value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static long And(ref long location, long value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.And(ref location, value);
#else
            long oldValue = Read(ref location);
            if (oldValue == value || (oldValue & ~value) == 0)
                return oldValue;
            do
            {
                long currentValue = Interlocked.CompareExchange(ref location, oldValue & value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static uint And(ref uint location, uint value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.And(ref location, value);
#else
            uint oldValue = Read(ref location);
            if (oldValue == value || (oldValue & ~value) == 0)
                return oldValue;
            do
            {
                uint currentValue = CompareExchange(ref location, oldValue & value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static ulong And(ref ulong location, ulong value)
        {
#if NET8_0_OR_GREATER
            return Interlocked.And(ref location, value);
#else
            ulong oldValue = Read(ref location);
            if (oldValue == value || (oldValue & ~value) == 0)
                return oldValue;
            do
            {
                ulong currentValue = CompareExchange(ref location, oldValue & value, oldValue);
                if (currentValue == oldValue)
                    return oldValue;
                oldValue = currentValue;
            }
            while (true);
#endif
        }

        public static int Min(ref int location, int value)
        {
            int oldValue = Read(ref location);
            if (oldValue <= value)
                return oldValue;
            do
            {
                int currentValue = Interlocked.CompareExchange(ref location, value, oldValue);
                if (currentValue == oldValue)
                    break;
                oldValue = currentValue;
            }
            while (oldValue > value);
            return oldValue;
        }

        public static long Min(ref long location, long value)
        {
            long oldValue = Read(ref location);
            if (oldValue <= value)
                return oldValue;
            do
            {
                long currentValue = Interlocked.CompareExchange(ref location, value, oldValue);
                if (currentValue == oldValue)
                    break;
                oldValue = currentValue;
            }
            while (oldValue > value);
            return oldValue;
        }

        public static int Max(ref int location, int value)
        {
            int oldValue = Read(ref location);
            if (oldValue >= value)
                return oldValue;
            do
            {
                int currentValue = Interlocked.CompareExchange(ref location, value, oldValue);
                if (currentValue == oldValue)
                    break;
                oldValue = currentValue;
            }
            while (oldValue < value);
            return oldValue;
        }

        public static long Max(ref long location, long value)
        {
            long oldValue = Read(ref location);
            if (oldValue >= value)
                return oldValue;
            do
            {
                long currentValue = Interlocked.CompareExchange(ref location, value, oldValue);
                if (currentValue == oldValue)
                    break;
                oldValue = currentValue;
            }
            while (oldValue < value);
            return oldValue;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Read(ref int location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static long Read(ref long location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint Read(ref uint location) => CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong Read(ref ulong location) => CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Read(ref float location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static double Read(ref double location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? Read<T>(ref T? location) where T : class
            => Interlocked.CompareExchange(ref location, default, default);

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CompareExchange(ref uint location, uint value, uint comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CompareExchange(ref uint location, uint value, uint comparand)
            => unchecked((uint)Interlocked.CompareExchange(ref UnsafeHelper.As<uint, int>(ref location), (int)value, (int)comparand));
#endif

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CompareExchange(ref ulong location, ulong value, ulong comparand)
            => Interlocked.CompareExchange(ref location, value, comparand);
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CompareExchange(ref ulong location, ulong value, ulong comparand)
            => unchecked((ulong)Interlocked.CompareExchange(ref UnsafeHelper.As<ulong, long>(ref location), (long)value, (long)comparand));
#endif

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static uint Exchange(ref uint location, uint value)
            => Interlocked.Exchange(ref location, value);
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Exchange(ref uint location, uint value)
            => unchecked((uint)Interlocked.Exchange(ref UnsafeHelper.As<uint, int>(ref location), (int)value));
#endif

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong Exchange(ref ulong location, ulong value)
            => Interlocked.Exchange(ref location, value);
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Exchange(ref ulong location, ulong value)
            => unchecked((ulong)Interlocked.Exchange(ref UnsafeHelper.As<ulong, long>(ref location), (long)value));
#endif
    }
}
