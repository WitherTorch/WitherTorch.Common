#if NET8_0_OR_GREATER
namespace RiceTea.Core;

partial class RTCore
{
    public static partial bool SystemBuffersExists => true;

    public static partial bool SystemMemoryExists => true;
}
#endif