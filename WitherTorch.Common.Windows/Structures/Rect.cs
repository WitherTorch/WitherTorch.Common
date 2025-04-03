using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Rect(in Rect original) : this(original.Left, original.Top, original.Right, original.Bottom) { }

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect FromXYWH(int x, int y, int width, int height) => new Rect(x, y, x + width, y + height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rect(Rectangle rectangle) => new Rect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rect(RectangleF rectangle) => new Rect((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Right, (int)rectangle.Bottom);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rect(RectF rectangle) => new Rect((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Right, (int)rectangle.Bottom);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rect(RectU rectangle) => new Rect(rectangle.Left.MakeSigned(), rectangle.Top.MakeSigned(),
            rectangle.Right.MakeSigned(), rectangle.Bottom.MakeSigned());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rectangle(Rect rectangle) => Rectangle.FromLTRB(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator RectangleF(Rect rectangle) => Rectangle.FromLTRB(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        public int X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => Left; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Left = value;
        }

        public int Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => Top; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Top = value;
        }

        public Point Location
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new Point(Left, Top);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Size Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new Size(Width, Height);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public readonly Point TopLeft => new Point(Left, Top);
        public readonly Point TopRight => new Point(Right, Top);
        public readonly Point BottomLeft => new Point(Left, Bottom);
        public readonly Point BottomRight => new Point(Right, Bottom);

        public int Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => Bottom - Top;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Bottom = Top + value;
        }

        public int Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => Right - Left;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Right = Left + value;
        }

        public readonly bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => IsEmptyLocation && IsEmptySize;
        }

        public readonly bool IsEmptyLocation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Left == 0 && Top == 0;
        }

        public readonly bool IsEmptySize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Left == Right && Top == Bottom;
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Left < Right && Top < Bottom;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(int x, int y) => x >= Left && y >= Top && x <= Right && y <= Bottom;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(float x, float y) => x >= Left && y >= Top && x <= Right && y <= Bottom;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Point point) => Contains(point.X, point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(PointF point) => Contains(point.X, point.Y); 
        
        public readonly bool Contains(in Rectangle rect)
        {
            if (X <= rect.X && rect.X + rect.Width <= Right && Y <= rect.Y)
            {
                return rect.Y + rect.Height <= Bottom;
            }

            return false;
        }

        public readonly bool Contains(in RectangleF rect)
        {
            if (X <= rect.X && rect.X + rect.Width <= Right && Y <= rect.Y)
            {
                return rect.Y + rect.Height <= Bottom;
            }

            return false;
        }

        public readonly bool Contains(in Rect rect)
        {
            if (X <= rect.X && rect.Right <= Right && Y <= rect.Y)
            {
                return rect.Bottom <= Bottom;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) => obj is Rect other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(in Rect other)
            => X == other.X && Y == other.Y && Right == other.Right && Bottom == other.Bottom;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
            => Left ^ Top ^ Right ^ Bottom;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => $"{{ {nameof(Left)} = {Left}, {nameof(Top)} = {Top}, {nameof(Right)} = {Right}, {nameof(Bottom)} = {Bottom}) }}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Rect a, in Rect b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Rect a, in Rect b) => !a.Equals(b);
    }
}
