#if NET472_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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
                64 => ExtractMostSignificantBits_64(in _this),
                128 => ExtractMostSignificantBits_128(in _this),
                256 => ExtractMostSignificantBits_256(in _this),
                512 => ExtractMostSignificantBits_512(in _this),
                _ => throw new PlatformNotSupportedException()
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_64<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore(
                UnsafeHelper.As<Vector<T>, ulong>(ref UnsafeHelper.AsRefIn(in vector)), 
                GetMaskOfType<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_128<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore_128(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)), 
                GetMaskOfType<T>(), 
                sizeof(T) / sizeof(ulong));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_256<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore_256(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)), 
                GetMaskOfType<T>(), 
                sizeof(T) / sizeof(ulong));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_512<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore_512(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)), 
                GetMaskOfType<T>(), 
                sizeof(T) / sizeof(ulong));

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_128(in Vector<ulong> vector, ulong mask, int bitCountPer64)
            => ExtractMostSignificantBitsCore(vector[0], mask) |
            (ExtractMostSignificantBitsCore(vector[1], mask) << bitCountPer64);

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_256(in Vector<ulong> vector, ulong mask, int bitCountPer64)
            => ExtractMostSignificantBitsCore(vector[0], mask) |
            (ExtractMostSignificantBitsCore(vector[1], mask) << bitCountPer64) |
            (ExtractMostSignificantBitsCore(vector[2], mask) << (bitCountPer64 * 2)) |
            (ExtractMostSignificantBitsCore(vector[3], mask) << (bitCountPer64 * 3));

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_512(in Vector<ulong> vector, ulong mask, int bitCountPer64)
            => ExtractMostSignificantBitsCore(vector[0], mask) |
            (ExtractMostSignificantBitsCore(vector[1], mask) << bitCountPer64) |
            (ExtractMostSignificantBitsCore(vector[2], mask) << (bitCountPer64 * 2)) |
            (ExtractMostSignificantBitsCore(vector[3], mask) << (bitCountPer64 * 3)) |
            (ExtractMostSignificantBitsCore(vector[4], mask) << (bitCountPer64 * 4)) |
            (ExtractMostSignificantBitsCore(vector[5], mask) << (bitCountPer64 * 5)) |
            (ExtractMostSignificantBitsCore(vector[6], mask) << (bitCountPer64 * 6)) |
            (ExtractMostSignificantBitsCore(vector[7], mask) << (bitCountPer64 * 7));

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore(ulong source, ulong mask)
            => ((source & mask) * 0x2040810204081) >>> 56;

        [Inline(InlineBehavior.Remove)]
        private static unsafe ulong GetMaskOfType<T>()
            => sizeof(T) switch
            {
                sizeof(byte) => 0x8080808080808080u,
                sizeof(ushort) => 0x8000800080008000u,
                sizeof(uint) => 0x8000000080000000u,
                sizeof(ulong) => 0x8000000000000000u,
                _ => throw new PlatformNotSupportedException()
            };
    }
}
#endif