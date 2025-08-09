using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class MemoryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase ToStringBase(this in ReadOnlySpan<char> _this)
            => ToStringBase(_this, WTCommon.StringCreateOptions);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase ToStringBase(this in ReadOnlySpan<char> _this, StringCreateOptions options)
        {
            int length = _this.Length;
            if (length <= 0)
                return StringBase.Empty;
            fixed (char* ptr = _this)
                return StringBase.Create(ptr, 0u, unchecked((nuint)length), options);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase ToStringBase(this in Span<char> _this)
            => ToStringBase(_this, WTCommon.StringCreateOptions);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase ToStringBase(this in Span<char> _this, StringCreateOptions options)
        {
            int length = _this.Length;
            if (length <= 0)
                return StringBase.Empty;
            fixed (char* ptr = _this)
                return StringBase.Create(ptr, 0u, unchecked((nuint)length), options);
        }
    }
}
