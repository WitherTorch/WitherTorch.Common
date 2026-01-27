using System;
using System.Diagnostics.CodeAnalysis;

namespace WitherTorch.Common.Windows.ObjectModels
{
    public interface IUnknown : IWin32HandleHolder, IDisposable
    {
        bool TryQueryInterface(in Guid guid, [NotNullWhen(true)] out IUnknown? queriedObject);
        uint AddRef();
        uint Release();
    }
}
