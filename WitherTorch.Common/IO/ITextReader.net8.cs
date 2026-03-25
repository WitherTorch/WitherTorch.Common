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

        // StringWrapper methods (async version + cancellation token)
        ValueTask<StringWrapper?> ReadLineAsStringWrapperAsync(CancellationToken token);
        ValueTask<StringWrapper> ReadToEndAsStringWrapperAsync(CancellationToken token);
    }
}
#endif
