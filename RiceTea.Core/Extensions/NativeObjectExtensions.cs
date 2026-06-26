using System.Runtime.CompilerServices;

using InlineMethod;

using RiceTea.Core.Native;

namespace RiceTea.Core.Extensions;

public static class NativeObjectExtensions
{
    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Clone<T>(this T _this) where T : NativeObject
        => CloneOrNull(_this) ?? _this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? CloneOrNull<T>(this T _this) where T : NativeObject
        => NativeObject.Clone(_this) as T;
}
