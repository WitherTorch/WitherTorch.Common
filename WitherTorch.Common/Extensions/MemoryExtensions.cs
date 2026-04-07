using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class MemoryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(999)]
        public static StringWrapper ToStringWrapper(this ReadOnlyMemory<char> _this)
            => StringWrapper.Create(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapper(this ReadOnlyMemory<char> _this, StringCreateOptions options)
            => StringWrapper.Create(_this, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsUtf16(this ReadOnlyMemory<char> _this)
            => StringWrapper.CreateUtf16String(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsAscii(this ReadOnlyMemory<byte> _this)
            => StringWrapper.CreateAsciiString(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsLatin1(this ReadOnlyMemory<byte> _this)
            => StringWrapper.CreateLatin1String(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsUtf8(this ReadOnlyMemory<byte> _this)
            => StringWrapper.CreateUtf8String(_this);
    }
}
