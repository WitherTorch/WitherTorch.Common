using System.Runtime.CompilerServices;

namespace WitherTorch.Common
{
    public static class Limits
    {
        /// <summary>
        /// 是否可以在 <see cref="Text.StringBuilderTiny"/> 上使用 <see cref="Text.StringBuilderTiny.SetStartPointer(char*, int)"/> 來配置堆疊位元組區塊 (參考用，無實質限制)
        /// </summary>
        public const bool UseStackallocStringBuilder = true;
#if NET6_0_OR_GREATER
        /// <summary>
        /// 是否啟用基於 <see cref="System.Runtime.Intrinsics.Vector512{T}"/> 的 512-bit 向量化加速
        /// </summary>
        public const bool UseVector512Acceleration = true;
        /// <summary>
        /// 是否啟用基於 <see cref="System.Runtime.Intrinsics.Vector256{T}"/> 的 256-bit 向量化加速
        /// </summary>
        public const bool UseVector256Acceleration = true;
        /// <summary>
        /// 是否啟用基於 <see cref="System.Runtime.Intrinsics.Vector128{T}"/> 的 128-bit 向量化加速
        /// </summary>
        public const bool UseVector128Acceleration = true;
        /// <summary>
        /// 是否啟用基於 <see cref="System.Runtime.Intrinsics.Vector64{T}"/> 的 64-bit 向量化加速
        /// </summary>
        public const bool UseVector64Acceleration = false;
#else
        /// <summary>
        /// 是否啟用基於 <see cref="System.Numerics.Vector{T}"/> 的向量化加速
        /// </summary>
        public const bool UseVectorAcceleration = true;
#endif
        /// <summary>
        /// 在堆疊上配置位元組區塊的上限值
        /// </summary>
        public const int MaxStackallocBytes = 4096;
        /// <summary>
        /// 在堆疊上配置字元區塊的上限值
        /// </summary>
        public const int MaxStackallocChars = MaxStackallocBytes / sizeof(char);
        /// <summary>
        /// 陣列的最大容許大小
        /// </summary>
        public static readonly int MaxArrayLength
#if NET8_0_OR_GREATER
            = System.Array.MaxLength;
#else
            = 0x7FEFFFFF;
#endif

#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseAnyVector() => UseVector512() || UseVector256() || UseVector128() || UseVector64();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseVector512() => UseVector512Acceleration && System.Runtime.Intrinsics.Vector512.IsHardwareAccelerated;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseVector256() => UseVector256Acceleration && System.Runtime.Intrinsics.Vector256.IsHardwareAccelerated;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseVector128() => UseVector128Acceleration && System.Runtime.Intrinsics.Vector128.IsHardwareAccelerated;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseVector64() => UseVector64Acceleration && System.Runtime.Intrinsics.Vector64.IsHardwareAccelerated;
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseAnyVector() => UseVector();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UseVector() => UseVectorAcceleration && System.Numerics.Vector.IsHardwareAccelerated;
#endif
    }
}
