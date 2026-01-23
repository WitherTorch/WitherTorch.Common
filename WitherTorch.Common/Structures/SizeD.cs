using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Structures
{
    public struct SizeD : IEquatable<SizeD>
    {
        public double Width;
        public double Height;

        public SizeD(in SizeD original) : this(original.Width, original.Height) { }

        public SizeD(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static explicit operator SizeF(SizeD size) => new SizeF((float)size.Width, (float)size.Height);

        public static implicit operator SizeD(SizeF size) => new SizeD(size.Width, size.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double width, out double height)
        {
            width = Width;
            height = Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) => obj is SizeD other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(SizeD other) => Width == other.Width && Height == other.Height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly unsafe int GetHashCode()
        {
            double width = Width, height = Height;
            return unchecked((*(long*)&width ^ *(long*)&height).GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() 
            => $"{{ {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(SizeD left, SizeD right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(SizeD left, SizeD right) => !left.Equals(right);
    }
}
