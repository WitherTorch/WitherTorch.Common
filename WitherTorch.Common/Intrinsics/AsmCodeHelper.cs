using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Intrinsics
{
    public static unsafe class AsmCodeHelper
    {
        private static readonly object _syncLock = new object();
        private static readonly nuint _pageSize = unchecked((nuint)Environment.SystemPageSize);

        private static byte* _pageStartAddress, _pageNextAddress, _pageEndAddress;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* PackAsmCodeIntoNativeMemory(byte[] source, nuint length)
        {
            fixed (byte* ptr = source)
                return PackAsmCodeIntoNativeMemory(ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* PackAsmCodeIntoNativeMemory(byte* source, nuint length)
        {
            byte* result = GetValidStartAddress(length);
            UnsafeHelper.CopyBlockUnaligned(result, source, length);
            return result;
        }

        private static byte* GetValidStartAddress(nuint requestedSize)
        {
            lock (_syncLock)
                return GetValidStartAddressCore(requestedSize);
        }

        [Inline(InlineBehavior.Remove)]
        private static byte* GetValidStartAddressCore(nuint requestedSize)
        {
            nuint addressAlignment = UnsafeHelper.PointerSizeUnsigned;

            byte* result = _pageNextAddress;
            if (result == null)
                goto NewAllocate;

            byte* pageEndAddress = _pageEndAddress;
            if (result + requestedSize > pageEndAddress)
                goto ChangeAddress;

            _pageNextAddress = result + CeilDiv_Internal(requestedSize, addressAlignment) * addressAlignment;
            goto Result;

        ChangeAddress:
            byte* pageStartAddress = _pageStartAddress;
            NativeMethods.ProtectMemoryPage(pageStartAddress, unchecked((nuint)(pageEndAddress - pageStartAddress)),
                NativeMethods.ProtectMemoryPageFlags.CanExecute);

        NewAllocate:
            nuint pageSize = _pageSize;
            if (requestedSize > pageSize)
                pageSize = CeilDiv_Internal(requestedSize, pageSize) * pageSize;
            result = (byte*)NativeMethods.AllocMemoryPage(pageSize, NativeMethods.ProtectMemoryPageFlags.CanExecute |
                NativeMethods.ProtectMemoryPageFlags.CanWrite | NativeMethods.ProtectMemoryPageFlags.CanRead);
            _pageStartAddress = result;
            _pageNextAddress = result + CeilDiv_Internal(requestedSize, addressAlignment) * addressAlignment;
            _pageEndAddress = result + pageSize;

        Result:
            return result;
        }

        private static nuint CeilDiv_Internal(nuint a, nuint b) // 由於 MathHelper 也會使用這個類別，為避免造成循環參考，故在這裡重新實作一份 CeilDiv
        {
            nuint quotient = a / b;
            return quotient + MathHelper.BooleanToNativeUnsigned((a - quotient * b) != 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InjectAsmCode(RuntimeMethodHandle method, byte[] source, nuint length)
        {
            fixed (byte* ptr = source)
                InjectAsmCode(method, ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InjectAsmCode(RuntimeMethodHandle method, byte* source, nuint length)
        {
            RuntimeHelpers.PrepareMethod(method);

            void* dest = (void*)method.GetFunctionPointer();

            while (true)
            {
                switch (*(byte*)dest)
                {
                    case 0xE8:
                    case 0xE9:
                        int offset = *(int*)((byte*)dest + 1);
                        dest = (byte*)dest + 5 + offset;
                        continue;
                    case 0xEB:
                        sbyte shortOffset = *(sbyte*)((byte*)dest + 1);
                        dest = (byte*)dest + 2 + shortOffset;
                        continue;
                    default:
                        goto OutOfLoop;
                }
            }
        OutOfLoop:

            NativeMethods.ProtectMemoryPage(dest, length, 
                NativeMethods.ProtectMemoryPageFlags.CanRead | 
                NativeMethods.ProtectMemoryPageFlags.CanWrite | 
                NativeMethods.ProtectMemoryPageFlags.CanExecute);
            UnsafeHelper.CopyBlock(dest, source, length);
            NativeMethods.FlushInstructionCache(dest, length);
        }
    }
}
