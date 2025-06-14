using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static byte MakeUnsigned(sbyte value) => unchecked((byte)Abs(value));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort MakeUnsigned(short value) => unchecked((ushort)Abs(value));

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned(int value) => unchecked((uint)Abs(value));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned(long value) => unchecked((ulong)Abs(value));

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint MakeUnsigned(nint value) => unchecked((nuint)Abs(value));
    }
}
