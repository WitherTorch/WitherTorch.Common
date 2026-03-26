using System;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte Max(sbyte val1, sbyte val2)
            => UnsafeHelper.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte Max(byte val1, byte val2)
            => UnsafeHelper.MaxUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static short Max(short val1, short val2)
            => UnsafeHelper.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort Max(ushort val1, ushort val2)
            => UnsafeHelper.MaxUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Max(int val1, int val2)
            => UnsafeHelper.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint Max(uint val1, uint val2)
            => UnsafeHelper.MaxUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static long Max(long val1, long val2)
            => UnsafeHelper.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong Max(ulong val1, ulong val2)
            => UnsafeHelper.MaxUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nint Max(nint val1, nint val2)
            => UnsafeHelper.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint Max(nuint val1, nuint val2)
            => UnsafeHelper.MaxUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void* Max(void* val1, void* val2)
            => unchecked((void*)Max((nuint)val1, (nuint)val2));

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* Max<T>(T* val1, T* val2)
            => unchecked((T*)Max((nuint)val1, (nuint)val2));

        [Inline(InlineBehavior.Keep, export: true)]
        public static decimal Max(decimal val1, decimal val2)
            => Math.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Max(float val1, float val2)
            => Math.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static double Max(double val1, double val2)
            => Math.Max(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte Min(sbyte val1, sbyte val2)
            => UnsafeHelper.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte Min(byte val1, byte val2)
            => UnsafeHelper.MinUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static short Min(short val1, short val2)
            => UnsafeHelper.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort Min(ushort val1, ushort val2)
            => UnsafeHelper.MinUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Min(int val1, int val2)
            => UnsafeHelper.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint Min(uint val1, uint val2)
            => UnsafeHelper.MinUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static long Min(long val1, long val2)
            => UnsafeHelper.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong Min(ulong val1, ulong val2)
            => UnsafeHelper.MinUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nint Min(nint val1, nint val2)
            => UnsafeHelper.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint Min(nuint val1, nuint val2)
            => UnsafeHelper.MinUnsigned(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void* Min(void* val1, void* val2)
            => unchecked((void*)Min((nuint)val1, (nuint)val2));

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* Min<T>(T* val1, T* val2)
            => unchecked((T*)Min((nuint)val1, (nuint)val2));

        [Inline(InlineBehavior.Keep, export: true)]
        public static decimal Min(decimal val1, decimal val2)
            => Math.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Min(float val1, float val2)
            => Math.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static double Min(double val1, double val2)
            => Math.Min(val1, val2);

        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
            => ClampCore(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte Clamp(byte value, byte min, byte max)
            => ClampCoreUnsigned(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static short Clamp(short value, short min, short max)
            => ClampCore(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort Clamp(ushort value, ushort min, ushort max)
            => ClampCoreUnsigned(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Clamp(int value, int min, int max)
            => ClampCore(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint Clamp(uint value, uint min, uint max)
            => ClampCoreUnsigned(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static long Clamp(long value, long min, long max)
            => ClampCore(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong Clamp(ulong value, ulong min, ulong max)
            => ClampCoreUnsigned(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nint Clamp(nint value, nint min, nint max)
            => ClampCore(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint Clamp(nuint value, nuint min, nuint max)
            => ClampCoreUnsigned(value, min, max);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void* Clamp(void* value, void* min, void* max)
            => unchecked((void*)Clamp((nuint)value, (nuint)min, (nuint)max));

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* Clamp<T>(T* value, T* min, T* max)
            => unchecked((T*)Clamp((nuint)value, (nuint)min, (nuint)max));

        [Inline(InlineBehavior.Keep, export: true)]
        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
#if NET8_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Min(Max(value, min), max);
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static float Clamp(float value, float min, float max)
        {
#if NET8_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Min(Max(value, min), max);
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static double Clamp(double value, double min, double max)
        {
#if NET8_0_OR_GREATER
            return Math.Clamp(value, min, max);
#else
            return Min(Max(value, min), max);
#endif
        }

        [Inline(InlineBehavior.Remove)]
        private static T ClampCore<T>(T value, T min, T max) where T : unmanaged 
            => UnsafeHelper.Min(UnsafeHelper.Max(value, min), max);

        [Inline(InlineBehavior.Remove)]
        private static T ClampCoreUnsigned<T>(T value, T min, T max) where T : unmanaged 
            => UnsafeHelper.MinUnsigned(UnsafeHelper.MaxUnsigned(value, min), max);
    }
}
