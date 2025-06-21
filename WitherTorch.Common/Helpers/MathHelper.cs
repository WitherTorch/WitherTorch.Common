using System;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static partial class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Abs(sbyte value)
        {
            IL.Push(value);
            IL.Push(sizeof(sbyte) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Dup();
            IL.Push(value);
            IL.Emit.Add();
            IL.Emit.Xor();
            IL.Emit.Conv_I1();
            return IL.Return<sbyte>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Abs(short value)
        {
            IL.Push(value);
            IL.Push(sizeof(short) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Dup();
            IL.Push(value);
            IL.Emit.Add();
            IL.Emit.Xor();
            IL.Emit.Conv_I2();
            return IL.Return<short>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Abs(int value)
        {
            IL.Push(value);
            IL.Push(sizeof(int) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Dup();
            IL.Push(value);
            IL.Emit.Add();
            IL.Emit.Xor();
            return IL.Return<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Abs(long value)
        {
            IL.Push(value);
            IL.Push(sizeof(long) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Dup();
            IL.Push(value);
            IL.Emit.Add();
            IL.Emit.Xor();
            return IL.Return<long>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe nint Abs(nint value)
        {
            nint mask = value >> (sizeof(nint) * 8 - 1);
            return mask ^ (mask + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivRem(int a, int b, out int rem)
        {
            int result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DivRem(long a, long b, out long rem)
        {
            long result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DivRem(uint a, uint b, out uint rem)
        {
            uint result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DivRem(ulong a, ulong b, out ulong rem)
        {
            ulong result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint DivRem(nint a, nint b, out nint rem)
        {
            nint result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint DivRem(nuint a, nuint b, out nuint rem)
        {
            nuint result = a / b;
            rem = a - (result * b); // Multiply operation faster than modulo operation
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilDiv(int a, int b)
        {
            int result = DivRem(a, b, out int rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CeilDiv(uint a, uint b)
        {
            uint result = DivRem(a, b, out uint rem);
            return result + BooleanToUInt32(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CeilDiv(long a, long b)
        {
            long result = DivRem(a, b, out long rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CeilDiv(ulong a, ulong b)
        {
            ulong result = DivRem(a, b, out ulong rem);
            return result + BooleanToUInt64(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint CeilDiv(nint a, nint b)
        {
            nint result = DivRem(a, b, out nint rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CeilDiv(nuint a, nuint b)
        {
            nuint result = DivRem(a, b, out nuint rem);
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
    }
}
