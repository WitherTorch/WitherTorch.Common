using System.Drawing;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.Common.Windows.Structures
{
    public struct PointU
    {
        public uint X;
        public uint Y;

        public PointU(in PointU original) : this(original.X, original.Y) { }

        public PointU(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PointU(Point point) => new PointU(point.X.MakeUnsigned(), point.Y.MakeUnsigned());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point(PointU point) => new Point(point.X.MakeSigned(), point.Y.MakeSigned());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) => obj is PointU other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PointU other) => X == other.X && Y == other.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => unchecked((int)(X ^ Y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => $"{{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PointU a, PointU b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointU a, PointU b) => !a.Equals(b);
    }
}
