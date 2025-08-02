using System;

namespace WitherTorch.Common.Text
{
    [Flags]
    public enum StringCreateOptions : uint
    {
        None = 0x0000,
        UseLatin1Compression = 0b0001,
        ForceUseLatin1 = _ForceUseLatin1_Flag | UseLatin1Compression,
        UseUtf8Compression = 0b0100,
        ForceUseUtf8 = _ForceUseUtf8_Flag | UseUtf8Compression,

        _ForceUseLatin1_Flag = 0b0010,
        _ForceUseUtf8_Flag = 0b1000,
    }
}
