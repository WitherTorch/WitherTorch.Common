using System.Runtime.CompilerServices;

using WitherTorch.Common.Internals;

namespace WitherTorch.Common
{
    public static class WTCrossNative
    {
        private static IArrayPoolProvider _arrayPoolProvider = FallbackArrayPoolProvider.Instance; 

        public static IArrayPoolProvider ArrayPoolProvider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _arrayPoolProvider;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _arrayPoolProvider = value;
        }
    }
}
