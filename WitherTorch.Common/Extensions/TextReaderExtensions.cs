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
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
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
            return result is null ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase ReadToEndAsStringBase(this TextReader _this)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadToEndAsStringBase();
            return StringBase.Create(_this.ReadToEnd(), StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<StringBase> ReadToEndAsStringBaseAsync(this TextReader _this)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadToEndAsStringBaseAsync();
            return ReadToEndAsStringBaseAsyncFallback(_this);
        }

        private static async Task<StringBase> ReadToEndAsStringBaseAsyncFallback(this TextReader _this) 
            => StringBase.Create(await _this.ReadToEndAsync(), StringCreateOptions.None);

#if NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<StringBase?> ReadLineAsStringBaseAsync(this TextReader _this, System.Threading.CancellationToken token)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadLineAsStringBaseAsync(token);
            return ReadLineAsStringBaseAsyncFallback(_this, token);
        }

        private static async Task<StringBase?> ReadLineAsStringBaseAsyncFallback(this TextReader _this, System.Threading.CancellationToken token)
        {
            string? result = await _this.ReadLineAsync(token);
            return (result is null || token.IsCancellationRequested) ? null : StringBase.Create(result, StringCreateOptions.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<StringBase> ReadToEndAsStringBaseAsync(this TextReader _this, System.Threading.CancellationToken token)
        {
            if (_this is CustomStreamReader reader)
                return reader.ReadToEndAsStringBaseAsync(token);
            return ReadToEndAsStringBaseAsyncFallback(_this, token);
        }

        private static async Task<StringBase> ReadToEndAsStringBaseAsyncFallback(this TextReader _this, System.Threading.CancellationToken token)
            => StringBase.Create(await _this.ReadToEndAsync(token), StringCreateOptions.None);
#endif
    }
}
