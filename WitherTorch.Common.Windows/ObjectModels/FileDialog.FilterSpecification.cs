using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct FileDialogFilterSpecification
    {
        public char* Name;
        public char* Specfication;
    }
}
