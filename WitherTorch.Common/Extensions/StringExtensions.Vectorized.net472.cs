#if NET472_OR_GREATER
using System.Numerics;

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

            if (Limits.UseVector())
            {
                Vector<ushort>* ptrLimit = ((Vector<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector<ushort> maskVectorLow = new Vector<ushort>(isUpper ? 'a' : 'A');
                    Vector<ushort> maskVectorHigh = new Vector<ushort>(isUpper ? 'z' : 'Z');
                    Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector<ushort> valueVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(ptr);
                        Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                UnsafeHelper.WriteUnaligned(ptrResult + (ptr - ptrStart), valueVector);
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
        private static unsafe Vector<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector<ushort> valueVector, in Vector<ushort> operationVector,
            in Vector<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
    }
}
#endif