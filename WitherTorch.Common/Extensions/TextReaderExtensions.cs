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
            string? result = _this.ReadLine();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringBase?> ReadLineAsStringBaseAsync(this TextReader _this)
        {
            string? result = await _this.ReadLineAsync();
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase ReadToEndAsStringBase(this TextReader _this)
            => StringBase.Create(_this.ReadToEnd(), StringCreateOptions.None);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringBase> ReadToEndAsStringBaseAsync(this TextReader _this) 
            => StringBase.Create(await _this.ReadToEndAsync(), StringCreateOptions.None);

#if NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringBase?> ReadLineAsStringBaseAsync(this TextReader _this, System.Threading.CancellationToken token)
        {
            string? result = await _this.ReadLineAsync(token);
            return (result is null || token.IsCancellationRequested) ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<StringBase> ReadToEndAsStringBaseAsync(this TextReader _this, System.Threading.CancellationToken token)
            => StringBase.Create(await _this.ReadToEndAsync(token), StringCreateOptions.None);
#endif
    }
}
