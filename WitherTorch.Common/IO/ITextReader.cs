using System;
using System.Threading;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    public interface ITextReader : IDisposable
    {
        // Normal methods
        int Peek();
        int Read();
        string? ReadLine();
        string ReadToEnd();

        // Normal methods (async version)
        Task<int> PeekAsync();
        ValueTask<int> PeekAsync(CancellationToken token);
        Task<int> ReadAsync();
        ValueTask<int> ReadAsync(CancellationToken token);
        Task<string?> ReadLineAsync();
        ValueTask<string?> ReadLineAsync(CancellationToken token);
        Task<string> ReadToEndAsync();
        ValueTask<string> ReadToEndAsync(CancellationToken token);

        // StringBase methods
        StringBase? ReadLineAsStringBase();
        StringBase ReadToEndAsStringBase();

        // StringBase methods (async version)
        Task<StringBase?> ReadLineAsStringBaseAsync();
        ValueTask<StringBase?> ReadLineAsStringBaseAsync(CancellationToken token);
        Task<StringBase> ReadToEndAsStringBaseAsync();
        ValueTask<StringBase> ReadToEndAsStringBaseAsync(CancellationToken token);
    }
}
