#if NET8_0_OR_GREATER
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO.Internals
{
    partial class CustomStreamReaderBase : IStreamReader
    {
        public async ValueTask<int> PeekAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return Peek();
        }

        public async ValueTask<int> ReadAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return -1;
            return Read();
        }

        public async ValueTask<string?> ReadLineAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return null;
            return ReadLine();
        }

        public async ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return null;
            return ReadLineAsStringBase();
        }

        public async ValueTask<string> ReadToEndAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return string.Empty;
            return ReadToEnd();
        }

        public async ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return StringBase.Empty;
            return ReadToEndAsStringBase();
        }
    }
}
#endif