#if NET472_OR_GREATER
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

            switch (value)
            {
                case uint.MaxValue:
                    return 0;
                case 0:
                    return 32;
                default:
                    {
                        uint count = 0;
                        for (int i = 31; i >= 0; i--)
                        {
                            if (((value >> i) & 1) != 0)
                                break;
                            count++;
                        }
                        return count;
                    }
            }
        }

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif