using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static partial class EnumeratorHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> CreateEmptyEnumerator<T>() => new EmptyEnumerator<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> CreateOneItemEnumerator<T>(T item) => new OneItemEnumerator<T>(item);
    }
}
