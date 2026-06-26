using System.IO;
using System.Runtime.CompilerServices;

using RiceTea.Core.Windows.Internals;
using RiceTea.Core.Windows.ObjectModels;

namespace RiceTea.Core.Windows.Helpers;

public static class StreamHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IWin32Stream ToWin32Stream(Stream stream) => new CLRStreamToWin32Adapter(stream);
}
