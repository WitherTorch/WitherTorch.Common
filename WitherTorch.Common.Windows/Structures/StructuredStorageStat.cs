using System;

namespace WitherTorch.Common.Windows.Structures
{
    public enum StructuredStorageType : uint
    {
        Storage = 1,
        Stream = 2,
        LockBytes = 3,
        Property = 4
    }

    [Flags]
    public enum StructuredStorageStatFlags : uint
    {
        Default = 0,
        NoName = 1,
        NoOpen = 2
    }

    [Flags]
    public enum LockType : uint
    {
        None = 0,
        Write = 1,
        Exclusive = 2,
        OnlyOnce = 4
    }

    public unsafe struct StructuredStorageStat
    {
        public char* pwcsName;
        public StructuredStorageType type;
        public ulong cbSize;
        public ulong mtime;
        public ulong ctime;
        public ulong atime;
        public StructuredStorageStatFlags grfMode;
        public LockType grfLocksSupported;
        public Guid clsid;
        public uint grfStateBits;
        public uint reserved;
    }
}
