using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Structures
{
    public struct PointD : IEquatable<PointD>
    {
        public double X;
        public double Y;

        public PointD(in PointD original) : this(original.X, original.Y) { }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PointF(PointD point) => new PointF((float)point.X, (float)point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD(PointF point) => new PointD(point.X, point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) => obj is PointD other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PointD other) => X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly unsafe int GetHashCode()
        {
            double x = X, y = Y;
            return unchecked((*(long*)&x ^ *(long*)&y).GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => $"{{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PointD a, PointD b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointD a, PointD b) => !a.Equals(b);
    }
}
