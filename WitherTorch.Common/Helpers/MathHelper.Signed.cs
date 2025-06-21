using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static int Sign(sbyte value)
        {
            const int ByteOffset = sizeof(sbyte) * 8 - 1;
            return unchecked(value >> ByteOffset | (int)((uint)-value >> ByteOffset));
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Sign(short value)
        {
            const int ByteOffset = sizeof(short) * 8 - 1;
            return unchecked(value >> ByteOffset | (int)((uint)-value >> ByteOffset));
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Sign(int value)
        {
#if NET8_0_OR_GREATER
            return Math.Sign(value);
#else
            const int ByteOffset = sizeof(int) * 8 - 1;
            return unchecked(value >> ByteOffset | (int)((uint)-value >> ByteOffset));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Sign(long value)
        {
#if NET8_0_OR_GREATER
            return Math.Sign(value);
#else
            const int ByteOffset = sizeof(long) * 8 - 1;
            return unchecked((int)(value >> ByteOffset | (long)((ulong)-value >> ByteOffset)));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int Sign(nint value)
        {
#if NET8_0_OR_GREATER
            return Math.Sign(value);
#else
            return UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(int) => Sign(unchecked((int)value)),
                sizeof(long) => Sign(unchecked((long)value)),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(int) => Sign(unchecked((int)value)),
                    sizeof(long) => Sign(unchecked((long)value)),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + UnsafeHelper.PointerSize),
                },
            };
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte MakeSigned(byte value) => unchecked((sbyte)(value & sbyte.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static short MakeSigned(ushort value) => unchecked((short)(value & short.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static int MakeSigned(uint value) => unchecked((int)(value & int.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static long MakeSigned(ulong value) => unchecked((long)(value & long.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
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
