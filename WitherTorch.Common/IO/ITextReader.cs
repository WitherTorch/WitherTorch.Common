using System;
using System.Threading.Tasks;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    public partial interface ITextReader : IDisposable
    {
        // Normal methods
        int Peek();
        int Read();
        string? ReadLine();
        string ReadToEnd();

        // Normal methods (async version)
        Task<int> PeekAsync();
        Task<int> ReadAsync();
        Task<string?> ReadLineAsync();
        Task<string> ReadToEndAsync();

        // StringWrapper methods
        StringWrapper? ReadLineAsStringWrapper();
        StringWrapper ReadToEndAsStringWrapper();

        // StringWrapper methods (async version)
        Task<StringWrapper?> ReadLineAsStringWrapperAsync();
        Task<StringWrapper> ReadToEndAsStringWrapperAsync();
    }
}
