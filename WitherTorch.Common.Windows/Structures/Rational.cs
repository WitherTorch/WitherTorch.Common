using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = sizeof(uint) * 2)]
    public readonly struct Rational : IEquatable<Rational>
    {
        public static readonly Rational Zero = new Rational(0, 1);
        public static readonly Rational One = new Rational(1, 1);

        public readonly uint Numerator, Denominator;

        public Rational(uint numerator, uint denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Rational left, Rational right) =>
            UnsafeHelper.As<Rational, ulong>(ref UnsafeHelper.AsRefIn(left)) == UnsafeHelper.As<Rational, ulong>(ref UnsafeHelper.AsRefIn(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rational left, Rational right) =>
            UnsafeHelper.As<Rational, ulong>(ref UnsafeHelper.AsRefIn(left)) != UnsafeHelper.As<Rational, ulong>(ref UnsafeHelper.AsRefIn(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int operator *(int left, Rational right) => (int)(left * right.Numerator / right.Denominator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint operator *(uint left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long operator *(long left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong operator *(ulong left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator *(nint left, Rational right) => (nint)(left * right.Numerator / right.Denominator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator *(nuint left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator *(float left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double operator *(double left, Rational right) => left * right.Numerator / right.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int operator /(int left, Rational right) => (int)(left * right.Denominator / right.Numerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint operator /(uint left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long operator /(long left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong operator /(ulong left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator /(nint left, Rational right) => (nint)(left * right.Denominator / right.Numerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator /(nuint left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator /(float left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double operator /(double left, Rational right) => left * right.Denominator / right.Numerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? other) => other is Rational rational && Equals(rational);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Rational other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => unchecked((int)(Numerator ^ Denominator));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => $"{{ {nameof(Numerator)} = {Numerator:D}, {nameof(Denominator)} = {Denominator:D} }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out uint numerator, out uint denominator)
        {
            numerator = Numerator;
            denominator = Denominator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(Rational r) => r.Numerator * 1.0 / r.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(Rational r) => r.Numerator * 1.0f / r.Denominator;
    }
}
