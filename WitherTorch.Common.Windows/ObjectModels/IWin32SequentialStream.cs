namespace WitherTorch.Common.Windows.ObjectModels
{
    public unsafe interface IWin32SequentialStream
    {
        ulong Read(byte* ptr, ulong length);

        ulong Write(byte* ptr, ulong length);
    }
}
