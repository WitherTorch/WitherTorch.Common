#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Intrinsics;

#pragma warning disable CS8500

namespace WitherTorch.Common.Extensions
{
    partial class VectorExtensions
    {
        public static partial ulong ExtractMostSignificantBits<T>(this in Vector<T> _this) where T : struct
            => UnsafeHelper.SizeOf<Vector<T>>() switch
            {
                sizeof(ulong) => ExtractMostSignificantBits_64(in _this),
                M128.SizeInBytes => ExtractMostSignificantBits_128(in _this),
                M256.SizeInBytes => ExtractMostSignificantBits_256(in _this),
                M512.SizeInBytes => ExtractMostSignificantBits_512(in _this),
                _ => throw new PlatformNotSupportedException()
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_64<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore<T>(
                UnsafeHelper.As<Vector<T>, ulong>(ref UnsafeHelper.AsRefIn(in vector)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_128<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore_128<T>(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)), 
                sizeof(ulong) / sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_256<T>(in Vector<T> vector) where T : struct 
            => ExtractMostSignificantBitsCore_256<T>(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)),
                sizeof(ulong) / sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong ExtractMostSignificantBits_512<T>(in Vector<T> vector) where T : struct
            => ExtractMostSignificantBitsCore_512<T>(
                in UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in vector)),
                sizeof(ulong) / sizeof(T));

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_128<T>(in Vector<ulong> vector, int bitCountPer64) where T : struct
            => ExtractMostSignificantBitsCore<T>(vector[0]) |
            (ExtractMostSignificantBitsCore<T>(vector[1]) << bitCountPer64);

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_256<T>(in Vector<ulong> vector, int bitCountPer64) where T : struct
            => ExtractMostSignificantBitsCore<T>(vector[0]) |
            (ExtractMostSignificantBitsCore<T>(vector[1]) << bitCountPer64) |
            (ExtractMostSignificantBitsCore<T>(vector[2]) << (bitCountPer64 * 2)) |
            (ExtractMostSignificantBitsCore<T>(vector[3]) << (bitCountPer64 * 3));

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_512<T>(in Vector<ulong> vector, int bitCountPer64) where T : struct
            => ExtractMostSignificantBitsCore<T>(vector[0]) |
            (ExtractMostSignificantBitsCore<T>(vector[1]) << bitCountPer64) |
            (ExtractMostSignificantBitsCore<T>(vector[2]) << (bitCountPer64 * 2)) |
            (ExtractMostSignificantBitsCore<T>(vector[3]) << (bitCountPer64 * 3)) |
            (ExtractMostSignificantBitsCore<T>(vector[4]) << (bitCountPer64 * 4)) |
            (ExtractMostSignificantBitsCore<T>(vector[5]) << (bitCountPer64 * 5)) |
            (ExtractMostSignificantBitsCore<T>(vector[6]) << (bitCountPer64 * 6)) |
            (ExtractMostSignificantBitsCore<T>(vector[7]) << (bitCountPer64 * 7));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractMostSignificantBitsCore<T>(ulong source) where T : struct
            => UnsafeHelper.SizeOf<T>() switch
            {
                sizeof(byte) => ExtractMostSignificantBitsCore_8(source),
                sizeof(ushort) => ExtractMostSignificantBitsCore_16(source),
                sizeof(uint) => ExtractMostSignificantBitsCore_32(source),
                sizeof(ulong) => ExtractMostSignificantBitsCore_64(source),
                _ => throw new PlatformNotSupportedException()
            };

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_8(ulong source)
            => ((source & 0x80_80_80_80_80_80_80_80u) * 0x00_02_04_08_10_20_40_81u) >>> 56;

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_16(ulong source)
            => ((source & 0x8000_8000_8000_8000u) * 0x0000_2000_4000_8001u) >>> 60;

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_32(ulong source)
            => ((source & 0x80000000_80000000u) * 0x00000000_80000001u) >>> 62;

        [Inline(InlineBehavior.Remove)]
        private static ulong ExtractMostSignificantBitsCore_64(ulong source)
            => source >>> 63;
    }
}
#endif