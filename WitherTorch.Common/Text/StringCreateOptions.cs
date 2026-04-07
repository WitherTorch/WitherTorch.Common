using System;

namespace WitherTorch.Common.Text
{
    [Flags]
    public enum StringCreateOptions : uint
    {
        None = 0x0000,
        UseAsciiCompression = 0b0001,
        UseLatin1Compression = 0b0010,
        UseUtf8Compression = 0b0100,
        UseUtf16Compression = 0b1000,

        ForceUseAscii = _Force_Flag | UseAsciiCompression,
        ForceUseLatin1 = _Force_Flag | UseLatin1Compression,
        ForceUseUtf8 = _Force_Flag | UseUtf8Compression,
        ForceUseUtf16 = _Force_Flag | UseUtf16Compression,

        _Force_Flag = unchecked((uint)int.MinValue),
    }
}
