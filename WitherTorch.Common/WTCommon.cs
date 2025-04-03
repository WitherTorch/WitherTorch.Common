using System.Runtime.CompilerServices;

using WitherTorch.Common.Internals;

namespace WitherTorch.Common
{
    public static class WTCommon
    {
        private static IArrayPoolProvider? _arrayPoolProvider = null; 

        public static IArrayPoolProvider ArrayPoolProvider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _arrayPoolProvider ?? FallbackArrayPoolProvider.Instance;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _arrayPoolProvider = value;
        }
    }
}
