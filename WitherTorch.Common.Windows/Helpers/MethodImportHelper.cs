﻿using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Structures;
using WitherTorch.Common.Windows.Internals;

namespace WitherTorch.Common.Windows.Helpers
{
    public static unsafe class MethodImportHelper
    {
        public static void* GetImportedMethodPointer(string dllName, int methodIndex)
        {
            nint module = Kernel32.LoadLibrary(dllName);
            return Kernel32.GetProcAddress(module, (byte*)methodIndex);
        }

        public static void* GetImportedMethodPointer(string dllName, string methodName)
        {
            nint module = Kernel32.LoadLibrary(dllName);

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;

            return GetImportedMethodPointerCore(pool, module, methodName);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName));

        [Inline(InlineBehavior.Keep, export: true)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName1, string methodName2)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName1, methodName2));

        [Inline(InlineBehavior.Keep, export: true)]
        public static void*[] GetImportedMethodPointers(string dllName, string methodName1, string methodName2, string methodName3)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodName1, methodName2, methodName3));

        [Inline(InlineBehavior.Keep, export: true)]
        public static void*[] GetImportedMethodPointers(string dllName, params string[] methodNames)
            => GetImportedMethodPointers(dllName, new ParamArrayTiny<string>(methodNames));

        public static void*[] GetImportedMethodPointers(string dllName, ParamArrayTiny<string> methodNames)
        {
            nint module = Kernel32.LoadLibrary(dllName);

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

        private static void* GetImportedMethodPointerCore(ArrayPool<byte> pool, nint module, string methodName)
        {
            int length = methodName.Length;
            byte[] buffer = pool.Rent(length + 1);
            AsciiHelper.ToAsciiUnchecked(buffer, methodName);
            buffer[length] = 0;

            void* result;
            fixed (byte* ptr = buffer)
                result = Kernel32.GetProcAddress(module, ptr);
            pool.Return(buffer);

            return result;
        }
    }
}
