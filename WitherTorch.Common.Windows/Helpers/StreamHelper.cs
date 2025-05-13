using System.IO;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Windows.Internals;
using WitherTorch.Common.Windows.ObjectModels;

namespace WitherTorch.Common.Windows.Helpers
{
    public static class StreamHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IWin32Stream ToWin32Stream(Stream stream) => new CLRStreamToWin32Adapter(stream);
    }
}
