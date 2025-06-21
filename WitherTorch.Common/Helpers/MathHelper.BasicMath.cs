using System;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static byte CheckedAdd(byte a, byte b)
            => unchecked((byte)(a + Min(b, (byte)(byte.MaxValue - a))));

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort CheckedAdd(ushort a, ushort b)
            => unchecked((ushort)(a + Min(b, (ushort)(ushort.MaxValue - a))));

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CheckedAdd(uint a, uint b)
            => a + Min(b, uint.MaxValue - a);

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CheckedAdd(ulong a, ulong b)
            => a + Min(b, ulong.MaxValue - a);

        [Inline(InlineBehavior.Keep, export: true)]
        [LocalsInit(false)]
        public static nuint CheckedAdd(nuint a, nuint b)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => CheckedAdd((uint)a, (uint)b),
                sizeof(ulong) => (nuint)CheckedAdd((ulong)a, b),
                _ => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => CheckedAdd((uint)a, (uint)b),
                    sizeof(ulong) => (nuint)CheckedAdd((ulong)a, b),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + UnsafeHelper.PointerSize)
                }
            };

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte CheckedSubstract(byte a, byte b)
        {
#if NET8_0_OR_GREATER
            return a > b ? unchecked((byte)(a - b)) : byte.MinValue;
#else
            return unchecked((byte)(a - Min(a, b)));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort CheckedSubstract(ushort a, ushort b)
        {
#if NET8_0_OR_GREATER
            return a > b ? unchecked((ushort)(a - b)) : ushort.MinValue;
#else
            return unchecked((ushort)(a - Min(a, b)));
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint CheckedSubstract(uint a, uint b)
        {
#if NET8_0_OR_GREATER
            return a > b ? a - b : uint.MinValue;
#else
            return a - Min(a, b);
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong CheckedSubstract(ulong a, ulong b)
        {
#if NET8_0_OR_GREATER
            return a > b ? a - b : ulong.MinValue;
#else
            return a - Min(a, b);
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CheckedSubstract(nuint a, nuint b)
        {
#if NET8_0_OR_GREATER
            return a > b ? a - b : nuint.MinValue;
#else
            return a - Min(a, b);
#endif
        }
    }
}
