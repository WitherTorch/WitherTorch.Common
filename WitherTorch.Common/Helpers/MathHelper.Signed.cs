using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte MakeSigned(byte value) => unchecked((sbyte)(value & sbyte.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static short MakeSigned(ushort value) => unchecked((short)(value & short.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static int MakeSigned(uint value) => unchecked((int)(value & int.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static long MakeSigned(ulong value) => unchecked((long)(value & long.MaxValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint MakeSigned(nuint value)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => MakeSigned(unchecked((uint)value)),
                sizeof(ulong) => unchecked((nint)MakeSigned(unchecked((ulong)value))),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => MakeSigned(unchecked((uint)value)),
                    sizeof(ulong) => unchecked((nint)MakeSigned(unchecked((ulong)value))),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + UnsafeHelper.PointerSize),
                },
            };
    }
}
