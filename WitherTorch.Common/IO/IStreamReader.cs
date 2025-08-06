using System.IO;
using System.Text;

namespace WitherTorch.Common.IO
{
    public interface IStreamReader : ITextReader
    {
        Stream BaseStream { get; }
        Encoding CurrentEncoding { get; }
        bool EndOfStream { get; }
    }
}
