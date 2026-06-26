using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RiceTea.Core.Windows.Internals;

[SuppressUnmanagedCodeSecurity]
internal static unsafe class Ole32
{
    private const string LibraryName = "ole32.dll";

    [SuppressGCTransition]
    [DllImport(LibraryName, EntryPoint = nameof(CoCreateInstance), CallingConvention = CallingConvention.Winapi)]
    public static extern int CoCreateInstance(Guid* rclsid, void* pUnkOuter, ClassContextFlags dwClsContext, Guid* riid, void** ppv);

    [SuppressGCTransition]
    [DllImport(LibraryName, EntryPoint = nameof(CoTaskMemFree), CallingConvention = CallingConvention.Winapi)]
    public static extern void CoTaskMemFree(void* pv);
}