#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Helpers;

#pragma warning disable CS8500

namespace WitherTorch.Common.Extensions
{
    partial class VectorExtensions
    {
        public static partial ulong ExtractMostSignificantBits<T>(this in Vector<T> _this) where T : struct
            => UnsafeHelper.SizeOf<Vector<T>>() switch
            {
                64 => UnsafeHelper.As<Vector<T>, Vector64<T>>(ref UnsafeHelper.AsRefIn(in _this)).ExtractMostSignificantBits(),
                128 => _this.AsVector128().ExtractMostSignificantBits(),
                256 => _this.AsVector256().ExtractMostSignificantBits(),
                512 => _this.AsVector512().ExtractMostSignificantBits(),
                _ => throw new PlatformNotSupportedException()
            };
    }
}
#endif