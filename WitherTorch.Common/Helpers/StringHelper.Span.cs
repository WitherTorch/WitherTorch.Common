using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static unsafe class StringHelperExtensions
    {
#if !NET8_0_OR_GREATER
        extension(StringHelper)
        {
#endif
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsAny(string str, params ReadOnlySpan<char> values)
            {
                for (int i = 0, valuesLength = values.Length; i < valuesLength; i++)
                {
                    if (StringHelper.Contains(str, values[i]))
                        return true;
                }
                return false;
            }
#if !NET8_0_OR_GREATER
        }
#endif
    }
}
