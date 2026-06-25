using System.Threading;

namespace WitherTorch.Common.Threading;

public interface ILockable
{
    Lock.Scope EnterLockScope();
}
