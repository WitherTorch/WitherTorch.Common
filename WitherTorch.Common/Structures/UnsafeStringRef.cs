using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Structures;

[StructLayout(LayoutKind.Auto)]
public readonly ref struct UnsafeStringRef
{
    private readonly string _str;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnsafeStringRef(string str) => _str = str;

    public ref readonly char FirstElement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref UnsafeHelper.GetStringDataReference(_str);
    }

    public ref readonly char LastElement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref UnsafeHelper.AddTypedOffsetAsReadOnly(in UnsafeHelper.GetStringDataReference(_str), _str.Length - 1);
    }

    public ref readonly char this[nint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref UnsafeHelper.AddTypedOffsetAsReadOnly(in UnsafeHelper.GetStringDataReference(_str), index);
    }

    public ref readonly char this[nuint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref UnsafeHelper.AddTypedOffsetAsReadOnly(in UnsafeHelper.GetStringDataReference(_str), index);
    }
}
