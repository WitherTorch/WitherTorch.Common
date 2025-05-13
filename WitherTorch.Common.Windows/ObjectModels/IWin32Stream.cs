using System;

using WitherTorch.Common.Windows.Structures;

namespace WitherTorch.Common.Windows.ObjectModels
{
    public enum StreamSeekType : uint
    {
        Head = 0,
        Cursor = 1,
        End = 2
    }

    [Flags]
    public enum StreamCommitFlags : uint
    {
        Default = 0,
        Overwrite = 1,
        OnlyIfCurrent = 2,
        DangerouslyCommitMerelyToDiskCache = 4,
        Consolidate = 8
    }

    public interface IWin32Stream : IWin32SequentialStream
    {
        ulong Seek(long dlibMove, StreamSeekType dwOrigin);

        void SetSize(ulong libNewSize);

        void CopyTo(IWin32Stream pstm, ulong cb, out ulong pcbRead, out ulong pcbWritten);

        void Commit(StreamCommitFlags grfCommitFlags);

        void Revert();

        void LockRegion(ulong libOffset, ulong cb, LockType dwLockType);

        void UnlockRegion(ulong libOffset, ulong cb, LockType dwLockType);

        StructuredStorageStat Stat(StructuredStorageStatFlags grfStatFlag);

        IWin32Stream Clone();
    }
}
