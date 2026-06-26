using InlineMethod;

namespace RiceTea.Core.Helpers;

#pragma warning disable CS8500

public static unsafe partial class SequenceHelper
{
    [Inline(InlineBehavior.Remove)]
    private static int ConvertToIndex32<T>(T* ptrResult, T* ptrBase)
        => ptrResult < ptrBase ? -1 : unchecked((int)(ptrResult - ptrBase));

    [Inline(InlineBehavior.Remove)]
    private static nuint? ConvertToIndexNative<T>(T* ptrResult, T* ptrBase)
        => ptrResult < ptrBase ? null : unchecked((nuint)(ptrResult - ptrBase));
}
