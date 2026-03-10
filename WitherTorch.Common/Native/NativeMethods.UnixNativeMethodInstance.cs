using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Structures;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class UnixNativeMethodInstance : INativeMethodInstance
        {
            private static readonly void* _gettidFunc;
            private static readonly int _syscallGetTIDIndex;

            static UnixNativeMethodInstance()
            {
                void* func = GetImportedMethodPointerCore(null, nameof(gettid));
                if (func is null)
                {
#if NET8_0_OR_GREATER
                    func = (delegate* unmanaged[Cdecl]<int>)&gettid_fallback;
#else
                    func = (delegate*<int>)&gettid_fallback;
#endif
                    _syscallGetTIDIndex = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? 178 : 186;
                }
                _gettidFunc = func;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int gettid() => ((delegate* unmanaged[Cdecl]<int>)_gettidFunc)();

            [SuppressGCTransition]
            [DllImport("c", EntryPoint = nameof(syscall))]
            private static extern nint syscall_fast(nint number);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern nint syscall(nint number);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* malloc(nuint size);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void free(void* memblock);

            [DllImport("dl", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr dlopen(byte* filename, int flags);

            [DllImport("dl", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* dlsym(IntPtr handle, byte* symbol);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memmove(void* dest, void* src, nuint sizeInBytes);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memcpy(void* dest, void* src, nuint sizeInBytes);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mmap(void* ptr, nuint length, ProtectMemoryPageFlags prot, MemoryMapFlags flags, int fd, nint offset);

            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mprotect(void* ptr, nuint length, ProtectMemoryPageFlags flags);

            [SuppressGCTransition]
            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern int sched_getcpu();

            [SuppressGCTransition]
            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern int clock_gettime(int clk_id, Timespec* t);

            [SuppressGCTransition]
            [DllImport("c", CallingConvention = CallingConvention.Cdecl)]
            private static extern int clock_nanosleep(int clk_id, int flags, Timespec* t, Timespec* remain);

            public int GetCurrentThreadId() => gettid();

            public int GetCurrentProcessorId() => sched_getcpu();

            public ulong GetTicksForSystem()
            {
                const int CLOCK_MONOTONIC = 1;

                Timespec ts;
                if (clock_gettime(CLOCK_MONOTONIC, &ts) != 0)
                {
#if NET8_0_OR_GREATER
                    return (ulong)Environment.TickCount64;
#else
                    return (ulong)(Environment.TickCount & uint.MaxValue);
#endif
                }
                return ts.tv_sec * TimeSpan.TicksPerSecond + ts.tv_nsec / 100;
            }

            public void* GetImportedMethodPointer(string? dllName, int methodIndex) => null;

            public void* GetImportedMethodPointer(string? dllName, string methodName) => GetImportedMethodPointerCore(dllName, methodName);

            public void*[] GetImportedMethodPointers(string? dllName, in ParamArrayTiny<int> methodIndices) => new void*[methodIndices.Length];

            public void*[] GetImportedMethodPointers(string? dllName, in ParamArrayTiny<string> methodNames) => GetImportedMethodPointersCore(dllName, methodNames);

            public IntPtr CreateWaitingHandle(bool initialState, bool autoReset)
            {
                throw new NotImplementedException();
            }

            public void ResetWaitingHandle(IntPtr handle)
            {
                throw new NotImplementedException();
            }

            public void SetWaitingHandle(IntPtr handle)
            {
                throw new NotImplementedException();
            }

            public void DestroyWaitingHandle(IntPtr handle)
            {
                throw new NotImplementedException();
            }

            public bool WaitForWaitingHandle(IntPtr handle, uint timeout)
            {
                throw new NotImplementedException();
            }

            public bool SleepInRelativeTicks(ulong ticks)
            {
                const int CLOCK_MONOTONIC = 1;
                const int EINTR = 4;

                if (ticks <= 0)
                    return false;

                nuint secs = (nuint)(ticks / TimeSpan.TicksPerSecond);
                nuint nanos = (nuint)((ticks % TimeSpan.TicksPerSecond) * 100);
                Timespec ts = new Timespec() { tv_sec = secs, tv_nsec = nanos };
                while (clock_nanosleep(CLOCK_MONOTONIC, flags: 0, &ts, &ts) == EINTR) ;
                return true;
            }

            public bool SleepInAbsoluteTicks(ulong ticks)
            {
                const int CLOCK_MONOTONIC = 1;
                const int TIMER_ABSTIME = 1;
                const int EINTR = 4;

                Timespec ts;
                if (clock_gettime(CLOCK_MONOTONIC, &ts) != 0)
                    return false;
                nuint secs = (nuint)(ticks / TimeSpan.TicksPerSecond);
                nuint nanos = (nuint)((ticks % TimeSpan.TicksPerSecond) * 100);
                if (ts.tv_sec > secs || (ts.tv_sec == secs && ts.tv_nsec >= nanos))
                    return false;
                ts.tv_sec = secs;
                ts.tv_nsec = nanos;
                while (clock_nanosleep(CLOCK_MONOTONIC, flags: TIMER_ABSTIME, &ts, null) == EINTR) ;
                return true;
            }

            public void* AllocMemory(nuint size) => malloc(size);

            public void FreeMemory(void* ptr) => free(ptr);

            public void CopyMemory(void* destination, void* source, nuint sizeInBytes) => memcpy(destination, source, sizeInBytes);

            public void MoveMemory(void* destination, void* source, nuint sizeInBytes) => memmove(destination, source, sizeInBytes);

            public void* AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags)
                => mmap(null, size, flags, MemoryMapFlags.Private | MemoryMapFlags.Anomymous, -1, 0);

            public void ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
                => mprotect(ptr, size, flags);

            private static void* GetImportedMethodPointerCore(string? dllName, string methodName)
            {
                const int RTLD_NOW = 2;
                const int RTLD_LOCAL = 0;

                IntPtr module = dlopen(dllName, RTLD_NOW | RTLD_LOCAL);

                ArrayPool<byte> pool = ArrayPool<byte>.Shared;

                return GetImportedMethodPointerCore(pool, module, methodName);
            }

            private static void*[] GetImportedMethodPointersCore(string? dllName, ParamArrayTiny<string> methodNames)
            {
                const int RTLD_NOW = 2;
                const int RTLD_LOCAL = 0;

                IntPtr module = dlopen(dllName, RTLD_NOW | RTLD_LOCAL);

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
                        return dlsym(module, destination);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static IntPtr dlopen(string? filename, int flags)
            {
                if (filename is null)
                    return dlopen((byte*)null, flags);

                int length = filename.Length;

                ArrayPool<byte> pool = ArrayPool<byte>.Shared;
                byte[] buffer = pool.Rent(Utf8EncodingHelper.GetWorstCaseForEncodeLength(length) + 1);
                try
                {
                    fixed (char* source = filename)
                    fixed (byte* destination = buffer)
                    {
                        Utf8EncodingHelper.ReadFromUtf16Buffer(source, source + length, destination, destination + buffer.Length);
                        return dlopen(destination, flags);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }

#if NET8_0_OR_GREATER
            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
#endif
            private static int gettid_fallback() => (int)syscall_fast(_syscallGetTIDIndex);

            private enum MemoryMapFlags : uint
            {
                Failed = unchecked((uint)-1),

                Shared = 0x01,
                Private = 0x02,
                SharedValidate = 0x03,
                Fixed = 0x10,
                Anomymous = 0x20,
                NoReserve = 0x4000,
                GrowsDown = 0x0100,
                DenyWrite = 0x0800,
                Executable = 0x1000,
                Locked = 0x2000,
                Populate = 0x8000,
                NonBlock = 0x10000,
                Stack = 0x20000,
                HugeTlb = 0x40000,
                Sync = 0x80000,
                FixedNoReplace = 0x100000,
                File = 0
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct Timespec
            {
                public nuint tv_sec;
                public nuint tv_nsec;
            }
        }
    }
}
