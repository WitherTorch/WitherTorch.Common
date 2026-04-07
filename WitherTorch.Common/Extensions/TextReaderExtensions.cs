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
        public static StringWrapper? ReadLineAsStringWrapper(this TextReader _this)
        {
            string? result = _this.ReadLine();
            return result is null ? null : StringWrapper.CreateUtf16String(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringWrapper?> ReadLineAsStringWrapperAsync(this TextReader _this)
        {
            string? result = await _this.ReadLineAsync();
            return result is null ? null : StringWrapper.CreateUtf16String(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper ReadToEndAsStringWrapper(this TextReader _this)
            => StringWrapper.CreateUtf16String(_this.ReadToEnd());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringWrapper> ReadToEndAsStringWrapperAsync(this TextReader _this) 
            => StringWrapper.CreateUtf16String(await _this.ReadToEndAsync());

#if NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringWrapper?> ReadLineAsStringWrapperAsync(this TextReader _this, System.Threading.CancellationToken token)
        {
            string? result = await _this.ReadLineAsync(token);
            return (result is null || token.IsCancellationRequested) ? null : StringWrapper.CreateUtf16String(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringWrapper> ReadToEndAsStringWrapperAsync(this TextReader _this, System.Threading.CancellationToken token)
            => StringWrapper.CreateUtf16String(await _this.ReadToEndAsync(token));
#endif
    }
}
