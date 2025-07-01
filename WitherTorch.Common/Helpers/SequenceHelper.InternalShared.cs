using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        internal sealed unsafe class InternalShared
        {
#if NET6_0_OR_GREATER
            public const int VectorClassCount = 4;
#else
            public const int VectorClassCount = 1;
#endif

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void CalculateOperationCount<T>(nuint count, nuint* pResult) where T : unmanaged
            {
#if NET6_0_OR_GREATER
                if (Limits.UseVector512())
                    pResult[0] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector512<T>.Count);
                if (Limits.UseVector256())
                    pResult[1] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector256<T>.Count);
                if (Limits.UseVector128())
                    pResult[2] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector128<T>.Count);
                if (Limits.UseVector64())
                    pResult[3] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector64<T>.Count);
#else
                if (Limits.UseVector())
                    pResult[0] = CalculateOperationCountCore(ref count, System.Numerics.Vector<T>.Count);
#endif
                pResult[VectorClassCount] = count;
            }

            [Inline(InlineBehavior.Remove)]
            private static nuint CalculateOperationCountCore(ref nuint count, int batchSize)
            {
                if (batchSize <= 2)
                    return 0;
                return CalculateOperationCountCore(ref count, unchecked((nuint)batchSize));
            }        
            
            [Inline(InlineBehavior.Remove)]
            private static nuint CalculateOperationCountCore(ref nuint count, nuint batchSize)
            {
                nuint result = count / batchSize;
                count %= batchSize;
                return result;
            }
        }
    }
}
