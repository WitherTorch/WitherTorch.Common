using System;
using System.Runtime.CompilerServices;

using RiceTea.Core.Helpers;

namespace RiceTea.Core.Extensions;

public static class PinnableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ReadOnlySpan<T> AsSpan<T>(this IPinnableReference<T> _this) where T : unmanaged
        => new ReadOnlySpan<T>(UnsafeHelper.AsPointerIn(in _this.GetPinnableReference()), unchecked((int)MathHelper.MakeSigned(_this.GetPinnedLength())));
}
