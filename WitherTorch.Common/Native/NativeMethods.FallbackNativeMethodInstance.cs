using System;
using System.Runtime.InteropServices;
using System.Threading;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        private sealed unsafe class FallbackNativeMethodInstance : INativeMethodInstance
        {
            public int GetCurrentThreadId() => Environment.CurrentManagedThreadId;

            public int GetCurrentProcessorId()
            {
#if NET8_0_OR_GREATER
                return Thread.GetCurrentProcessorId();
#else
                return Thread.CurrentThread.ManagedThreadId;
#endif
            }

            public ulong GetTicksForSystem()
#if NET8_0_OR_GREATER
                => (ulong)Environment.TickCount64;
#else
                => (ulong)(Environment.TickCount & uint.MaxValue);
#endif

            public bool SleepInRelativeTicks(ulong ticks)
            {
                if (ticks <= 0)
                    return false;
                SleepCore(ticks);
                return true;
            }

            public bool SleepInAbsoluteTicks(ulong ticks)
            {
                ulong currentTicks = GetTicksForSystem();
                if (ticks <= currentTicks)
                    return false;
                SleepCore(ticks - currentTicks);
                return true;
            }

            public void* AllocMemory(nuint size) => Marshal.AllocHGlobal(unchecked((nint)size)).ToPointer();

            public void FreeMemory(void* ptr) => Marshal.FreeHGlobal(new IntPtr(ptr));

            public void CopyMemory(void* destination, void* source, nuint sizeInBytes)
                => UnsafeHelper.CopyBlockUnaligned(destination, source, sizeInBytes);

            public void MoveMemory(void* destination, void* source, nuint sizeInBytes)
                => Buffer.MemoryCopy(source, destination, sizeInBytes, sizeInBytes);

            public void* AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags) => AllocMemory(size);

            public void ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
            {
                // Do nothing
            }

            [Inline(InlineBehavior.Remove)]
            private static void SleepCore(ulong ticks) => Thread.Sleep((int)MathHelper.MakeSigned(ticks / TimeSpan.TicksPerMillisecond));
        }
    }
}
