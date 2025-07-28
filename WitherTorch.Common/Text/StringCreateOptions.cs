using System;

namespace WitherTorch.Common.Text
{
    [Flags]
    public enum StringCreateOptions : uint
    {
        None = 0,
        UseLatin1Compression = 0b01,
        UseUtf8Compression = 0b10,
        ForceUseUtf8 = _ForceUseUtf8_Flag | UseUtf8Compression,


        _ForceUseUtf8_Flag = 0b100,
    }
}
