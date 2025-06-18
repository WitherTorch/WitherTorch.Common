using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte MakeUnsigned(sbyte value) => unchecked((byte)MakeUnsigned((int)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort MakeUnsigned(short value) => unchecked((ushort)MakeUnsigned((int)value));

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned(int value)
            => unchecked((uint)(value & (0 - BooleanToInt32(value > 0))));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned(long value)
            => unchecked((uint)(value & (0L - BooleanToInt64(value > 0))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint MakeUnsigned(nint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => MakeUnsigned(unchecked((int)value)),
                sizeof(long) => unchecked((nuint)MakeUnsigned(unchecked((long)value))),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => MakeUnsigned(unchecked((int)value)),
                    sizeof(long) => unchecked((nuint)MakeUnsigned(unchecked((long)value))),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + UnsafeHelper.PointerSize),
                },
            };
    }
}
