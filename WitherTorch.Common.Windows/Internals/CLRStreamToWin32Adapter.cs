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
        private const uint TransferBufferSize = 81920;

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
                    uint bytesRead = ReadCore(stream, buffer, ptr, TransferBufferSize);
                    if (bytesRead == 0u)
                    {
                        pool.Return(buffer);
                        return result;
                    }
                    ptr += bytesRead;
                    result += bytesRead;
                    length -= bytesRead;
                } while (length >= TransferBufferSize);
                pool.Return(buffer);
            }
            if (length == 0)
                return result;
            return result + ReadSmall(pool, stream, ptr, unchecked((uint)length));
        }

        private static uint ReadSmall(ArrayPool<byte> pool, Stream stream, byte* ptr, uint length)
        {
            byte[] buffer = pool.Rent(length);
            uint bytesRead = ReadCore(stream, buffer, ptr, length);
            if (bytesRead == 0u)
            {
                pool.Return(buffer);
                return 0u;
            }
            pool.Return(buffer);
            return bytesRead;
        }

        private static uint ReadCore(Stream stream, byte[] transferBuffer, byte* destination, uint length)
        {
            int bytesRead = stream.Read(transferBuffer, 0, unchecked((int)length));
            if (bytesRead <= 0)
                return 0;
            fixed (byte* ptr = transferBuffer)
                UnsafeHelper.CopyBlockUnaligned(destination, ptr, length);
            return unchecked((uint)bytesRead);
        }

        public ulong Write(byte* ptr, ulong length)
        {
            Stream stream = _stream;
            if (!stream.CanWrite)
                throw new InvalidOperationException();

            _lastModifiedTime = DateTime.Now;

            ulong result = 0;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer;
            if (length >= TransferBufferSize)
            {
                buffer = pool.Rent(TransferBufferSize);
                do
                {
                    WriteCore(stream, buffer, ptr, TransferBufferSize);
                    ptr += TransferBufferSize;
                    result += TransferBufferSize;
                } while ((length -= TransferBufferSize) >= TransferBufferSize);
                pool.Return(buffer);
            }
            if (length == 0)
                return result;
            return result + WriteSmall(pool, stream, ptr, unchecked((uint)length));
        }

        private static uint WriteSmall(ArrayPool<byte> pool, Stream stream, byte* ptr, uint length)
        {
            byte[] buffer = pool.Rent(length);
            WriteCore(stream, buffer, ptr, length);
            pool.Return(buffer);
            return length;
        }

        private static void WriteCore(Stream stream, byte[] transferBuffer, byte* source, uint length)
        {
            fixed (byte* ptr = transferBuffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, length);
            stream.Write(transferBuffer, 0, unchecked((int)length));
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
            Stream stream = _stream;
            if (!stream.CanRead)
                throw new InvalidOperationException();

            _lastAccessTime = DateTime.Now;

            pcbRead = 0;
            pcbWritten = 0;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            if (cb >= TransferBufferSize)
            {
                byte[] buffer = pool.Rent(TransferBufferSize);
                do
                {
                    CopyToCore(stream, buffer, pstm, TransferBufferSize, out uint bytesRead, out uint bytesWritten);
                    if (bytesRead == 0u)
                    {
                        pool.Return(buffer);
                        return;
                    }
                    if (bytesRead != bytesWritten)
                    {
                        pool.Return(buffer);
                        throw new InvalidOperationException();
                    }
                    pcbRead += bytesRead;
                    pcbWritten += bytesWritten;
                    cb -= bytesRead;
                } while (cb >= TransferBufferSize);
                pool.Return(buffer);
            }
            if (cb == 0)
                return;
            CopyToSmall(pool, stream, pstm, unchecked((uint)cb), ref pcbRead, ref pcbWritten);
        }

        private static void CopyToSmall(ArrayPool<byte> pool, Stream source, IWin32Stream destination, uint length, ref ulong pcbRead, ref ulong pcbWritten)
        {
            byte[] buffer = pool.Rent(length);
            CopyToCore(source, buffer, destination, length, out uint bytesRead, out uint bytesWritten);
            pool.Return(buffer);
            if (bytesRead != bytesWritten)
                throw new InvalidOperationException();
            pcbRead += bytesRead;
            pcbWritten += bytesWritten;
        }

        private static void CopyToCore(Stream source, byte[] buffer, IWin32Stream destination, uint length, out uint bytesRead, out uint bytesWritten)
        {
            bytesRead = MathHelper.MakeUnsigned(source.Read(buffer, 0, unchecked((int)length)));
            if (bytesRead == 0)
            {
                bytesWritten = 0;
                return;
            }
            fixed (byte* ptr = buffer)
                bytesWritten = unchecked((uint)destination.Write(ptr, length));
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
