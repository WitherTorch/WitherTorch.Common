using System;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static byte MakeUnsigned(sbyte value)
        {
            IL.Push(value);
            IL.Push(sizeof(sbyte) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            IL.Emit.Conv_U1();
            return IL.Return<byte>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort MakeUnsigned(short value)
        {
            IL.Push(value);
            IL.Push(sizeof(short) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            IL.Emit.Conv_U2();
            return IL.Return<ushort>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned(int value)
        {
            IL.Push(value);
            IL.Push(sizeof(int) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            return IL.Return<uint>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned(long value)
        {
            IL.Push(value);
            IL.Push(sizeof(long) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            return IL.Return<uint>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
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
