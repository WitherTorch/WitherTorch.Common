#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        private static readonly bool _isSupported;

        static Bmi1()
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
            const int Bmi1Mask = 1 << 3;
            return (X86Base.CpuId(7, 0).Ebx & Bmi1Mask) == Bmi1Mask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static partial uint TrailingZeroCount(uint value)
        {
            InjectTzcntAsm();

            return (uint)MathHelper.TrailingZeroCountSoftwareFallback(value);
        }

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif