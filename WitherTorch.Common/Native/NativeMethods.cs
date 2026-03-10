using System;
using System.Runtime.CompilerServices;
using System.Threading;

using InlineMethod;

using WitherTorch.Common.Structures;

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

        [Inline(InlineBehavior.Keep, export: true)]
        public static IntPtr CreateWaitingHandle(bool autoReset) => CreateWaitingHandle(initialState: false, autoReset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateWaitingHandle(bool initialState, bool autoReset) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.CreateWaitingHandle(initialState, autoReset),
            UnixNativeMethodInstance inst => inst.CreateWaitingHandle(initialState, autoReset),
            FallbackNativeMethodInstance inst => inst.CreateWaitingHandle(initialState, autoReset),
            _ => _methodInstance.CreateWaitingHandle(initialState, autoReset)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetWaitingHandle(IntPtr handle)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.ResetWaitingHandle(handle);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.ResetWaitingHandle(handle);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.ResetWaitingHandle(handle);
                    break;
                default:
                    _methodInstance.ResetWaitingHandle(handle);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetWaitingHandle(IntPtr handle)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.SetWaitingHandle(handle);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.SetWaitingHandle(handle);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.SetWaitingHandle(handle);
                    break;
                default:
                    _methodInstance.SetWaitingHandle(handle);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyWaitingHandle(IntPtr handle)
        {
            switch (_methodInstance)
            {
                case Win32NativeMethodInstance inst:
                    inst.DestroyWaitingHandle(handle);
                    break;
                case UnixNativeMethodInstance inst:
                    inst.DestroyWaitingHandle(handle);
                    break;
                case FallbackNativeMethodInstance inst:
                    inst.DestroyWaitingHandle(handle);
                    break;
                default:
                    _methodInstance.DestroyWaitingHandle(handle);
                    break;
            }
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool WaitForWaitingHandle(IntPtr handle) => WaitForWaitingHandle(handle, unchecked((uint)Timeout.Infinite));

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool WaitForWaitingHandle(IntPtr handle, TimeSpan timeout)
            => WaitForWaitingHandle(handle, (uint)(ulong)timeout.Ticks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WaitForWaitingHandle(IntPtr handle, uint timeout) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.WaitForWaitingHandle(handle, timeout),
            UnixNativeMethodInstance inst => inst.WaitForWaitingHandle(handle, timeout),
            FallbackNativeMethodInstance inst => inst.WaitForWaitingHandle(handle, timeout),
            _ => _methodInstance.WaitForWaitingHandle(handle, timeout)
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
        public static void* GetImportedMethodPointer(string dllName, int methodIndex) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodIndex),
            UnixNativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodIndex),
            FallbackNativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodIndex),
            _ => _methodInstance.GetImportedMethodPointer(dllName, methodIndex)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* GetImportedMethodPointer(string dllName, string methodName) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodName),
            UnixNativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodName),
            FallbackNativeMethodInstance inst => inst.GetImportedMethodPointer(dllName, methodName),
            _ => _methodInstance.GetImportedMethodPointer(dllName, methodName)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, int methodIndex)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<int>(methodIndex));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, int methodIndex1, int methodIndex2)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<int>(methodIndex1, methodIndex2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, int methodIndex1, int methodIndex2, int methodIndex3)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<int>(methodIndex1, methodIndex2, methodIndex3));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, params int[] methodIndices)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<int>(methodIndices));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName1, string methodName2)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName1, methodName2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName1, string methodName2, string methodName3)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName1, methodName2, methodName3));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void*[] GetImportedMethodPointers(string dllName, params string[] methodNames)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodNames));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void*[] GetImportedMethodPointers(string dllName, in ParamArrayTiny<int> methodIndices) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodIndices),
            UnixNativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodIndices),
            FallbackNativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodIndices),
            _ => _methodInstance.GetImportedMethodPointers(dllName, methodIndices)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void*[] GetImportedMethodPointers(string dllName, in ParamArrayTiny<string> methodNames) => _methodInstance switch
        {
            Win32NativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodNames),
            UnixNativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodNames),
            FallbackNativeMethodInstance inst => inst.GetImportedMethodPointers(dllName, methodNames),
            _ => _methodInstance.GetImportedMethodPointers(dllName, methodNames)
        };
    }
}
