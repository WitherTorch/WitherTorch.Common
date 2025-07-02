using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class SequenceHelper
    {
#pragma warning disable CS8500

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* WithOffset<T>(T* ptr, int offset) => ptr + offset;
    }
}
