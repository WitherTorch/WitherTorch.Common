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
    }
}
