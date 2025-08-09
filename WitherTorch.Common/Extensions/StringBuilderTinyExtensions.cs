using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class StringBuilderTinyExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetStartPointer(this StringBuilderTiny _this, in Span<char> span)
        {
            char* ptr = UnsafeHelper.AsPointerRef(ref span.GetPinnableReference());
            _this.SetStartPointer(ptr, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetStartPointer(this StringBuilderTiny _this, in ReadOnlySpan<char> span)
        {
            char* ptr = UnsafeHelper.AsPointerIn(in span.GetPinnableReference());
            _this.SetStartPointer(ptr, span.Length);
        }
    }
}
