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
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Read(ref int location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static long Read(ref long location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Read(ref float location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static double Read(ref double location) => Interlocked.CompareExchange(ref location, default, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? Read<T>(ref T? location) where T : class
            => Interlocked.CompareExchange(ref location, default, default);
    }
}
