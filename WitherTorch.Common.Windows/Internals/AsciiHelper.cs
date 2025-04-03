using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Windows.Internals
{
    internal static class AsciiHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToAsciiUnchecked(byte[] destination, string source)
        {
            for (int i = 0, length = source.Length; i < length; i++)
                destination[i] = unchecked((byte)source[i]);    
        }
    }
}
