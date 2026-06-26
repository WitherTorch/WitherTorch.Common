namespace RiceTea.Core.Threading;

public interface IMonitorLockable
{
    MonitorLockScope EnterLockScope();
}
