using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Windows.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe class Shell32
    {
        private const string SHELL32_DLL = "shell32.dll";

        [DllImport(SHELL32_DLL)]
        public static extern int SHCreateItemFromParsingName(char* pszPath, void* pbc, Guid riid, void** ppv);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SHCreateItemFromParsingName(string pszPath, void* pbc, Guid riid, void** ppv)
        {
            fixed (char* ptr = pszPath)
                return SHCreateItemFromParsingName(ptr, pbc, riid, ppv);
        }
    }
}
