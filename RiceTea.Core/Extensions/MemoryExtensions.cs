using System;
using System.Runtime.CompilerServices;

using RiceTea.Core.Text;

namespace RiceTea.Core.Extensions;

public static class MemoryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [OverloadResolutionPriority(int.MaxValue)]
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
