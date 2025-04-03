using System.Runtime.CompilerServices;

using WitherTorch.CrossNative.Internals;

namespace WitherTorch.CrossNative
{
    public static class WTCrossNative
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
