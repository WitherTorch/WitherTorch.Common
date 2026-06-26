namespace RiceTea.Core.Windows.ObjectModels;

public unsafe interface IWin32HandleHolder
{
    void* GetWin32Handle();
}
