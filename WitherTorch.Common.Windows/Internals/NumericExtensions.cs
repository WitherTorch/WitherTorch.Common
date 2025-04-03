using InlineMethod;

namespace WitherTorch.CrossNative.Windows.Internals
{
    internal static class NumericExtensions
    {
        [Inline(InlineBehavior.Remove)]
        public static int MakeSigned(this uint value) => unchecked((int)(value & int.MaxValue));

        [Inline(InlineBehavior.Remove)]
        public static uint MakeUnsigned(this int value) => unchecked((uint)(value & int.MaxValue));
    }
}
