namespace WitherTorch.Common.Text
{
    partial class AsciiEncodingHelper
    {
        internal static unsafe partial byte* ReadFromUtf16BufferCore_OutOfAsciiRange(char* source, byte* destination, nuint length);

        internal static unsafe partial byte* ReadFromUtf16BufferCore(char* source, byte* destination, nuint length);

        internal static unsafe partial char* WriteToUtf16BufferCore(byte* source, char* destination, nuint length);
    }
}
