using System;

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
#if NET8_0_OR_GREATER
            const string JumpLabel = "ZeroReturn";

            IL.Emit.Ldc_I4_0();
            IL.Emit.Blt(JumpLabel);
            IL.Push(value);
            IL.Emit.Ret();

            IL.MarkLabel(JumpLabel);
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
#else
            IL.Push(sizeof(sbyte) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            IL.Emit.Conv_U1();
            return IL.Return<byte>();
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort MakeUnsigned(short value)
        {
            IL.Push(value);
#if NET8_0_OR_GREATER
            const string JumpLabel = "ZeroReturn";

            IL.Emit.Ldc_I4_0();
            IL.Emit.Blt(JumpLabel);
            IL.Push(value);
            IL.Emit.Ret();

            IL.MarkLabel(JumpLabel);
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
#else
            IL.Push(sizeof(short) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            IL.Emit.Conv_U2();
            return IL.Return<ushort>();
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint MakeUnsigned(int value)
        {
            IL.Push(value);
#if NET8_0_OR_GREATER
            const string JumpLabel = "ZeroReturn";

            IL.Emit.Ldc_I4_0();
            IL.Emit.Blt(JumpLabel);
            IL.Push(value);
            IL.Emit.Ret();

            IL.MarkLabel(JumpLabel);
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
#else
            IL.Push(sizeof(int) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            return IL.Return<uint>();
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong MakeUnsigned(long value)
        {
            IL.Push(value);
#if NET8_0_OR_GREATER
            const string JumpLabel = "ZeroReturn";

            IL.Emit.Ldc_I4_0();
            IL.Emit.Blt(JumpLabel);
            IL.Push(value);
            IL.Emit.Ret();

            IL.MarkLabel(JumpLabel);
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
#else
            IL.Push(sizeof(long) * 8 - 1);
            IL.Emit.Shr();
            IL.Emit.Not();
            IL.Push(value);
            IL.Emit.And();
            return IL.Return<uint>();
#endif
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
