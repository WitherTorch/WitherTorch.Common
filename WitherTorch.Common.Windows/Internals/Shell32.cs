using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Windows.Internals
{
    internal static unsafe partial class Shell32
    {
        public static partial int SHCreateItemFromParsingName(char* pszPath, void* pbc, Guid riid, void** ppv);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SHCreateItemFromParsingName(string pszPath, void* pbc, Guid riid, void** ppv)
        {
            fixed (char* ptr = pszPath)
                return SHCreateItemFromParsingName(ptr, pbc, riid, ppv);
        }
    }
}
