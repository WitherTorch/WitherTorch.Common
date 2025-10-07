#if NET472_OR_GREATER
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe partial class Shell32
    {
        private const string SHELL32_DLL = "shell32.dll";

        [DllImport(SHELL32_DLL)]
        public static extern partial int SHCreateItemFromParsingName(char* pszPath, void* pbc, Guid riid, void** ppv);
    }
}
#endif