using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        public static unsafe char* AllocCStyleUtf16String(string value)
        {
            int length = value.Length;
            uint byteCount = unchecked((uint)length) * sizeof(char);
            char* result = (char*)AllocMemory(byteCount + sizeof(char));
            result[length] = '\0';
            fixed (char* ptr = value)
                UnsafeHelper.CopyBlock(result, ptr, byteCount);
            return result;
        }

        public static unsafe T* AllocUnmanagedStructure<T>(T value) where T : unmanaged
        {
            T* result = (T*)AllocMemory(unchecked((nuint)sizeof(T)));
            *result = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(int size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            UnixNativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            FallbackNativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            _ => _methodInstance.AllocMemory(MathHelper.MakeUnsigned(size))
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(uint size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(size),
            UnixNativeMethodInstance inst => inst.AllocMemory(size),
            FallbackNativeMethodInstance inst => inst.AllocMemory(size),
            _ => _methodInstance.AllocMemory(size)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(long size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)MathHelper.MakeUnsigned(size))),
            UnixNativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)MathHelper.MakeUnsigned(size))),
            FallbackNativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)MathHelper.MakeUnsigned(size))),
            _ => _methodInstance.AllocMemory(unchecked((nuint)MathHelper.MakeUnsigned(size)))
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(ulong size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)size)),
            UnixNativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)size)),
            FallbackNativeMethodInstance inst => inst.AllocMemory(unchecked((nuint)size)),
            _ => _methodInstance.AllocMemory(unchecked((nuint)size))
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(nint size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            UnixNativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            FallbackNativeMethodInstance inst => inst.AllocMemory(MathHelper.MakeUnsigned(size)),
            _ => _methodInstance.AllocMemory(MathHelper.MakeUnsigned(size))
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AllocMemory(nuint size) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.AllocMemory(size),
            UnixNativeMethodInstance inst => inst.AllocMemory(size),
            FallbackNativeMethodInstance inst => inst.AllocMemory(size),
            _ => _methodInstance.AllocMemory(size)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemory(IntPtr ptr)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FreeMemory(ptr.ToPointer());
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FreeMemory(ptr.ToPointer());
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FreeMemory(ptr.ToPointer());
                    break;
                default:
                    _methodInstance.FreeMemory(ptr.ToPointer());
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeMemory(void* ptr)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.FreeMemory(ptr);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.FreeMemory(ptr);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.FreeMemory(ptr);
                    break;
                default:
                    _methodInstance.FreeMemory(ptr);
                    break;
            }
        }
    }
}
