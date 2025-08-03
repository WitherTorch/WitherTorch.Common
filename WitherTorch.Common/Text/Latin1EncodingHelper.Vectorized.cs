using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Text
{
    partial class Latin1EncodingHelper
    {
        internal static unsafe partial byte* ReadFromUtf16BufferCore_OutOfLatin1Range(char* source, byte* destination, nuint length);

        internal static unsafe partial byte* ReadFromUtf16BufferCore(char* source, byte* destination, nuint length);

        internal static unsafe partial char* WriteToUtf16BufferCore(byte* source, char* destination, nuint length);
    }
}
