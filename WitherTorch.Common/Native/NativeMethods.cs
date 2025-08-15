using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

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
        public static int GetCurrentThreadId() => _methodInstance.GetCurrentThreadId();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, int sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, uint sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, sizeInBytes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, long sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, ulong sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, unchecked((nuint)sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, nint sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, nuint sizeInBytes) 
            => _methodInstance.CopyMemory(destination, source, sizeInBytes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, int sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, uint sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, sizeInBytes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, long sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, ulong sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, unchecked((nuint)sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, nint sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, nuint sizeInBytes) 
            => _methodInstance.MoveMemory(destination, source, sizeInBytes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, int length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, MathHelper.MakeUnsigned(length), flags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, uint length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, length, flags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, long length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, unchecked((nuint)MathHelper.MakeUnsigned(length)), flags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, ulong length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, unchecked((nuint)length), flags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, nint length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, MathHelper.MakeUnsigned(length), flags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemory(void* ptr, nuint length, ProtectMemoryFlags flags) 
            => _methodInstance.ProtectMemory(ptr, length, flags);
    }
}
