﻿using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class NativeObjectExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static T Clone<T>(this T _this) where T : NativeObject
            => CloneOrNull(_this) ?? _this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? CloneOrNull<T>(this T _this) where T : NativeObject
            => NativeObject.CopyReference(_this) as T;
    }
}
