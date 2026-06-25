namespace WitherTorch.Common.Threading;

public interface IMonitorLockable
{
    MonitorLockScope EnterLockScope();
}
