using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Luid
    {
        public ulong LowPart;
        public long HighPart;

        public Luid(ulong lowPart, long highPart)
        {
            LowPart = lowPart;
            HighPart = highPart;
        }
    }
}
