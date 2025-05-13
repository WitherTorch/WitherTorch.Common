using System;

using WitherTorch.Common.Native;
using WitherTorch.Common.Windows.Structures;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    public sealed unsafe class Win32StreamAdapter : Win32SequentialStreamAdapter, IWin32Stream
    {
        private readonly IWin32Stream _stream;

        public Win32StreamAdapter(IWin32Stream stream) : base(stream) { _stream = stream; }

        public ulong Seek(long dlibMove, StreamSeekType dwOrigin) => _stream.Seek(dlibMove, dwOrigin);

        public void SetSize(ulong libNewSize) => _stream.SetSize(libNewSize);

        public void CopyTo(IWin32Stream pstm, ulong cb, out ulong pcbRead, out ulong pcbWritten) => _stream.CopyTo(pstm, cb, out pcbRead, out pcbWritten);

        public void Commit(StreamCommitFlags grfCommitFlags) => _stream.Commit(grfCommitFlags);

        public void Revert() => _stream.Revert();

        public void LockRegion(ulong libOffset, ulong cb, LockType dwLockType) => _stream.LockRegion(libOffset, cb, dwLockType);

        public void UnlockRegion(ulong libOffset, ulong cb, LockType dwLockType) => _stream.UnlockRegion(libOffset, cb, dwLockType);

        public StructuredStorageStat Stat(StructuredStorageStatFlags grfStatFlag) => _stream.Stat(grfStatFlag);

        public IWin32Stream Clone() => _stream.Clone();

        protected override bool IsGuidSupported(in Guid guid) => base.IsGuidSupported(guid) || guid == Win32Stream.IID_IStream;

        protected override int GetMethodTableLength() => base.GetMethodTableLength() + 9;

        protected override void FillMethodTable(ref VTableStack table)
        {
            base.FillMethodTable(ref table);
            table.Push((delegate*<NativeDataHolder*, long, StreamSeekType, ulong*, uint>)&Seek);
            table.Push((delegate*<NativeDataHolder*, ulong, uint>)&SetSize);
            table.Push((delegate*<NativeDataHolder*, void*, ulong, ulong*, ulong*, uint>)&CopyTo);
            table.Push((delegate*<NativeDataHolder*, StreamCommitFlags, uint>)&Commit);
            table.Push((delegate*<NativeDataHolder*, uint>)&Revert);
            table.Push((delegate*<NativeDataHolder*, ulong, ulong, LockType, uint>)&LockRegion);
            table.Push((delegate*<NativeDataHolder*, ulong, ulong, LockType, uint>)&UnlockRegion);
            table.Push((delegate*<NativeDataHolder*, StructuredStorageStatFlags, StructuredStorageStat*, uint>)&Stat);
            table.Push((delegate*<NativeDataHolder*, void**, uint>)&Clone);
        }

        private static uint Seek(NativeDataHolder* nativePointer, long dlibMove, StreamSeekType dwOrigin, ulong* plibNewPosition)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDFUNCTION = 0x80030001;

            if (dwOrigin > StreamSeekType.End)
                return STG_E_INVALIDFUNCTION;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return STG_E_INVALIDFUNCTION;

            ulong result;
            try
            {
                result = adapter.Seek(dlibMove, dwOrigin);
            }
            catch (Exception)
            {
                return STG_E_INVALIDFUNCTION;
            }

            if (plibNewPosition != null)
                *plibNewPosition = result;

            return S_OK;
        }

        private static uint SetSize(NativeDataHolder* nativePointer, ulong libNewSize)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDFUNCTION = 0x80030001;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return STG_E_INVALIDFUNCTION;

            try
            {
                adapter.SetSize(libNewSize);
            }
            catch (Exception)
            {
                return STG_E_INVALIDFUNCTION;
            }

            return S_OK;
        }

        private static uint CopyTo(NativeDataHolder* nativePointer, void* pstm, ulong cb, ulong* pcbRead, ulong* pcbWritten)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDFUNCTION = 0x80030001;
            const uint STG_E_INVALIDPOINTER = 0x80030009;

            if (pstm == null || !nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return STG_E_INVALIDPOINTER;

            try
            {
                adapter.CopyTo(new Win32Stream(pstm, ReferenceType.Weak), cb, out ulong tempRead, out ulong tempWritten);
                if (pcbRead != null)
                    *pcbRead = tempRead;
                if (pcbWritten != null)
                    *pcbWritten = tempWritten;
            }
            catch (Exception)
            {
                return STG_E_INVALIDFUNCTION;
            }

            return S_OK;
        }

        private static uint Commit(NativeDataHolder* nativePointer, StreamCommitFlags flags)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_MEDIUMFULL = 0x80030070;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return S_OK;

            try
            {
                adapter.Commit(flags);
            }
            catch (Exception)
            {
                return STG_E_MEDIUMFULL;
            }

            return S_OK;
        }

        private static uint Revert(NativeDataHolder* nativePointer)
        {
            const uint S_OK = 0x00000000;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return S_OK;

            try
            {
                adapter.Revert();
            }
            catch (Exception)
            {
            }

            return S_OK;
        }

        private static uint LockRegion(NativeDataHolder* nativePointer, ulong libOffset, ulong cb, LockType dwLockType)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDFUNCTION = 0x80030001;
            const uint STG_E_LOCKVIOLATION = 0x80030021;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return S_OK;

            try
            {
                adapter.LockRegion(libOffset, cb, dwLockType);
            }
            catch (InvalidOperationException)
            {
                return STG_E_LOCKVIOLATION;
            }
            catch (Exception)
            {
                return STG_E_INVALIDFUNCTION;
            }

            return S_OK;
        }

        private static uint UnlockRegion(NativeDataHolder* nativePointer, ulong libOffset, ulong cb, LockType dwLockType)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDFUNCTION = 0x80030001;
            const uint STG_E_LOCKVIOLATION = 0x80030021;

            if (!nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return S_OK;

            try
            {
                adapter.UnlockRegion(libOffset, cb, dwLockType);
            }
            catch (InvalidOperationException)
            {
                return STG_E_LOCKVIOLATION;
            }
            catch (Exception)
            {
                return STG_E_INVALIDFUNCTION;
            }

            return S_OK;
        }

        private static uint Stat(NativeDataHolder* nativePointer, StructuredStorageStatFlags grfStatFlag, StructuredStorageStat* pStatstg)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDPOINTER = 0x80030009;
            const uint STG_E_INVALIDFLAG = 0x800300FF;
            const uint STG_E_ACCESSDENIED = 0x80030005;

            if ((grfStatFlag & ~(StructuredStorageStatFlags.NoOpen | StructuredStorageStatFlags.NoName)) != StructuredStorageStatFlags.Default)
                return STG_E_INVALIDFLAG;

            if (pStatstg == null || !nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return STG_E_INVALIDPOINTER;
            try
            {
                *pStatstg = adapter.Stat(grfStatFlag);
            }
            catch (Exception)
            {
                return STG_E_ACCESSDENIED;
            }
            return S_OK;
        }

        private static uint Clone(NativeDataHolder* nativePointer, void** ppstm)
        {
            const uint S_OK = 0x00000000;
            const uint STG_E_INVALIDPOINTER = 0x80030009;
            const uint STG_E_INSUFFICIENTMEMORY = 0x80030008;

            if (ppstm == null || !nativePointer->TryGetAdapter(out Win32StreamAdapter? adapter))
                return STG_E_INVALIDPOINTER;

            IWin32Stream clonedStream;
            try
            {
                clonedStream = adapter.Clone();
            }
            catch (Exception)
            {
                return STG_E_INSUFFICIENTMEMORY;
            }
            if (clonedStream is ComObject comObject)
                *ppstm = comObject.NativePointer;
            else if (clonedStream is Win32StreamAdapter clonedAdapter)
                *ppstm = clonedAdapter.GetWin32Handle();
            else
                *ppstm = new Win32StreamAdapter(clonedStream).GetWin32Handle();

            return S_OK;
        }
    }
}
