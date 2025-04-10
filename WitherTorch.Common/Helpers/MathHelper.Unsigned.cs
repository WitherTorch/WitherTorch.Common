using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static byte MakeUnsigned([InlineParameter] sbyte value) => unchecked((byte)(value & sbyte.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort MakeUnsigned([InlineParameter] short value) => unchecked((ushort)(value & short.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned([InlineParameter] int value) => unchecked((uint)(value & int.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned([InlineParameter] long value) => unchecked((ulong)(value & long.MaxValue));
    }
}
