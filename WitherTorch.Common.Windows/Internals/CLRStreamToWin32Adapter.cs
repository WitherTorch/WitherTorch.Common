using System;
using System.IO;
using System.Runtime.InteropServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Windows.ObjectModels;
using WitherTorch.Common.Windows.Structures;

namespace WitherTorch.Common.Windows.Internals
{
    internal sealed unsafe class CLRStreamToWin32Adapter : IWin32Stream, IDisposable
    {
        private readonly Stream _stream;
        private readonly DateTime _creationTime;
        private DateTime _lastModifiedTime, _lastAccessTime;

        public CLRStreamToWin32Adapter(Stream stream)
        {
            _stream = stream;
            _creationTime = DateTime.Now;
            _lastModifiedTime = _creationTime;
            _lastAccessTime = _creationTime;
        }

        public ulong Read(byte* ptr, ulong length)
        {
            const int TransferBufferSize = 8192;

            Stream stream = _stream;
            if (!stream.CanRead)
                throw new InvalidOperationException();

            _lastAccessTime = DateTime.Now;

            ulong result = 0;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            if (length >= TransferBufferSize)
            {
                byte[] buffer = pool.Rent(TransferBufferSize);
                do
                {
                    int bytesRead = stream.Read(buffer, 0, TransferBufferSize);
                    if (bytesRead <= 0)
                    {
                        pool.Return(buffer);
                        return result;
                    }
                    result += unchecked((uint)bytesRead);
                    fixed (byte* pBuffer = buffer)
                        UnsafeHelper.CopyBlockUnaligned(ptr, pBuffer, TransferBufferSize);
                    ptr += TransferBufferSize;
                } while (length >= TransferBufferSize);
                pool.Return(buffer);
            }
            if (length == 0)
                return result;
            {
                byte[] buffer = pool.Rent(unchecked((uint)length));
                int bytesRead = stream.Read(buffer, 0, unchecked((int)length));
                if (bytesRead <= 0)
                {
                    pool.Return(buffer);
                    return result;
                }
                result += unchecked((uint)bytesRead);
                fixed (byte* pBuffer = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptr, pBuffer, unchecked((uint)bytesRead));
                pool.Return(buffer);
            }

            return result;
        }

        public ulong Write(byte* ptr, ulong length)
        {
            const int TransferBufferSize = 8192;

            Stream stream = _stream;
            if (!stream.CanWrite)
                throw new InvalidOperationException();

            _lastModifiedTime = DateTime.Now;

            ulong result = 0;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            if (length >= TransferBufferSize)
            {
                byte[] buffer = pool.Rent(TransferBufferSize);
                do
                {
                    fixed (byte* pBuffer = buffer)
                        UnsafeHelper.CopyBlockUnaligned(pBuffer, ptr, TransferBufferSize);
                    stream.Write(buffer, 0, TransferBufferSize);
                    ptr += TransferBufferSize;
                    result += TransferBufferSize;
                } while (length >= TransferBufferSize);
                pool.Return(buffer);
            }
            if (length == 0)
                return result;
            {
                byte[] buffer = pool.Rent(unchecked((uint)length));
                fixed (byte* pBuffer = buffer)
                    UnsafeHelper.CopyBlockUnaligned(pBuffer, ptr, unchecked((uint)length));
                result += TransferBufferSize;
                stream.Write(buffer, 0, unchecked((int)length));
                pool.Return(buffer);
            }

            return result;
        }

        public ulong Seek(long dlibMove, StreamSeekType dwOrigin)
        {
            Stream stream = _stream;
            if (!stream.CanSeek)
                throw new InvalidOperationException();

            _lastAccessTime = DateTime.Now;

            if (dlibMove == 0 && dwOrigin == StreamSeekType.Cursor)
                return MathHelper.MakeUnsigned(stream.Position);

            _lastModifiedTime = DateTime.Now;

            return MathHelper.MakeUnsigned(stream.Seek(dlibMove, dwOrigin switch
            {
                StreamSeekType.Head => SeekOrigin.Begin,
                StreamSeekType.Cursor => SeekOrigin.Current,
                StreamSeekType.End => SeekOrigin.End,
                _ => throw new ArgumentOutOfRangeException(nameof(dwOrigin), dwOrigin, null)
            }));
        }

