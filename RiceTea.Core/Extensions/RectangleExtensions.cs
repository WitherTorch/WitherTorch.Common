using System.Drawing;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace RiceTea.Core.Extensions;

public static class RectangleExtensions
{
    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this Rectangle rectangle, in PointF point) => rectangle.Contains(point.X, point.Y);

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this Rectangle rectangle, float x, float y)
    {
        if (rectangle.X <= x && x < rectangle.Right && rectangle.Y <= y)
        {
            return y < rectangle.Bottom;
        }
        return false;
    }
}
