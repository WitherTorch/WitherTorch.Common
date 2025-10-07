#if NET472_OR_GREATER
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe partial class Ole32
    {
        private const string LibraryName = "ole32.dll";

        [DllImport(LibraryName, EntryPoint = nameof(CoCreateInstance), CallingConvention = CallingConvention.Winapi)]
        public static unsafe extern partial int CoCreateInstance(Guid* rclsid, void* pUnkOuter, ClassContextFlags dwClsContext, Guid* riid, void** ppv);

        [DllImport(LibraryName, EntryPoint = nameof(CoTaskMemFree), CallingConvention = CallingConvention.Winapi)]
        public static unsafe extern partial void CoTaskMemFree(void* pv);
    }
}
#endif