using System.Threading;

namespace WitherTorch.Common;

public interface ILockable
{
    Lock.Scope EnterLockScope();
}
