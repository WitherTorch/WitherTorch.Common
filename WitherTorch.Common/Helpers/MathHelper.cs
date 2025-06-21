using System;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    public static partial class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Abs(sbyte value) => unchecked((sbyte)AbsCore(value, sizeof(sbyte) * 8 - 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Abs(short value) => unchecked((short)AbsCore(value, sizeof(short) * 8 - 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Abs(int value) => AbsCore(value, sizeof(int) * 8 - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Abs(long value) => AbsCore(value, sizeof(long) * 8 - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe nint Abs(nint value) => AbsCore(value, sizeof(nint) * 8 - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivRem(int a, int b, out int rem) => DivRemCore(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DivRem(long a, long b, out long rem) => DivRemCoreUnsigned(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DivRem(uint a, uint b, out uint rem) => DivRemCore(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DivRem(ulong a, ulong b, out ulong rem) => DivRemCoreUnsigned(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint DivRem(nint a, nint b, out nint rem) => DivRemCore(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint DivRem(nuint a, nuint b, out nuint rem) => DivRemCoreUnsigned(a, b, out rem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilDiv(int a, int b)
        {
            int result = DivRemCore(a, b, out int rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CeilDiv(uint a, uint b)
        {
            uint result = DivRemCoreUnsigned(a, b, out uint rem);
            return result + BooleanToUInt32(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CeilDiv(long a, long b)
        {
            long result = DivRemCore(a, b, out long rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CeilDiv(ulong a, ulong b)
        {
            ulong result = DivRemCoreUnsigned(a, b, out ulong rem);
            return result + BooleanToUInt64(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint CeilDiv(nint a, nint b)
        {
            nint result = DivRemCore(a, b, out nint rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CeilDiv(nuint a, nuint b)
        {
            nuint result = DivRemCoreUnsigned(a, b, out nuint rem);
            return result + BooleanToUInt32(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Pow([InlineParameter] int _base, [InlineParameter] int pow)
        {
            if (_base == 2)
                return 1 << pow;
            if (pow < 0 || pow > byte.MaxValue)
                return (int)Math.Pow(_base, pow);
            return (int)PowCore(_base, unchecked((byte)pow));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Pow([InlineParameter] long _base, [InlineParameter] long pow)
        {
            if (_base == 2 && pow <= int.MaxValue)
                return 1L << unchecked((int)pow);
            if (pow < 0 || pow > byte.MaxValue)
                return (long)Math.Pow(_base, pow);
            return PowCore(_base, unchecked((byte)pow));
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int LeadingZeroCount(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(value);
#else
            return LeadingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int LeadingZeroCount(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(value);
#else
            return LeadingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int TrailingZeroCount(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(value);
#else
            return TrailingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int TrailingZeroCount(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(value);
#else
            return TrailingZeroCountCore(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int Log2(ulong value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.Log2(value);
#else
            return Log2Core(value);
#endif
        }

#if NETCOREAPP3_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int Log2(uint value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Numerics.BitOperations.Log2(value);
#else
            return Log2Core(value);
#endif
        }

        [Inline(InlineBehavior.Remove)]
        private static T AbsCore<T>(T value, T offset) where T : unmanaged
        {
            IL.Push(value);
            IL.Push(offset);
            IL.Emit.Shr();
            IL.Emit.Dup();
            IL.Push(value);
            IL.Emit.Add();
            IL.Emit.Xor();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Remove)]
        [LocalsInit(false)]
        private static T DivRemCore<T>(T a, T b, out T rem) where T : unmanaged
        {
            T quantity = UnsafeHelper.Divide(a, b);
            rem = UnsafeHelper.Substract(a, UnsafeHelper.Multiply(b, quantity));
            return quantity;
        }

        [Inline(InlineBehavior.Remove)]
        [LocalsInit(false)]
        private static T DivRemCoreUnsigned<T>(T a, T b, out T rem) where T : unmanaged
        {
            T quantity = UnsafeHelper.DivideUnsigned(a, b);
            rem = UnsafeHelper.Substract(a, UnsafeHelper.Multiply(b, quantity));
            return quantity;
        }
    }
}
