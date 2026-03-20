#if NET472_OR_GREATER
using System.Diagnostics;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Popcnt
    {
        private static readonly bool _isSupported;

        static Popcnt()
        {
            if (!CheckIsSupported())
                return;
            _isSupported = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIsSupported()
        {
            if (!X86Base.IsSupported)
                return false;
            const int PopcntMask = 1 << 23;
            return (X86Base.CpuId(unchecked((int)0x80000001), 0).Ecx & PopcntMask) == PopcntMask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static partial uint PopCount(uint value)
        {
            InjectPopcntAsm();

            return (uint)MathHelper.PopCountSoftwareFallback(value);
        }

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif