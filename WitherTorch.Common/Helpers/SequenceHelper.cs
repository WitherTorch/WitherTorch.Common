using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class SequenceHelper
    {
        [Inline(InlineBehavior.Remove)]
        private static T* WithOffset<T>(T* ptr, [InlineParameter] int offset) where T : unmanaged
        {
            if (offset == 0)
                return ptr;
            return ptr + offset;
        }
    }
}
