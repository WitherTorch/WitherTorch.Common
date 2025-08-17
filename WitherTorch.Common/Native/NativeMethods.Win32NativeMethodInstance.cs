using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [SuppressUnmanagedCodeSecurity]
        private sealed unsafe class Win32NativeMethodInstance : INativeMethodInstance
        {
            private readonly IntPtr _heap;

            [DllImport("kernel32", CallingConvention = CallingConvention.StdCall)]
            private static extern int GetCurrentThreadId();

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

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlMoveMemory(void* dest, void* src, nuint sizeInBytes);

            [DllImport("ntdll", CallingConvention = CallingConvention.StdCall)]
            private static extern void RtlCopyMemory(void* dest, void* src, nuint sizeInBytes);

            public Win32NativeMethodInstance()
            {
                _heap = GetProcessHeap();
            }

            int INativeMethodInstance.GetCurrentThreadId() => GetCurrentThreadId();

            void* INativeMethodInstance.AllocMemory(nuint size) => HeapAlloc(_heap, 0, size);

            void INativeMethodInstance.FreeMemory(void* ptr) => HeapFree(_heap, 0, ptr);

            void INativeMethodInstance.CopyMemory(void* destination, void* source, nuint sizeInBytes) => RtlCopyMemory(destination, source, sizeInBytes);

            void INativeMethodInstance.MoveMemory(void* destination, void* source, nuint sizeInBytes) => RtlMoveMemory(destination, source, sizeInBytes);

            void* INativeMethodInstance.AllocMemoryPage(nuint size, ProtectMemoryPageFlags flags) 
                => VirtualAlloc(null, size, MemoryAllocationTypes.Commit | MemoryAllocationTypes.Reserve, ConvertPageAccessRightsFromFlags(flags));

            void INativeMethodInstance.ProtectMemoryPage(void* ptr, nuint size, ProtectMemoryPageFlags flags)
            {
                PageAccessRights rights = ConvertPageAccessRightsFromFlags(flags);
                VirtualProtect(ptr, size, rights, &rights);
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
        }
    }
}
