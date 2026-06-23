#if NET472_OR_GREATER
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

using WitherTorch.Common;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

#pragma warning disable IDE0130
namespace System;
#pragma warning restore IDE0130

public static class MathF
{
    private static readonly bool _isSystemMemoryExists = WTCommon.SystemBuffersExists;
    private static readonly float[] _power10 = new float[] { 1f, 10f, 100f, 1000f, 10000f, 100000f, 1000000f };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CopySign(float x, float y)
    {
        const uint SignMask = 0x80000000;

        IL.Emit.Ldarga_S(nameof(x));
        IL.Emit.Dup();
        IL.Emit.Dup();
        IL.Emit.Ldind_I4();
        IL.Push(~SignMask);
        IL.Emit.And();
        IL.Emit.Ldarga_S(nameof(y));
        IL.Emit.Ldind_I4();
        IL.Push(SignMask);
        IL.Emit.And();
        IL.Emit.Or();
        IL.Emit.Stind_I4();
        IL.Emit.Ldind_R4();
        return IL.Return<float>();
    }

    /// <inheritdoc cref="Math.Floor(double)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Floor(float d)
    {
        const float Magic = 8388608.0f;

        if (d < Magic && d > -Magic)
        {
            float z = (d + Magic) - Magic;
            return z - MathHelper.BooleanToFloat32(z > d);
        }

        return d;
    }

    /// <inheritdoc cref="Math.Floor(double)"/>
    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Floor(double d) => unchecked((float)Math.Floor(d));

    /// <inheritdoc cref="Math.Ceiling(double)"/>
    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ceiling(float a)
    {
        const float Magic = 8388608.0f;

        if (a > 0.0f)
        {
            if (a < Magic)
            {
                float z = (a + Magic) - Magic;
                return z + MathHelper.BooleanToFloat32(z < a);
            }
            return a;
        }

        if (a < 0.0f)
        {
            if (a > -1.0f) return -0.0f;
            if (a > -Magic)
            {
                float z = (a - Magic) + Magic;
                return z + MathHelper.BooleanToFloat32(z < a);
            }
        }

        return a;
    }

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ceiling(double a) => unchecked((float)Math.Ceiling(a));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float a)
    {
        const float IntegerBoundary = 8388608.0f; // 2^23

        if (Math.Abs(a) >= IntegerBoundary)
            return a;

        float temp = CopySign(IntegerBoundary, a);
        return CopySign((a + temp) - temp, a);
    }

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float value, int digits) => Round(value, digits, mode: MidpointRounding.ToEven);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float value, MidpointRounding mode)
        => mode switch
        {
            MidpointRounding.AwayFromZero => Truncate(value + CopySign(0.49999997f, value)),
            MidpointRounding.ToEven => Round(value),
            _ => throw new ArgumentOutOfRangeException(nameof(mode)),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float value, int digits, MidpointRounding mode)
    {
        const uint MaxRoundingDigits = 6;
        const float RoundingLimit = 1e8f;

        if (digits > MaxRoundingDigits)
            return Throw();

        if (Math.Abs(value) < RoundingLimit)
        {
            float power10 = _power10.AsUnsafeRef()[digits];
            value = Round(value * power10, mode) / power10;
        }

        return value;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static float Throw() => throw new ArgumentOutOfRangeException(nameof(digits));
    }

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(double a) => unchecked((float)Math.Round(a));

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(double value, int digit) => unchecked((float)Math.Round(value, digit));

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(double value, MidpointRounding mode) => unchecked((float)Math.Round(value, mode));

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(double value, int digit, MidpointRounding mode) => unchecked((float)Math.Round(value, digit, mode));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Truncate(float d)
    {
        const float Magic = 8388608.0f; // 2^23
        float absX = Math.Abs(d);

        if (absX < Magic)
        {
            float temp = CopySign(Magic, d);
            float rounded = (d + temp) - temp;
            float isEnlarged = MathHelper.BooleanToFloat32(Math.Abs(rounded) > absX);
            return rounded - CopySign(isEnlarged, d);
        }

        return d;
    }
}
#endif
