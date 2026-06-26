namespace RiceTea.Core.Native;

public enum ReferenceType : long
{
    Disposed = -1,
    NeedBinding = 0,
    Owned = 1,
    Weak = 2,
    _Limit,
}
