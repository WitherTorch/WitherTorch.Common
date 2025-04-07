
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Point16
    {
        private readonly short _x, _y;

        public short X => _x;

        public short Y => _y;

        public Point16(short x, short y)
        {
            _x = x;
            _y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point ToPoint32()
        {
            return new Point(_x, _y);
        }

        public override readonly int GetHashCode()
            => unchecked(_x ^ _y);

        public override readonly string ToString()
            => $"{nameof(X)} = {_x}, {nameof(Y)} = {_y}";
    }
}