#if NET8_0_OR_GREATER
using System;
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    partial interface ITextReader
    {
        // Normal methods (async version + cancellation token)
        ValueTask<int> PeekAsync(CancellationToken token);
        ValueTask<int> ReadAsync(CancellationToken token);
        ValueTask<string?> ReadLineAsync(CancellationToken token);
        ValueTask<string> ReadToEndAsync(CancellationToken token);

        // StringBase methods (async version + cancellation token)
        ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token);
        ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token);
    }
}
#endif
