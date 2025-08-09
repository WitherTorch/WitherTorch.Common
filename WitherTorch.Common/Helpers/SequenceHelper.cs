using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500
        [Inline(InlineBehavior.Remove)]
        private static int ConvertToIndex32<T>(T* ptrResult, T* ptrBase)
            => ptrResult < ptrBase ? -1 : unchecked((int)(ptrResult - ptrBase));

        [Inline(InlineBehavior.Remove)]
        private static nuint? ConvertToIndexNative<T>(T* ptrResult, T* ptrBase)
            => ptrResult < ptrBase ? null : unchecked((nuint)(ptrResult - ptrBase));
    }
}
