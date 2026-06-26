using System.Threading;

namespace RiceTea.Core.Threading;

public interface ILockable
{
    Lock.Scope EnterLockScope();
}
