using InlineMethod;

using WitherTorch.Common.Threading;


namespace WitherTorch.Common.Extensions
{
    public static partial class StringExtensions
    {
        private const ushort UpperLowerDiff = 'a' - 'A';

        [Inline(InlineBehavior.Remove)]
        private static unsafe partial string? ToLowerOrUpperAsciiCore(char* ptr, char* ptrEnd, [InlineParameter] bool isUpper);

        [Inline(InlineBehavior.Remove)]
        private static unsafe void LegacyToLowerOrUpperAsciiCore(char* ptr, char* ptrStart, ref LazyTinyRefStruct<string> resultLazy, [InlineParameter] bool isUpper)
        {
            char c = *ptr;
            if (isUpper)
            {
                if (c < 'a' || c > 'z')
                    return;
            }
            else
            {
                if (c < 'A' || c > 'Z')
                    return;
            }
            fixed (char* ptrResult = resultLazy.Value)
                *(ptrResult + (ptr - ptrStart)) =
                    isUpper ? unchecked((char)(c - UpperLowerDiff)) : unchecked((char)(c + UpperLowerDiff));
        }
    }
}
