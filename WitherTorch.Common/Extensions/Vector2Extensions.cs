using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Extensions
{
    public static class Vector2Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct(this Vector2 _this, out float x, out float y)
        {
            x = _this.X;
            y = _this.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct(this Point _this, out int x, out int y)
        {
            x = _this.X;
            y = _this.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct(this PointF _this, out float x, out float y)
        {
            x = _this.X;
            y = _this.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct(this Size _this, out int width, out int height)
        {
            width = _this.Width;
            height = _this.Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct(this SizeF _this, out float width, out float height)
        {
            width = _this.Width;
            height = _this.Height;
        }
    }
}
