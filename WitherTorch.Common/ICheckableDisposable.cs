using System;

namespace WitherTorch.Common
{
    public interface ICheckableDisposable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
