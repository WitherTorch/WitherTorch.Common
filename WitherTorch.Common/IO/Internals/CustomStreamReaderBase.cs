using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Native;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    internal abstract class CustomStreamReaderBase : IStreamReader
    {
        private readonly Stream _stream;
        private readonly byte[] _buffer;
        private readonly object _syncLock;
        private readonly bool _leaveOpen;

        protected nuint _bufferPos, _bufferLength;
        private bool _eofReached, _disposed;

        public Stream BaseStream => _stream;
        public abstract Encoding CurrentEncoding { get; }
        public bool EndOfStream => CheckEndOfStream(fullyCheck: true);

        public CustomStreamReaderBase(Stream stream, int bufferSize, bool leaveOpen)
        {
            _stream = stream;
            _buffer = new byte[bufferSize];
            _leaveOpen = leaveOpen;
            _syncLock = new object();

            _bufferPos = 0;
            _bufferLength = 0;
            _eofReached = false;
            _disposed = false;
        }

        protected bool CheckEndOfStream(bool fullyCheck)
        {
            lock (_syncLock)
                return _eofReached || (fullyCheck && CheckEndOfStreamCore());
        }

        public int Peek()
        {
            lock (_syncLock)
                return ReadCharacterCore(_buffer, movePosition: false) ?? -1;
        }

        public int Read()
        {
            lock (_syncLock)
                return ReadCharacterCore(_buffer, movePosition: true) ?? -1;
        }

        public string? ReadLine()
        {
            lock (_syncLock)
                return ReadLineCore(_buffer);
        }

        public StringBase? ReadLineAsStringBase()
        {
            lock (_syncLock)
                return ReadLineAsStringBaseCore(_buffer);
        }

        public string ReadToEnd()
        {
            lock (_syncLock)
                return ReadToEndCore(_buffer);  
        }

        public StringBase ReadToEndAsStringBase()
        {
            lock (_syncLock)
                return ReadToEndAsStringBaseCore(_buffer);
        }

        protected virtual bool CheckEndOfStreamCore() => _bufferPos >= _bufferLength;

        protected abstract char? ReadCharacterCore(byte[] buffer, bool movePosition);
        protected abstract string? ReadLineCore(byte[] buffer);
        protected abstract StringBase? ReadLineAsStringBaseCore(byte[] buffer);
        protected abstract string ReadToEndCore(byte[] buffer);
        protected abstract StringBase ReadToEndAsStringBaseCore(byte[] buffer);

        protected unsafe void ReadStream()
        {
            if (_eofReached)
                return;
            nuint currentPos = _bufferPos;
            nuint currentLength = _bufferLength;
            if (currentPos == 0) // Just expand buffer
            {
                int castedCurrentLength = unchecked((int)currentLength);
                if (castedCurrentLength < _buffer.Length) // Buffer has space
                {
                    nuint readLength = ReadStreamCore(castedCurrentLength);
                    _bufferLength += readLength;
                }
                return;
            }
            if (currentPos >= currentLength) // buffer is empty
            {
                _bufferPos = 0;
                _bufferLength = ReadStreamCore(0);
                return;
            }
            nuint newPos = currentLength - currentPos;
            fixed (byte* ptr = _buffer)
                NativeMethods.MoveMemory(ptr, ptr + currentPos, newPos);
            _bufferPos = newPos;
            _bufferLength = newPos + ReadStreamCore(unchecked((int)newPos));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private nuint ReadStreamCore(int position)
        {
            byte[] buffer = _buffer;
            int length = _stream.Read(buffer, position, buffer.Length - position);
            if (length <= 0)
            {
                _eofReached = true;
                return 0;
            }
            return unchecked((nuint)length);
        }

        public async Task<int> PeekAsync() => await PeekAsync(CancellationToken.None);

        public async ValueTask<int> PeekAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return Peek();
        }

        public async Task<int> ReadAsync() => await ReadAsync(CancellationToken.None);

        public async ValueTask<int> ReadAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return Read();
        }

        public async Task<string?> ReadLineAsync() => await ReadLineAsync(CancellationToken.None);

        public async ValueTask<string?> ReadLineAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return null;
            return ReadLine();
        }

        public async Task<StringBase?> ReadLineAsStringBaseAsync() => await ReadLineAsStringBaseAsync(CancellationToken.None);

        public async ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return null;
            return ReadLineAsStringBase();
        }

        public async Task<string> ReadToEndAsync() => await ReadToEndAsync(CancellationToken.None);

        public async ValueTask<string> ReadToEndAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return string.Empty;
            return ReadToEnd();
        }

        public async Task<StringBase> ReadToEndAsStringBaseAsync() => await ReadToEndAsStringBaseAsync(CancellationToken.None);

        public async ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return StringBase.Empty;
            return ReadToEndAsStringBase();
        }

        ~CustomStreamReaderBase() => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            DisposeCore(disposing);
        }

        protected void DisposeCore(bool disposing)
        {
            if (_leaveOpen)
                return;
            _stream.Dispose();
        }
    }
}
