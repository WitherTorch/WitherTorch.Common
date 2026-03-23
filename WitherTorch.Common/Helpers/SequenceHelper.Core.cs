using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        [LocalsInit(false)]
        private static partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static bool IsIdempotence([InlineParameter] UnaryOperatorType type) => type == UnaryOperatorType.Identity;

            [Inline(InlineBehavior.Remove)]
            public static bool IsIdempotence([InlineParameter] BinaryOperatorType type) =>
                type is BinaryOperatorType.Left or BinaryOperatorType.Right or
                BinaryOperatorType.Or or BinaryOperatorType.And or
                BinaryOperatorType.Min or BinaryOperatorType.Max;
        }

        [LocalsInit(false)]
        private static partial class FastCore<T> where T : unmanaged { }

        [LocalsInit(false)]
        private static partial class SlowCore { }

        [LocalsInit(false)]
        private static partial class SlowCore<T> { }
    }
}
