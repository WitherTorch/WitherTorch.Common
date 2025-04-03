using InlineMethod;

using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    public static unsafe partial class NativeMethods
    {
        private static readonly INativeMethodInstance _methodInstance = GetOSDependedInstance();

        [Inline(InlineBehavior.Remove)]
        private static INativeMethodInstance GetOSDependedInstance()
        {
#if NET40_OR_GREATER
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return new UnixNativeMethodInstance();
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                    return new Win32NativeMethodInstance();
                default:
                    break;
            }
#elif NET5_0_OR_GREATER
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                return new UnixNativeMethodInstance();
            if (OperatingSystem.IsWindows())
                return new Win32NativeMethodInstance();
#endif
            return new FallbackNativeMethodInstance();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecurityCritical]
        public static int GetCurrentThreadId()
        {
            return _methodInstance.GetCurrentThreadId();
        }
    }
}
