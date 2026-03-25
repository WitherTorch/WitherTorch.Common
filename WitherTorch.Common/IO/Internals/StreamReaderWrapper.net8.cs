#if NET8_0_OR_GREATER
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    partial class StreamReaderWrapper : IStreamReader
    {
        public async ValueTask<int> PeekAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return _reader.Peek();
        }

        public async ValueTask<int> ReadAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return _reader.Read();
        }

        public ValueTask<string?> ReadLineAsync(CancellationToken token)
            => _reader.ReadLineAsync(token);

        public async ValueTask<StringWrapper?> ReadLineAsStringWrapperAsync(CancellationToken token)
        {
            string? result = await _reader.ReadLineAsync(token);
            return (result is null || token.IsCancellationRequested) ? null : StringWrapper.Create(result, StringCreateOptions.None);
        }

        public async ValueTask<string> ReadToEndAsync(CancellationToken token)
            => await _reader.ReadToEndAsync(token);

        public async ValueTask<StringWrapper> ReadToEndAsStringWrapperAsync(CancellationToken token)
            => StringWrapper.Create(await _reader.ReadToEndAsync(token), StringCreateOptions.None);
    }
}
#endif
