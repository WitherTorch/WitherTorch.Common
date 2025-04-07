using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe class Ole32
    {
        private const string LibraryName = "ole32.dll";

        [DllImport(LibraryName, EntryPoint = nameof(CoCreateInstance), CallingConvention = CallingConvention.Winapi)]
        public static unsafe extern int CoCreateInstance(Guid* rclsid, void* pUnkOuter, ClassContextFlags dwClsContext, Guid* riid, void** ppv);
    }
}
