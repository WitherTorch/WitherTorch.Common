using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class Win32NativeMethodInstance : INativeMethodInstance
        {
            private static readonly void* _waitOnAddressFunc, _wakeByAddressAllFunc;
            private readonly IntPtr _heap;

            static Win32NativeMethodInstance()
            {
                _waitOnAddressFunc = GetImportedMethodPointerCore("kernelbase.dll", nameof(WaitOnAddress));
                _wakeByAddressAllFunc = GetImportedMethodPointerCore("kernelbase.dll", nameof(WakeByAddressAll));
            }

            [SuppressGCTransition]
            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, EntryPoint = nameof(GetCurrentThreadId))]
            private static extern int GetCurrentThreadIdCore();

            [SuppressGCTransition]
            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern IntPtr GetProcessHeap();

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void* HeapAlloc(IntPtr hHeap, int dwFlags, nuint size);

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void HeapFree(IntPtr hHeap, int dwFlags, void* ptr);

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern void* VirtualAlloc(void* address, nuint dwSize, MemoryAllocationTypes allocationTypes, PageAccessRights rights);

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern bool VirtualProtect(void* address, nuint dwSize, PageAccessRights rights, PageAccessRights* oldRights);

            [SuppressGCTransition]
            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern int GetCurrentProcessorNumber();

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlMoveMemory(void* dest, void* src, nuint sizeInBytes);

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlCopyMemory(void* dest, void* src, nuint sizeInBytes);

            [SuppressGCTransition]
            [DllImport("kernel32")]
            private static extern void QueryUnbiasedInterruptTime(ulong* pUnbiasedTime);

            [DllImport("ntdll")]
            private static extern uint NtDelayExecution(int alertable, long* delayInterval);

            [SuppressGCTransition]
            [DllImport("kernel32")]
            private static extern void* GetProcAddress(IntPtr hModule, byte* lpProcName);

            [DllImport("kernel32")]
            private static extern IntPtr LoadLibraryW(char* lpLibFileName);

            [DllImport("kernel32")]
            private static extern IntPtr GetModuleHandleW(char* lpModuleName);

            [DllImport("kernel32")]
            public static extern IntPtr CreateEventW(void* lpEventAttributes, uint bManualReset, uint bInitialState, char* lpName);

            [DllImport("kernel32")]
            public static extern uint SetEvent(IntPtr hEvent);

            [DllImport("kernel32")]
            public static extern uint ResetEvent(IntPtr hEvent);

            [DllImport("kernel32")]
            public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

            [DllImport("kernel32")]
            public static extern uint CloseHandle(IntPtr hObject);

            [SuppressGCTransition]
            [DllImport("kernel32")]
            public static extern uint GetLastError();

            private static bool WaitOnAddress(void* address, void* compareAddress, nuint addressSize, uint dwMilliseconds)
            {
                void* func = _waitOnAddressFunc;
                DebugHelper.ThrowIf(func is null);
                return ((delegate* unmanaged[Stdcall]<void*, void*, nuint, uint, uint>)func)(address, compareAddress, addressSize, dwMilliseconds) != 0;
            }

            private static void WakeByAddressAll(void* address)
            {
                void* func = _wakeByAddressAllFunc;
                DebugHelper.ThrowIf(func is null);
                ((delegate* unmanaged[Stdcall]<void*, void>)func)(address);
            }

            public Win32NativeMethodInstance()
            {
                _heap = GetProcessHeap();
            }

            public int GetCurrentThreadId() => GetCurrentThreadIdCore();

            public int GetCurrentProcessorId() => GetCurrentProcessorNumber();

            public ulong GetTicksForSystem()
            {
                ulong result;
                QueryUnbiasedInterruptTime(&result);
                return result;
            }

            public bool SleepInRelativeTicks(ulong ticks)
            {
                if (ticks <= 0)
                    return false;
                SleepInRelativeTicksCore(ticks);
                return true;
            }

            public bool SleepInAbsoluteTicks(ulong ticks)
            {
                ulong currentTicks;
                QueryUnbiasedInterruptTime(&currentTicks);
                if (ticks <= currentTicks)
                    return false;
                SleepInRelativeTicksCore(ticks - currentTicks);
                return true;
            }

            public void* GetImportedMethodPointer(string? dllName, int methodIndex) => GetImportedMethodPointerCore(dllName, methodIndex);

            public void* GetImportedMethodPointer(string? dllName, string methodName) => GetImportedMethodPointerCore(dllName, methodName);

            public void*[] GetImportedMethodPointers(string? dllName, in ParamArrayTiny<int> methodIndices) => GetImportedMethodPointersCore(dllName, methodIndices);

            public void*[] GetImportedMethodPointers(string? dllName, in ParamArrayTiny<string> methodNames) => GetImportedMethodPointersCore(dllName, methodNames);

            public IntPtr CreateWaitingHandle(bool initialState, bool autoReset)
            {
                if (_waitOnAddressFunc is null)
                    return CreateEventW(null, bManualReset: MathHelper.BooleanToUInt32(!autoReset), MathHelper.BooleanToUInt32(initialState), null);
                else
                {
                    RawWaitingEvent* ptr = (RawWaitingEvent*)AllocMemory(UnsafeHelper.SizeOf<RawWaitingEvent>());
                    *ptr = new RawWaitingEvent(!autoReset);
                    return RawWaitingEvent.GetWaitingHandleFromEvent(ptr);
                }
            }

            public void ResetWaitingHandle(IntPtr handle)
            {
                if (_waitOnAddressFunc is null)
                    ResetEvent(handle);
                else
                    RawWaitingEvent.GetEventFromWaitingHandle(handle)->State = false;
            }

            public void SetWaitingHandle(IntPtr handle)
            {
                if (_waitOnAddressFunc is null)
                    ResetEvent(handle);
                else
                {
                    RawWaitingEvent.GetEventFromWaitingHandle(handle)->State = true;
                    WakeByAddressAll((void*)handle);
                }
            }

            public void DestroyWaitingHandle(IntPtr handle)
            {
                if (_waitOnAddressFunc is null)
                    CloseHandle(handle);
                else
                {
                    RawWaitingEvent* ptr = RawWaitingEvent.GetEventFromWaitingHandle(handle);
                    FreeMemory(ptr);
                }
            }

            public bool WaitForWaitingHandle(IntPtr handle, uint timeout)
            {
                if (_waitOnAddressFunc is null)
                    return LegacyWait(handle, timeout);
                else
                    return ModernWait(handle, timeout);
            }

            public void* AllocMemory(nuint size) => HeapAlloc(_heap, 0, size);

            public void FreeMemory(void* ptr) => HeapFree(_heap, 0, ptr);

            public void CopyMemory(void* destination, void* source, nuint sizeInBytes) => RtlCopyMemory(destination, source, sizeInBytes);

            public void MoveMemory(void* destination, void* source, nuint sizeInBytes) => RtlMoveMemory(destination, source, sizeInBytes);

            public void* AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags)
                => VirtualAlloc(null, size, MemoryAllocationTypes.Commit | MemoryAllocationTypes.Reserve, ConvertPageAccessRightsFromFlags(flags));

            public void ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
            {
                PageAccessRights rights = ConvertPageAccessRightsFromFlags(flags);
                VirtualProtect(ptr, size, rights, &rights);
            }

            private static void* GetImportedMethodPointerCore(string? dllName, int methodIndex)
            {
                IntPtr module = dllName is null ? GetModuleHandleW(null) : LoadLibrary(dllName);
                return GetProcAddress(module, (byte*)methodIndex);
            }

            private static void* GetImportedMethodPointerCore(string dllName, string methodName)
            {
                IntPtr module = dllName is null ? GetModuleHandleW(null) : LoadLibrary(dllName);

                ArrayPool<byte> pool = ArrayPool<byte>.Shared;

                return GetImportedMethodPointerCore(pool, module, methodName);
            }

            private static void*[] GetImportedMethodPointersCore(string dllName, ParamArrayTiny<int> methodIndices)
            {
                IntPtr module = dllName is null ? GetModuleHandleW(null) : LoadLibrary(dllName);

                int length = methodIndices.Length;
                void*[] pointers = new void*[length];

                for (int i = 0; i < length; i++)
                {
                    int methodIndex = methodIndices[i];
                    pointers[i] = GetProcAddress(module, (byte*)methodIndex);
                }

                return pointers;
            }

            private static void*[] GetImportedMethodPointersCore(string dllName, ParamArrayTiny<string> methodNames)
            {
                IntPtr module = dllName is null ? GetModuleHandleW(null) : LoadLibrary(dllName);

                ArrayPool<byte> pool = ArrayPool<byte>.Shared;

                int length = methodNames.Length;
                void*[] pointers = new void*[length];

                for (int i = 0; i < length; i++)
                {
                    string methodName = methodNames[i];
                    pointers[i] = GetImportedMethodPointerCore(pool, module, methodName);
                }

                return pointers;
            }

            private static void* GetImportedMethodPointerCore(ArrayPool<byte> pool, IntPtr module, string methodName)
            {
                int length = methodName.Length;
                byte[] buffer = pool.Rent(length + 1);
                try
                {
                    fixed (char* source = methodName)
                    fixed (byte* destination = buffer)
                    {
                        AsciiEncodingHelper.ReadFromUtf16Buffer(source, source + length, destination, destination + length);
                        destination[length] = 0;
                        return GetProcAddress(module, destination);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static IntPtr LoadLibrary(string lpLibFileName)
            {
                fixed (char* ptr = lpLibFileName)
                    return LoadLibraryW(ptr);
            }

            private static bool LegacyWait(IntPtr waitingHandle, uint timeout)
            {
                const uint INFINITE = unchecked((uint)Timeout.Infinite);
                const uint WAIT_TIMEOUT = 0x00000102U;

                if (timeout == INFINITE)
                {
                    WaitForSingleObject(waitingHandle, dwMilliseconds: timeout);
                    return true;
                }

                return WaitForSingleObject(waitingHandle, dwMilliseconds: timeout) != WAIT_TIMEOUT;
            }

            private static bool ModernWait(IntPtr waitingHandle, uint timeout)
            {
                RawWaitingEvent* ptr = RawWaitingEvent.GetEventFromWaitingHandle(waitingHandle);
                if (ptr->IsManuallyReset)
                    return ModernWaitCore(waitingHandle, timeout);
                try
                {
                    return ModernWaitCore(waitingHandle, timeout);
                }
                finally
                {
                    ptr->State = false;
                }
            }

            private static bool ModernWaitCore(IntPtr waitingHandle, uint timeout)
            {
                const uint INFINITE = unchecked((uint)Timeout.Infinite);
                const int ERROR_TIMEOUT = 0x5B4;

                nuint referenceValue = 0;

                bool result = WaitOnAddress((void*)waitingHandle, &referenceValue, UnsafeHelper.SizeOf<nuint>(), timeout);

                if (timeout == INFINITE)
                    return result;

                if (result)
                    return true;

                uint lastError = GetLastError();
                if (lastError != ERROR_TIMEOUT)
                    throw new Win32Exception((int)lastError);
                return false;
            }

            private static PageAccessRights ConvertPageAccessRightsFromFlags(ProtectMemoryPageFlags flags)
            {
                if ((flags & ProtectMemoryPageFlags.CanExecute) == ProtectMemoryPageFlags.CanExecute)
                {
                    if ((flags & ProtectMemoryPageFlags.CanRead) == ProtectMemoryPageFlags.CanRead)
                    {
                        if ((flags & ProtectMemoryPageFlags.CanWrite) == ProtectMemoryPageFlags.CanWrite)
                            return PageAccessRights.ExecuteReadWrite;
                        return PageAccessRights.ExecuteRead;
                    }
                    return PageAccessRights.Execute;
                }
                else
                {
                    if ((flags & ProtectMemoryPageFlags.CanRead) == ProtectMemoryPageFlags.CanRead)
                    {
                        if ((flags & ProtectMemoryPageFlags.CanWrite) == ProtectMemoryPageFlags.CanWrite)
                            return PageAccessRights.ReadWrite;
                        return PageAccessRights.ReadOnly;
                    }
                    return PageAccessRights.NoAccess;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void SleepInRelativeTicksCore(ulong ticks)
            {
                if (ticks > long.MaxValue)
                {
                    long time = -long.MaxValue;
                    NtDelayExecution(alertable: Booleans.FalseInt, &time);
                    time = -(long)(ticks - long.MaxValue);
                    NtDelayExecution(alertable: Booleans.FalseInt, &time);
                }
                else
                {
                    ticks = UnsafeHelper.Negate(ticks);
                    NtDelayExecution(alertable: Booleans.FalseInt, (long*)&ticks);
                }
            }

            [Flags]
            private enum MemoryAllocationTypes : uint
            {
                None = 0,
                Commit = 0x00001000,
                Reserve = 0x00002000,
                ReplacePlaceholder = 0x00004000,
                ReservePlaceholder = 0x00040000,
                Reset = 0x00080000,
                TopDown = 0x00100000,
                WriteWatch = 0x00200000,
                Physical = 0x00400000,
                Rotate = 0x00800000,
                DifferenceImageBaseOk = 0x00800000,
                ResetUndo = 0x01000000,
                LargePages = 0x20000000,
                Alloc4MbPages = 0x80000000,
                Alloc64KPages = (LargePages | Physical),
                UnmapWithTransientBoost = 0x00000001,
                Coalesce_Placeholders = 0x00000001,
                PreservePlaceholder = 0x00000002,
                Decommit = 0x00004000,
                Release = 0x00008000,
                Free = 0x00010000
            }

            [Flags]
            private enum PageAccessRights : uint
            {
                None = 0x00,
                NoAccess = 0x01,
                ReadOnly = 0x02,
                ReadWrite = 0x04,
                WriteCopy = 0x08,
                Execute = 0x10,
                ExecuteRead = 0x20,
                ExecuteReadWrite = 0x40,
                ExecuteWriteCopy = 0x80,
                Guard = 0x100,
                NoCache = 0x200,
                WriteCombine = 0x400,
                GraphicsNoAccess = 0x0800,
                GraphicsReadOnly = 0x1000,
                GraphicsReadWrite = 0x2000,
                GraphicsExecute = 0x4000,
                GraphicsExecuteRead = 0x8000,
                GraphicsExecuteReadWrite = 0x10000,
                GraphicsConherent = 0x20000,
                GraphicsNoCache = 0x40000,
                EnclaveThreadControl = 0x80000000,
                RevertToFileMap = 0x80000000,
                TargetsNoUpdate = 0x40000000,
                TargetsInvalid = 0x40000000,
                EnclaveUnvalidated = 0x20000000,
                EnclaveMask = 0x10000000,
                EnclaveDecommit = (EnclaveMask | 0),
                EnclaveSSFirst = (EnclaveMask | 1),
                EnclaveSSRest = (EnclaveMask | 2),
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            private struct RawWaitingEvent
            {
                private readonly nuint _manualReset;
                private nuint _state;

                public readonly bool IsManuallyReset => _manualReset != 0;
                public bool State
                {
                    readonly get => _state != 0;
                    set => _state = value ? 1u : 0u;
                }

                public static IntPtr GetWaitingHandleFromEvent(RawWaitingEvent* source)
                    => (IntPtr)(&source->_state);

                public static RawWaitingEvent* GetEventFromWaitingHandle(IntPtr waitingHandle)
                    => (RawWaitingEvent*)(((nuint*)waitingHandle) - 1);

                public RawWaitingEvent(bool manualReset)
                {
                    _manualReset = manualReset ? 1u : 0u;
                    _state = 0;
                }
            }

        }
    }
}
