using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static unsafe class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> AsSpan(this in NativeMemoryBlock _this)
            => new Span<byte>(_this.NativePointer, (int)MathHelper.MakeSigned(_this.Length));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this in TypedNativeMemoryBlock<T> _this) where T : unmanaged
            => new Span<T>(_this.NativePointer, (int)MathHelper.MakeSigned(_this.Length));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MaxValue)]
        public static StringWrapper ToStringWrapper(this ReadOnlySpan<char> _this)
            => StringWrapper.Create(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapper(this ReadOnlySpan<char> _this, StringCreateOptions options)
            => StringWrapper.Create(_this, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsUtf16(this ReadOnlySpan<char> _this)
            => StringWrapper.CreateUtf16String(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsAscii(this ReadOnlySpan<byte> _this)
            => StringWrapper.CreateAsciiString(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsLatin1(this ReadOnlySpan<byte> _this)
            => StringWrapper.CreateLatin1String(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ToStringWrapperAsUtf8(this ReadOnlySpan<byte> _this)
            => StringWrapper.CreateUtf8String(_this);
    }
}
