#if NET472_OR_GREATER
using WitherTorch.Common.Intrinsics.X86;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        private static readonly bool _isSystemMemoryExists = WTCommon.SystemBuffersExists;
        private static readonly bool _isLzcntSupported = Lzcnt.IsSupported;
        private static readonly bool _isLzcnt_X64Supported = Lzcnt.X64.IsSupported;
        private static readonly bool _isPopcntSupported = Popcnt.IsSupported;
        private static readonly bool _isPopcnt_X64Supported = Popcnt.X64.IsSupported;
        private static readonly bool _isBmi1Supported = Bmi1.IsSupported;
        private static readonly bool _isBmi1_X64Supported = Bmi1.X64.IsSupported;
        private static readonly bool _isX86BaseSupported = X86Base.IsSupported;
        private static readonly bool _isX86Base_X64Supported = X86Base.X64.IsSupported;
    }
}
#endif