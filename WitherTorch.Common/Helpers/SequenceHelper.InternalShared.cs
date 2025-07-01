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

#if NET6_0_OR_GREATER
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void CalculateOperationCount<T>(nuint count, nuint* pResult) where T : unmanaged
            {
                if (Limits.UseVector512())
                    pResult[0] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector512<T>.Count);
                if (Limits.UseVector256())
                    pResult[1] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector256<T>.Count);
                if (Limits.UseVector128())
                    pResult[2] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector128<T>.Count);
                if (Limits.UseVector64())
                    pResult[3] = CalculateOperationCountCore(ref count, System.Runtime.Intrinsics.Vector64<T>.Count);
                pResult[4] = count;
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void CalculateOperationCount<T>(nuint count, nuint* pResult) where T : unmanaged
            {
                if (Limits.UseVector())
                    pResult[0] = CalculateOperationCountCore(ref count, System.Numerics.Vector<T>.Count);
                pResult[1] = count;
            }
#endif

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static nuint CalculateOperationCountCore(ref nuint count, int batchSize)
            {
                if (batchSize > 2)
                    return MathHelper.DivRem(count, unchecked((nuint)batchSize), out count);
                return 0;
            }
        }
    }
}
