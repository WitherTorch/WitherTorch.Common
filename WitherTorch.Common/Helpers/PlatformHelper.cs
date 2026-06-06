using System.Runtime.InteropServices;

namespace WitherTorch.Common.Helpers
{
    public static class PlatformHelper
    {
        public static readonly bool IsX86, IsX64, IsMono, IsUnix, IsWindows, IsLinux, IsMacOSX, IsFreeBSD;

        static PlatformHelper()
        {
#if X86_ARCH
            IsX86 = true;
#if B64_ARCH
            IsX64 = true;
#elif B32_ARCH
            IsX64 = false;
#else
            IsX64 = UIntPtr.Size == sizeof(ulong);
#endif
#else
            var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
            IsX86 = arch switch
            {
                System.Runtime.InteropServices.Architecture.X86 or System.Runtime.InteropServices.Architecture.X64 => true,
                _ => false,
            };
            IsX64 = arch == System.Runtime.InteropServices.Architecture.X64;
#endif

            OSPlatform freeBsd;
#if NETCOREAPP3_0_OR_GREATER
            IsMono = false;
            freeBsd = OSPlatform.FreeBSD;
#else
            IsMono = System.Type.GetType("Mono.Runtime") is not null;
            freeBsd = OSPlatform.Create("FREEBSD");
#endif
            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                IsUnix = true;
                IsLinux = true;
                IsMacOSX = false;
                IsFreeBSD = false;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                IsUnix = true;
                IsLinux = false;
                IsMacOSX = true;
                IsFreeBSD = false;
            }
            else if (RuntimeInformation.IsOSPlatform(freeBsd))
            {
                IsUnix = true;
                IsLinux = false;
                IsMacOSX = false;
                IsFreeBSD = true;
            }
            else
            {
                IsUnix = false;
                IsLinux = false;
                IsMacOSX = false;
                IsFreeBSD = false;
            }
        }
    }
}
