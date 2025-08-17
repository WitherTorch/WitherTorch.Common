﻿using System;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

using LocalsInit;

using WitherTorch.Common.Intrinsics.X86;

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

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int DivRem(int a, int b, out int rem)
        {
#if NET8_0_OR_GREATER
            return Math.DivRem(a, b, out rem);
#else
            if (X86Base.IsSupported)
            {
                (int quotient, rem) = X86Base.DivRem(a, b);
                return quotient;
            }

            return DivRemSoftwareFallback(a, b, out rem);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DivRem(uint a, uint b, out uint rem)
        {
#if NET8_0_OR_GREATER
            (uint quotient, rem) = Math.DivRem(a, b);
            return quotient;
#else
            if (X86Base.IsSupported)
            {
                (uint quotient, rem) = X86Base.DivRem(a, b);
                return quotient;
            }

            return DivRemUnsignedSoftwareFallback(a, b, out rem);
#endif
        }

#if NET8_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static long DivRem(long a, long b, out long rem)
        {
#if NET8_0_OR_GREATER
            return Math.DivRem(a, b, out rem);
#else
            if (X86Base.X64.IsSupported)
            {
                (long quotient, rem) = X86Base.X64.DivRem((ulong)a, -BooleanToInt64(a < 0), b);
                return quotient;
            }
            if (X86Base.IsSupported && b <= int.MaxValue)
            {
                (int quotient, rem) = X86Base.DivRem(a, (int)b);
                return quotient;
            }

            return DivRemSoftwareFallback(a, b, out rem);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DivRem(ulong a, ulong b, out ulong rem)
        {
#if NET8_0_OR_GREATER
            (ulong quotient, rem) = Math.DivRem(a, b);
            return quotient;
#else
            if (X86Base.X64.IsSupported)
            {
                (ulong quotient, rem) = X86Base.X64.DivRem(a, 0, b);
                return quotient;
            }
            if (X86Base.IsSupported && b <= uint.MaxValue)
            {
                (uint quotient, rem) = X86Base.DivRem(a, (uint)b);
                return quotient;
            }

            return DivRemUnsignedSoftwareFallback(a, b, out rem);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint DivRem(nint a, nint b, out nint rem)
        {
#if NET8_0_OR_GREATER
            (nint quotient, rem) = Math.DivRem(a, b);
            return quotient;
#else
            return UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => DivRem((int)a, (int)b, out UnsafeHelper.As<nint, int>(ref UnsafeHelper.AsRefOut(out rem))),
                sizeof(long) => (nint)DivRem(a, b, out UnsafeHelper.As<nint, long>(ref UnsafeHelper.AsRefOut(out rem))),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => DivRem((int)a, (int)b, out UnsafeHelper.As<nint, int>(ref UnsafeHelper.AsRefOut(out rem))),
                    sizeof(long) => (nint)DivRem(a, b, out UnsafeHelper.As<nint, long>(ref UnsafeHelper.AsRefOut(out rem))),
                    _ => DivRemSoftwareFallback(a, b, out rem)
                }
            };
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint DivRem(nuint a, nuint b, out nuint rem)
        {
#if NET8_0_OR_GREATER
            (nuint quotient, rem) = Math.DivRem(a, b);
            return quotient;
#else
            return UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => DivRem((uint)a, (uint)b, out UnsafeHelper.As<nuint, uint>(ref UnsafeHelper.AsRefOut(out rem))),
                sizeof(ulong) => (nuint)DivRem(a, b, out UnsafeHelper.As<nuint, ulong>(ref UnsafeHelper.AsRefOut(out rem))),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => DivRem((uint)a, (uint)b, out UnsafeHelper.As<nuint, uint>(ref UnsafeHelper.AsRefOut(out rem))),
                    sizeof(ulong) => (nuint)DivRem(a, b, out UnsafeHelper.As<nuint, ulong>(ref UnsafeHelper.AsRefOut(out rem))),
                    _ => DivRemUnsignedSoftwareFallback(a, b, out rem)
                }
            };
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilDiv(int a, int b)
        {
            int result = DivRemSoftwareFallback(a, b, out int rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CeilDiv(uint a, uint b)
        {
            uint result = DivRemUnsignedSoftwareFallback(a, b, out uint rem);
            return result + BooleanToUInt32(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CeilDiv(long a, long b)
        {
            long result = DivRemSoftwareFallback(a, b, out long rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CeilDiv(ulong a, ulong b)
        {
            ulong result = DivRemUnsignedSoftwareFallback(a, b, out ulong rem);
            return result + BooleanToUInt64(rem > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint CeilDiv(nint a, nint b)
        {
            nint result = DivRemSoftwareFallback(a, b, out nint rem);
            return result + Sign(rem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint CeilDiv(nuint a, nuint b)
        {
            nuint result = DivRemUnsignedSoftwareFallback(a, b, out nuint rem);
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
        public static int LeadingZeroCount(nuint value)
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
        public static int TrailingZeroCount(nuint value)
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
        public static int Log2(nuint value)
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

        [LocalsInit(false)]
        private static T DivRemSoftwareFallback<T>(T a, T b, out T rem) where T : unmanaged
        {
            T quantity = UnsafeHelper.Divide(a, b);
            rem = UnsafeHelper.Substract(a, UnsafeHelper.Multiply(b, quantity));
            return quantity;
        }

        [LocalsInit(false)]
        private static T DivRemUnsignedSoftwareFallback<T>(T a, T b, out T rem) where T : unmanaged
        {
            T quantity = UnsafeHelper.DivideUnsigned(a, b);
            rem = UnsafeHelper.Substract(a, UnsafeHelper.Multiply(b, quantity));
            return quantity;
        }
    }
}
