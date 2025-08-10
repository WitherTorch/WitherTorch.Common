#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;

namespace WitherTorch.Common.Text
{
    partial class Utf16StringHelper
    {
        private static unsafe partial bool VectorizedHasSurrogateCharacters(char* ptr, char* ptrEnd)
        {
            if (Limits.UseVector512())
            {
                Vector512<ushort>* ptrLimit = ((Vector512<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector512<ushort> surrogateStartVector = Vector512.Create<ushort>(SurrogateStart);
                    Vector512<ushort> surrogateEndVector = Vector512.Create<ushort>(SurrogateEnd);
                    Vector512<ushort> zeroVector = Vector512<ushort>.Zero;
                    do
                    {
                        Vector512<ushort> sourceVector = Vector512.Load((ushort*)ptr);
                        Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector512.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* ptrLimit = ((Vector256<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector256<ushort> surrogateStartVector = Vector256.Create<ushort>(SurrogateStart);
                    Vector256<ushort> surrogateEndVector = Vector256.Create<ushort>(SurrogateEnd);
                    Vector256<ushort> zeroVector = Vector256<ushort>.Zero;
                    do
                    {
                        Vector256<ushort> sourceVector = Vector256.Load((ushort*)ptr);
                        Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector256.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* ptrLimit = ((Vector128<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector128<ushort> surrogateStartVector = Vector128.Create<ushort>(SurrogateStart);
                    Vector128<ushort> surrogateEndVector = Vector128.Create<ushort>(SurrogateEnd);
                    Vector128<ushort> zeroVector = Vector128<ushort>.Zero;
                    do
                    {
                        Vector128<ushort> sourceVector = Vector128.Load((ushort*)ptr);
                        Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector128.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* ptrLimit = ((Vector64<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector64<ushort> surrogateStartVector = Vector64.Create<ushort>(SurrogateStart);
                    Vector64<ushort> surrogateEndVector = Vector64.Create<ushort>(SurrogateEnd);
                    Vector64<ushort> zeroVector = Vector64<ushort>.Zero;
                    do
                    {
                        Vector64<ushort> sourceVector = Vector64.Load((ushort*)ptr);
                        Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector64.LessThanOrEqual(sourceVector, surrogateEndVector);
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