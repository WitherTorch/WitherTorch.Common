#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Extensions
{
    public static partial class StringExtensions
    {
        private static unsafe partial string? ToLowerOrUpperAsciiCore(char* ptr, char* ptrEnd, bool isUpper)
        {
            char* ptrStart = ptr;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                int resultLength = unchecked((int)(ptrEnd - ptrStart));
                string result = StringHelper.AllocateRawString(resultLength);
                fixed (char* ptrResult = result)
                    UnsafeHelper.CopyBlock(ptrResult, ptrStart, unchecked((uint)(resultLength * sizeof(char))));
                return result;
            });

            if (Limits.UseVector512())
            {
                Vector512<ushort>* ptrLimit = ((Vector512<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector512<ushort> maskVectorLow = Vector512.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector512<ushort> maskVectorHigh = Vector512.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector512<ushort> operationVector = Vector512.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector512<ushort> valueVector = Vector512.Load((ushort*)ptr);
                        Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector512.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* ptrLimit = ((Vector256<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector256<ushort> maskVectorLow = Vector256.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector256<ushort> maskVectorHigh = Vector256.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector256<ushort> operationVector = Vector256.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector256<ushort> valueVector = Vector256.Load((ushort*)ptr);
                        Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector256.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* ptrLimit = ((Vector128<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector128<ushort> maskVectorLow = Vector128.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector128<ushort> maskVectorHigh = Vector128.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector128<ushort> operationVector = Vector128.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector128<ushort> valueVector = Vector128.Load((ushort*)ptr);
                        Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector128.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* ptrLimit = ((Vector64<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector64<ushort> maskVectorLow = Vector64.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector64<ushort> valueVector = Vector64.Load((ushort*)ptr);
                        Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            for (; ptr < ptrEnd; ptr++)
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
            return resultLazy.GetValueDirectly();
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector512<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector512<ushort> valueVector, in Vector512<ushort> operationVector,
            in Vector512<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector256<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector256<ushort> valueVector, in Vector256<ushort> operationVector,
            in Vector256<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector128<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector128<ushort> valueVector, in Vector128<ushort> operationVector,
            in Vector128<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector64<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector64<ushort> valueVector, in Vector64<ushort> operationVector,
            in Vector64<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
    }
}
#endif