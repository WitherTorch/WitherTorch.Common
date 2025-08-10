#if NET472_OR_GREATER
using System.Numerics;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16StringHelper
    {
        private static unsafe partial bool VectorizedHasSurrogateCharacters(char* ptr, char* ptrEnd)
        {
            if (Limits.UseVector())
            {
                Vector<ushort>* ptrLimit = ((Vector<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector<ushort> surrogateStartVector = new Vector<ushort>(SurrogateStart);
                    Vector<ushort> surrogateEndVector = new Vector<ushort>(SurrogateEnd);
                    Vector<ushort> zeroVector = Vector<ushort>.Zero;
                    do
                    {
                        Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(ptr);
                        Vector<ushort> resultVector = Vector.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                }
            }

            return LegacyHasSurrogateCharacters(ptr, ptrEnd);
        }
    }
}
#endif