        public void SetSize(ulong libNewSize)
        {
            Stream stream = _stream;
            if (!stream.CanWrite || !stream.CanSeek)
                throw new InvalidOperationException();

            _lastModifiedTime = DateTime.Now;

            stream.SetLength(MathHelper.MakeSigned(libNewSize));
        }

        public void Commit(StreamCommitFlags grfCommitFlags) => _stream.Flush();

        public void CopyTo(IWin32Stream pstm, ulong cb, out ulong pcbRead, out ulong pcbWritten)
        {
            const int TransferBufferSize = 8192;

            Stream stream = _stream;
            if (!stream.CanRead)
                throw new InvalidOperationException();

            _lastAccessTime = DateTime.Now;

            pcbRead = 0;
            pcbWritten = 0;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            if (cb > TransferBufferSize)
            {
                byte[] buffer = pool.Rent(TransferBufferSize);
                do
                {
                    uint bytesRead = MathHelper.MakeUnsigned(stream.Read(buffer, 0, TransferBufferSize));
                    if (bytesRead == 0)
                    {
                        pool.Return(buffer);
                        return;
                    }
                    ulong bytesWritten;
                    fixed (byte* ptr = buffer)
                        bytesWritten = pstm.Write(ptr, bytesRead);
                    pcbRead += bytesRead;
                    pcbWritten += bytesWritten;
                    if (bytesWritten != bytesRead)
                    {
                        pool.Return(buffer);
                        throw new InvalidOperationException();
                    }
                    cb -= TransferBufferSize;
                } while (cb > TransferBufferSize);
                pool.Return(buffer);
            }
            if (cb == 0)
                return;
            {
                byte[] buffer = pool.Rent(unchecked((uint)cb));
                uint bytesRead = MathHelper.MakeUnsigned(stream.Read(buffer, 0, unchecked((int)cb)));
                if (bytesRead == 0)
                {
                    pool.Return(buffer);
                    return;
                }
                ulong bytesWritten;
                fixed (byte* ptr = buffer)
                    bytesWritten = pstm.Write(ptr, bytesRead);
                pcbRead += bytesRead;
                pcbWritten += bytesWritten;
                if (bytesWritten != bytesRead)
                {
                    pool.Return(buffer);
                    throw new InvalidOperationException();
                }
                pool.Return(buffer);
            }
        }

        public void Revert() { }

        public void LockRegion(ulong libOffset, ulong cb, LockType dwLockType)
        {
            if (dwLockType == LockType.None)
                return;
            throw new NotImplementedException();
        }

        public void UnlockRegion(ulong libOffset, ulong cb, LockType dwLockType) { }

        public StructuredStorageStat Stat(StructuredStorageStatFlags grfStatFlag)
        {
            StructuredStorageStat result = new StructuredStorageStat()
            {
                pwcsName = null,
                type = StructuredStorageType.Stream,
                cbSize = MathHelper.MakeUnsigned(_stream.Length),
                mtime = MathHelper.MakeUnsigned(_lastModifiedTime.ToFileTime()),
                ctime = MathHelper.MakeUnsigned(_creationTime.ToFileTime()),
                atime = MathHelper.MakeUnsigned(_lastAccessTime.ToFileTime()),
                grfMode = grfStatFlag,
                grfLocksSupported = LockType.None,
                clsid = Guid.Empty,
                grfStateBits = 0,
                reserved = 0
            };
            if ((grfStatFlag & StructuredStorageStatFlags.NoName) != StructuredStorageStatFlags.NoName)
                result.pwcsName = (char*)Marshal.StringToCoTaskMemUni(_stream.GetType().FullName).ToPointer();
            return result;
        }

        public IWin32Stream Clone() => this;

        public void Dispose() => _stream.Dispose();
    }
}
