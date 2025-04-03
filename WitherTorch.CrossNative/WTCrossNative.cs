using System.Runtime.CompilerServices;

using WitherTorch.CrossNative.Internals;

namespace WitherTorch.CrossNative
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
