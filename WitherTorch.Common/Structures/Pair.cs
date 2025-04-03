using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Pair<T> : ITuple where T : unmanaged
    {
        public readonly T Value1;
        public readonly T Value2;

        public Pair(T value1, T value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        object ITuple.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Value1;
                    case 1: return Value2;
                    default: throw new InvalidOperationException();
                }
            }
        }

        int ITuple.Length => 2;
    }
}
