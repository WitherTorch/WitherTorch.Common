using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    unsafe partial class NativeMethods
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, int size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, uint size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, size);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, long size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)));
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, unchecked((nuint)MathHelper.MakeUnsigned(size)));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, ulong size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)size));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)size));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, unchecked((nuint)size));
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, unchecked((nuint)size));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, nint size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, MathHelper.MakeUnsigned(size));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlushInstructionCache(void* ptr, nuint size)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FlushInstructionCache(ptr, size);
                    break;
                default:
                    _methodInstance.FlushInstructionCache(ptr, size);
                    break;
            }
        }
    }
}
