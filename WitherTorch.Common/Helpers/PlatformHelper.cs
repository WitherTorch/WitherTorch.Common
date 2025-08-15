namespace WitherTorch.Common.Helpers
{
    public static class PlatformHelper
    {
        private static readonly bool _isX86, _isX64;

        static PlatformHelper()
        {
#if X86_ARCH
            _isX86 = true;
            _isX64 = UnsafeHelper.PointerSize == sizeof(ulong);
#else
            var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
            _isX86 = arch switch
            {
                System.Runtime.InteropServices.Architecture.X86 or System.Runtime.InteropServices.Architecture.X64 => true,
                _ => false,
            };
            _isX64 = arch == System.Runtime.InteropServices.Architecture.X64;
#endif
        }

        public static bool IsX86 => _isX86;

        public static bool IsX64 => _isX64;
    }
}
