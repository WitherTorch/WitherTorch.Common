using System;

using InlineMethod;

using WitherTorch.CrossNative.Structures;
using WitherTorch.CrossNative.Windows.Internals;

namespace WitherTorch.CrossNative.Windows.Helpers
{
    public static unsafe class MethodImportHelper
    {
        public static void* GetImportedMethodPointer(string dllName, int methodIndex)
        {
            IntPtr module = Kernel32.LoadLibrary(dllName);
            return Kernel32.GetProcAddress(module, (byte*)methodIndex);
        }

        public static void* GetImportedMethodPointer(string dllName, string methodName)
        {
            IntPtr module = Kernel32.LoadLibrary(dllName);

            IArrayPool<byte> pool = WTCrossNative.ArrayPoolProvider.GetArrayPool<byte>();

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
            IntPtr module = Kernel32.LoadLibrary(dllName);

            IArrayPool<byte> pool = WTCrossNative.ArrayPoolProvider.GetArrayPool<byte>();

            int length = methodNames.Length;
            void*[] pointers = new void*[length];

            for (int i = 0; i < length; i++)
            {
                string methodName = methodNames[i];
                pointers[i] = GetImportedMethodPointerCore(pool, module, methodName);
            }

            return pointers;
        }

        private static void* GetImportedMethodPointerCore(IArrayPool<byte> pool, IntPtr module, string methodName)
        {
            byte[] buffer = pool.Rent(methodName.Length);
            AsciiHelper.ToAsciiUnchecked(buffer, methodName);

            void* result;
            fixed (byte* ptr = buffer)
                result = Kernel32.GetProcAddress(module, ptr);
            pool.Return(buffer);

            return result;
        }
    }
}
