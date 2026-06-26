using System;

namespace RiceTea.Core;

public interface ICheckableDisposable : IDisposable
{
    bool IsDisposed { get; }
}
