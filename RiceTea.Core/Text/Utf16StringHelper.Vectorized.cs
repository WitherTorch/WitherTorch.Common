namespace RiceTea.Core.Text;

partial class Utf16StringHelper
{
    private static unsafe partial bool VectorizedHasSurrogateCharacters(char* ptr, char* ptrEnd);
}
