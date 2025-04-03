using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Max(sbyte val1, sbyte val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Bgt_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Max(byte val1, byte val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Bgt_Un_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Max(short val1, short val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Bgt_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Max(ushort val1, ushort val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Bgt_Un_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Max(uint val1, uint val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long val1, long val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Max(ulong val1, ulong val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Max(decimal val1, decimal val2)
        {
            return Math.Max(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2)
        {
            return Math.Max(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2)
        {
            return Math.Max(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Min(sbyte val1, sbyte val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Blt_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Min(byte val1, byte val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Blt_Un_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Min(short val1, short val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Blt_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Min(ushort val1, ushort val2)
        {
            IL.Push(val1);
            IL.Push(val2);
            IL.Emit.Blt_Un_S("AltReturn");
            IL.Push(val2);
            IL.Emit.Ret();
            IL.MarkLabel("AltReturn");
            IL.Push(val1);
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Min(uint val1, uint val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long val1, long val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Min(ulong val1, ulong val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Min(decimal val1, decimal val2)
        {
            return Math.Min(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2)
        {
            return Math.Min(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2)
        {
            return Math.Min(val1, val2);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Clamp(byte value, byte min, byte max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Clamp(short value, short min, short max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Clamp(ushort value, ushort min, ushort max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Clamp(uint value, uint min, uint max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Clamp(ulong value, ulong min, ulong max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
#if NET6_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Max(min, Min(max, value));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
#if NET6_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Max(min, Min(max, value));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min, double max)
        {
#if NET6_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Max(min, Min(max, value));
#endif
        }
    }
}
