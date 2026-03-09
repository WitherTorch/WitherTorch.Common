using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class UnixNativeMethodInstance : INativeMethodInstance
        {
            [SuppressGCTransition]
            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern int gettid();

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* malloc(nuint size);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void free(void* memblock);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memmove(void* dest, void* src, nuint sizeInBytes);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* memcpy(void* dest, void* src, nuint sizeInBytes);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mmap(void* ptr, nuint length, ProtectMemoryPageFlags prot, MemoryMapFlags flags, int fd, nint offset);

            [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
            private static extern void* mprotect(void* ptr, nuint length, ProtectMemoryPageFlags flags);

            [SuppressGCTransition]
            [DllImport("libc", CallingConvention = CallingConvention.StdCall)]
            private static extern int sched_getcpu();

            [SuppressGCTransition]
            [DllImport("libc", CallingConvention = CallingConvention.StdCall)]
            private static extern int clock_gettime(int clk_id, Timespec* t);

            [SuppressGCTransition]
            [DllImport("libc", CallingConvention = CallingConvention.StdCall)]
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
