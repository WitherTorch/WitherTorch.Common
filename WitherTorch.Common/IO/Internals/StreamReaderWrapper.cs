using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    internal sealed class StreamReaderWrapper : IStreamReader
    {
        private readonly StreamReader _reader;

        public StreamReaderWrapper(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        public StreamReaderWrapper(Stream stream, bool detectEncodingFromByteOrderMarks, Encoding encoding, int bufferSize, bool leaveOpen)
        {
            _reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize, leaveOpen);
        }

        public Stream BaseStream => _reader.BaseStream;

        public Encoding CurrentEncoding => _reader.CurrentEncoding;

        public bool EndOfStream => _reader.EndOfStream;

        public int Peek() => _reader.Peek();

        public Task<int> PeekAsync()
            => Task<int>.Factory.StartNew(static (state) => ((StreamReader)state!).Peek(), _reader, 
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public async ValueTask<int> PeekAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return _reader.Peek();
        }

        public int Read() => _reader.Read();

        public Task<int> ReadAsync()
            => Task<int>.Factory.StartNew(static (state) => ((StreamReader)state!).Read(), _reader,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public async ValueTask<int> ReadAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return _reader.Read();
        }

        public string? ReadLine() => _reader.ReadLine();

        public Task<string?> ReadLineAsync() => _reader.ReadLineAsync();

#if NET8_0_OR_GREATER
        public ValueTask<string?> ReadLineAsync(CancellationToken token)
            => _reader.ReadLineAsync(token);
#else
            public async ValueTask<string?> ReadLineAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                    return null;
                return await _reader.ReadLineAsync();
            }
#endif

        public StringBase? ReadLineAsStringBase()
        {
            string? result = _reader.ReadLine();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        public async Task<StringBase?> ReadLineAsStringBaseAsync()
        {
            string? result = await _reader.ReadLineAsync();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        public async ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;
            string? result
#if NET8_0_OR_GREATER
             = await _reader.ReadLineAsync(token);
#else
                 = await _reader.ReadLineAsync();
#endif
            return (result is null || token.IsCancellationRequested) ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        public string ReadToEnd() => _reader.ReadToEnd();

        public Task<string> ReadToEndAsync() => _reader.ReadToEndAsync();

        public async ValueTask<string> ReadToEndAsync(CancellationToken token)
#if NET8_0_OR_GREATER
            => await _reader.ReadToEndAsync(token);
#else
            {
                if (token.IsCancellationRequested)
                    return string.Empty;
                return await _reader.ReadToEndAsync();
            }
#endif

        public StringBase ReadToEndAsStringBase()
            => StringBase.Create(_reader.ReadToEnd(), StringCreateOptions.None);

        public async Task<StringBase> ReadToEndAsStringBaseAsync()
            => StringBase.Create(await _reader.ReadToEndAsync(), StringCreateOptions.None);

        public async ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return StringBase.Empty;
            string result
#if NET8_0_OR_GREATER
             = await _reader.ReadToEndAsync(token);
#else
                 = await _reader.ReadToEndAsync();
#endif
            return StringBase.Create(result, StringCreateOptions.None);
        }

        public void Dispose() => _reader.Dispose();
    }
}
