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

        public async ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token)
        {
            string? result = await _reader.ReadLineAsync(token);
            return (result is null || token.IsCancellationRequested) ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        public async ValueTask<string> ReadToEndAsync(CancellationToken token)
            => await _reader.ReadToEndAsync(token);

        public async ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token)
            => StringBase.Create(await _reader.ReadToEndAsync(token), StringCreateOptions.None);
    }
}
#endif
