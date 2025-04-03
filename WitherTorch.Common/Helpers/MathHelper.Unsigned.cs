using InlineMethod;

using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static byte MakeUnsigned(sbyte value) => unchecked((byte)(value & sbyte.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort MakeUnsigned(short value) => unchecked((ushort)(value & short.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned(int value) => unchecked((uint)(value & int.MaxValue));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned(long value) => unchecked((ulong)(value & long.MaxValue));
    }
}
