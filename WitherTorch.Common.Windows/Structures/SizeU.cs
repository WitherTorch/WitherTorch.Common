using System.Drawing;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.Common.Windows.Structures
{
    public struct SizeU
    {
        public uint Width;
        public uint Height;

        public SizeU(in SizeU original) : this(original.Width, original.Height) { }

        public SizeU(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public static explicit operator SizeU(Size size) => new SizeU(size.Width.MakeUnsigned(), size.Height.MakeUnsigned());

        public static explicit operator Size(SizeU size) => new Size(size.Width.MakeSigned(), size.Height.MakeSigned());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) => obj is PointU other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(SizeU other) => Width == other.Width && Height == other.Height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() 
            => unchecked((int)(Width ^ Height));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() 
            => $"{{ {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(SizeU left, SizeU right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(SizeU left, SizeU right) => !left.Equals(right);
    }
}
