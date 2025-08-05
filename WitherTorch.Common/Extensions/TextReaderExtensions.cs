using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using WitherTorch.Common.IO;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class TextReaderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase? ReadLineAsStringBase(this TextReader _this)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadLineAsStringBase();
            string? result = _this.ReadLine();
            return result is null ? null : StringBase.Create(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<StringBase?> ReadLineAsStringBaseAsync(this TextReader _this)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadLineAsStringBaseAsync();
            return ReadLineAsStringBaseAsyncFallback(_this);
        }

        private static async Task<StringBase?> ReadLineAsStringBaseAsyncFallback(this TextReader _this)
        {
            string? result = await _this.ReadLineAsync();
            return result is null ? null : StringBase.Create(result);
        }
    }
}
