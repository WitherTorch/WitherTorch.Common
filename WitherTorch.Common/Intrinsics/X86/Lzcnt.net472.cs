#if NET472_OR_GREATER
using System.Diagnostics;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    unsafe partial class Lzcnt
    {
        private static readonly bool _isSupported;

        static Lzcnt()
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
            const int LzcntMask = 1 << 5;
            return (X86Base.CpuId(unchecked((int)0x80000001), 0).Ecx & LzcntMask) == LzcntMask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static partial uint LeadingZeroCount(uint value)
        {
            InjectLzcntAsm();

            return 31u ^ (uint)MathHelper.Log2SoftwareFallback(value);
        }

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif