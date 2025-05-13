using System;
using System.Diagnostics.CodeAnalysis;

namespace WitherTorch.Common.Windows.ObjectModels
{
    public interface IUnknown : IDisposable
    {
        bool TryQueryInterface(in Guid guid, [NotNullWhen(true)] out IUnknown? queriedObject);
        ulong AddRef();
        ulong Release();
    }
}
