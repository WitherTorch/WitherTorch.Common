using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    internal sealed partial class StreamReaderWrapper : IStreamReader
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

        public int Read() => _reader.Read();

        public Task<int> ReadAsync()
            => Task<int>.Factory.StartNew(static (state) => ((StreamReader)state!).Read(), _reader,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public string? ReadLine() => _reader.ReadLine();

        public Task<string?> ReadLineAsync() => _reader.ReadLineAsync();

        public StringWrapper? ReadLineAsStringWrapper()
        {
            string? result = _reader.ReadLine();
            return result is null ? null : StringWrapper.CreateUtf16String(result);
        }

        public async Task<StringWrapper?> ReadLineAsStringWrapperAsync()
        {
            string? result = await _reader.ReadLineAsync();
            return result is null ? null : StringWrapper.CreateUtf16String(result);
        }

        public string ReadToEnd() => _reader.ReadToEnd();

        public Task<string> ReadToEndAsync() => _reader.ReadToEndAsync();

        public StringWrapper ReadToEndAsStringWrapper()
            => StringWrapper.CreateUtf16String(_reader.ReadToEnd());

        public async Task<StringWrapper> ReadToEndAsStringWrapperAsync()
            => StringWrapper.CreateUtf16String(await _reader.ReadToEndAsync());

        public void Dispose() => _reader.Dispose();
    }
}
