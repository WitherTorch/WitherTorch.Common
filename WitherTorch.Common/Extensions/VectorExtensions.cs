using System.Numerics;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Extensions
{
    public static partial class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ulong ExtractMostSignificantBits<T>(this in Vector<T> _this) where T : struct;
    }
}
