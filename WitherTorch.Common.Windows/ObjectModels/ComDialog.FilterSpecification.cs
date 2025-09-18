using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct ComDialogFilterSpecification
    {
        public char* Name;
        public char* Specfication;
    }
}
