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
        public static int GetCurrentThreadId() => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetCurrentThreadId(),
            UnixNativeMethodInstance inst => inst.GetCurrentThreadId(),
            FallbackNativeMethodInstance inst => inst.GetCurrentThreadId(),
            _ => _methodInstance.GetCurrentThreadId()
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCurrentProcessorId() => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetCurrentProcessorId(),
            UnixNativeMethodInstance inst => inst.GetCurrentProcessorId(),
            FallbackNativeMethodInstance inst => inst.GetCurrentProcessorId(),
            _ => _methodInstance.GetCurrentProcessorId()
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetTicksForSystem() => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetTicksForSystem(),
            UnixNativeMethodInstance inst => inst.GetTicksForSystem(),
            FallbackNativeMethodInstance inst => inst.GetTicksForSystem(),
            _ => _methodInstance.GetTicksForSystem()
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SleepInRelativeTicks(ulong ticks) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.SleepInRelativeTicks(ticks),
            UnixNativeMethodInstance inst => inst.SleepInRelativeTicks(ticks),
            FallbackNativeMethodInstance inst => inst.SleepInRelativeTicks(ticks),
            _ => _methodInstance.SleepInRelativeTicks(ticks)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SleepInAbsoluteTicks(ulong ticks) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.SleepInAbsoluteTicks(ticks),
            UnixNativeMethodInstance inst => inst.SleepInAbsoluteTicks(ticks),
            FallbackNativeMethodInstance inst => inst.SleepInAbsoluteTicks(ticks),
            _ => _methodInstance.SleepInAbsoluteTicks(ticks)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, int sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, uint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, sizeInBytes);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, long sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, ulong sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, nint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* destination, void* source, nuint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.CopyMemory(destination, source, sizeInBytes);
                    break;
                default:
                    _methodInstance.CopyMemory(destination, source, sizeInBytes);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, int sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, uint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, sizeInBytes);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, long sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, unchecked((nuint)MathHelper.MakeUnsigned(sizeInBytes)));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, ulong sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, unchecked((nuint)sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, nint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, MathHelper.MakeUnsigned(sizeInBytes));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveMemory(void* destination, void* source, nuint sizeInBytes)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.MoveMemory(destination, source, sizeInBytes);
                    break;
                default:
                    _methodInstance.MoveMemory(destination, source, sizeInBytes);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(int size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                _ => _methodInstance.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(uint size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                _ => _methodInstance.AllocMemoryPage(size, flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(long size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)MathHelper.MakeUnsigned(size)), flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)MathHelper.MakeUnsigned(size)), flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)MathHelper.MakeUnsigned(size)), flags),
                _ => _methodInstance.AllocMemoryPage(unchecked((nuint)MathHelper.MakeUnsigned(size)), flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(ulong size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)size), flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)size), flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(unchecked((nuint)size), flags),
                _ => _methodInstance.AllocMemoryPage(unchecked((nuint)size), flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(nint size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags),
                _ => _methodInstance.AllocMemoryPage(MathHelper.MakeUnsigned(size), flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags)
            => _methodInstance switch
            {
                Win32NativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                UnixNativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                FallbackNativeMethodInstance inst => inst.AllocMemoryPage(size, flags),
                _ => _methodInstance.AllocMemoryPage(size, flags)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, int size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, uint size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, size, flags);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, long size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)), flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)), flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)), flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)), flags);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, ulong size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)size), flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)size), flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, unchecked((nuint)size), flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, unchecked((nuint)size), flags);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, nint size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, MathHelper.MakeUnsigned(size), flags);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ProtectMemoryPage(ptr, size, flags);
                    break;
                default:
                    _methodInstance.ProtectMemoryPage(ptr, size, flags);
                    break;
            }
        }
    }
}
