using System;

namespace WitherTorch.Common.Windows.Internals
{
    internal static unsafe partial class Ole32
    {
        public static unsafe partial int CoCreateInstance(Guid* rclsid, void* pUnkOuter, ClassContextFlags dwClsContext, Guid* riid, void** ppv);
        public static unsafe partial void CoTaskMemFree(void* pv);
    }
}
