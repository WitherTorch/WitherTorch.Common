#if NET8_0_OR_GREATER
using InlineMethod;

namespace WitherTorch.Common.Text
{
    partial class CLRStringHelper
    {
        [Inline(InlineBehavior.Remove)]
        public static partial ref readonly char GetPinnableReference(string source)
            => ref source.GetPinnableReference();
    }
}
#endif