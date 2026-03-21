using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        [LocalsInit(false)]
        private static partial class FastCore { }

        [LocalsInit(false)]
        private static partial class FastCore<T> where T : unmanaged { }

        [LocalsInit(false)]
        private static partial class SlowCore { }

        [LocalsInit(false)]
        private static partial class SlowCore<T> { }
    }
}
