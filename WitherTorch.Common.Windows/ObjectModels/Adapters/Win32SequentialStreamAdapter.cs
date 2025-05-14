using System;
using System.IO;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    public unsafe class Win32SequentialStreamAdapter : CustomUnknownAdapterBase, IWin32SequentialStream
    {
        private readonly IWin32SequentialStream _stream;

        public Win32SequentialStreamAdapter(IWin32SequentialStream stream)
        {
            _stream = stream;
        }

        public ulong Read(byte* ptr, ulong length) => _stream.Read(ptr, length);

        public ulong Write(byte* ptr, ulong length) => _stream.Write(ptr, length);

        protected override int GetMethodTableLength() => base.GetMethodTableLength() + 2;

        protected override void FillMethodTable(ref VTableStack table)
        {
            base.FillMethodTable(ref table);
            table.Push((delegate*<NativeDataHolder*, void*, ulong, ulong*, uint>)&Read);
            table.Push((delegate*<NativeDataHolder*, void*, ulong, ulong*, uint>)&Write);
        }

        private static uint Read(NativeDataHolder* nativePointer, void* pv, ulong cb, ulong* pcbRead)
        {
            const uint S_OK = 0x00000000;
            const uint S_FALSE = 0x00000001;
            const uint STG_E_INVALIDPOINTER = 0x80030009;
            const uint STG_E_ACCESSDENIED = 0x80030005;

            if (pv == null || !nativePointer->TryGetAdapter(out Win32SequentialStreamAdapter? adapter))
                return STG_E_INVALIDPOINTER;

            ulong result;
            try
            {
                result = adapter.Read((byte*)pv, cb);
            }
            catch (ArgumentOutOfRangeException)
            {
                result = 0UL;
            }
            catch (IOException)
            {
                return STG_E_ACCESSDENIED;
            }
            catch (NotSupportedException)
            {
                return STG_E_ACCESSDENIED;
            }
            catch (Exception)
            {
                result = 0UL;
            }
            if (pcbRead != null)
                *pcbRead = result;

            return result == cb ? S_OK : S_FALSE;
        }

        private static uint Write(NativeDataHolder* nativePointer, void* pv, ulong cb, ulong* pcbWritten)
        {
            const uint S_OK = 0x00000000;
            const uint S_FALSE = 0x00000001;
            const uint STG_E_INVALIDPOINTER = 0x80030009;
            const uint STG_E_CANTSAVE = 0x80030103;
            const uint STG_E_ACCESSDENIED = 0x80030005;
            const uint STG_E_MEDIUMFULL = 0x80030070;

            if (pv == null || !nativePointer->TryGetAdapter(out Win32SequentialStreamAdapter? adapter))
                return STG_E_INVALIDPOINTER;

            ulong result;
            try
            {
                result = adapter.Write((byte*)pv, cb);
            }
            catch (ArgumentOutOfRangeException)
            {
                return STG_E_MEDIUMFULL;
            }
            catch (IOException)
            {
                return STG_E_ACCESSDENIED;
            }
            catch (NotSupportedException)
            {
                return STG_E_ACCESSDENIED;
            }
            catch (Exception)
            {
                return STG_E_CANTSAVE;
            }
            if (pcbWritten != null)
                *pcbWritten = result;

            return result == cb ? S_OK : S_FALSE;
        }
    }
}
