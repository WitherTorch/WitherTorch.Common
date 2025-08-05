using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using InlineIL;

using WitherTorch.Common.Native;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.IO
{
    public unsafe sealed partial class CustomStreamReader : TextReader
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly byte[] _buffer;
        private readonly void* _readOneFunc, _readLineFunc, _readLineAsStringBaseFunc;
        private readonly bool _leaveOpen;

        private char[]? _charBuffer;
        private nuint _bufferPos, _bufferLength, _charBufferPos;
        private bool _eofReached, _disposed;

        public Stream BaseStream => _stream;
        public Encoding CurrentEncoding => _encoding;
        public bool EndOfStream
        {
            get
            {
                if (!_eofReached)
                    return false;

                if (_bufferPos < _bufferLength)
                    return false;

                if (_charBuffer is not null && _charBufferPos < unchecked((nuint)_charBuffer.Length))

                    ReadStream();

                if (_bufferPos < _bufferLength)
                    return false;

                return _eofReached;
            }
        }

        public CustomStreamReader(Stream stream, Encoding encoding) : this(stream, encoding, bufferSize: 1024, leaveOpen: false) { }

        public CustomStreamReader(Stream stream, Encoding encoding, int bufferSize) : this(stream, encoding, bufferSize, leaveOpen: false) { }

        public CustomStreamReader(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen)
        {
            _stream = stream;
            _buffer = new byte[bufferSize];
            _encoding = encoding;
            _leaveOpen = leaveOpen;
            _bufferLength = 0;
            _bufferPos = 0;
            _disposed = false;

            void* readOneFunc, readLineFunc, readLineAsStringBaseFunc;
            switch (encoding.CodePage)
            {
                case 1200: // UTF-16 code page (little-endian encoding)
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInUtf16Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInUtf16Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInUtf16Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 65001: // UTF-8 code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInUtf8Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInUtf8Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInUtf8Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 28591: // Latin-1 code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInLatin1Encoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInLatin1Encoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInLatin1Encoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                case 20127: // Ascii code page
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInAsciiEncoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInAsciiEncoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInAsciiEncoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
                default:
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadOneInOtherEncoding)));
                    IL.Pop(out readOneFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineInOtherEncoding)));
                    IL.Pop(out readLineFunc);
                    IL.Emit.Ldftn(MethodRef.Method(typeof(CustomStreamReader), nameof(ReadLineAsStringBaseInOtherEncoding)));
                    IL.Pop(out readLineAsStringBaseFunc);
                    break;
            }
            _readOneFunc = readOneFunc;
            _readLineFunc = readLineFunc;
            _readLineAsStringBaseFunc = readLineAsStringBaseFunc;
        }

        public override int Peek()
        {
            IL.Emit.Ldarg_0();
            IL.Push(false);
            IL.Push(_readOneFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(int), typeof(bool)));
            return IL.Return<int>();
        }

        public override int Read()
        {
            IL.Emit.Ldarg_0();
            IL.Push(true);
            IL.Push(_readOneFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(int), typeof(bool)));
            return IL.Return<int>();
        }

        public override string? ReadLine()
        {
            IL.Emit.Ldarg_0();
            IL.Push(_readLineFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(string)));
            return IL.Return<string>();
        }

        public StringBase? ReadLineAsStringBase()
        {
            IL.Emit.Ldarg_0();
            IL.Push(_readLineAsStringBaseFunc);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.HasThis, typeof(StringBase)));
            return IL.Return<StringBase>();
        }

        public Task<StringBase?> ReadLineAsStringBaseAsync()
            => Task.Factory.StartNew(ReadLineAsStringBase, TaskCreationOptions.LongRunning);

        private partial int ReadOneInUtf16Encoding(bool movePosition);
        private partial int ReadOneInUtf8Encoding(bool movePosition);
        private partial int ReadOneInLatin1Encoding(bool movePosition);
        private partial int ReadOneInAsciiEncoding(bool movePosition);
        private partial int ReadOneInOtherEncoding(bool movePosition);
        private partial string? ReadLineInUtf16Encoding();
        private partial string? ReadLineInUtf8Encoding();
        private partial string? ReadLineInLatin1Encoding();
        private partial string? ReadLineInAsciiEncoding();
        private partial string? ReadLineInOtherEncoding();
        private partial StringBase? ReadLineAsStringBaseInUtf16Encoding();
        private partial StringBase? ReadLineAsStringBaseInUtf8Encoding();
        private partial StringBase? ReadLineAsStringBaseInLatin1Encoding();
        private partial StringBase? ReadLineAsStringBaseInAsciiEncoding();
        private partial StringBase? ReadLineAsStringBaseInOtherEncoding();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_disposed)
                return;
            _disposed = true;

            if (!_leaveOpen)
                _stream.Dispose();
        }
    }
}
