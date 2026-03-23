using System;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
#pragma warning disable CS8500
#pragma warning disable CS0162
        unsafe partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static void Not(nint* ptr, nuint length)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Not((int*)ptr, length);
                        return;
                    case sizeof(long):
                        FastCore<long>.Not((long*)ptr, length);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Not((int*)ptr, length);
                                return;
                            case sizeof(long):
                                FastCore<long>.Not((long*)ptr, length);
                                return;
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static void Not(nuint* ptr, nuint length)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Not((uint*)ptr, length);
                        return;
                    case sizeof(long):
                        FastCore<ulong>.Not((ulong*)ptr, length);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Not((uint*)ptr, length);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Not((ulong*)ptr, length);
                                return;
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
#pragma warning restore CS0162

        unsafe partial class FastCore<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Identity(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, ref length, UnaryOperatorType.Identity);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Not(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, ref length, UnaryOperatorType.Not);

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedIdentity(T* ptr, nuint length)
                => VectorizedUnaryOperationCore(ref ptr, ref length, UnaryOperatorType.Identity);

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedNot(T* ptr, nuint length)
                => VectorizedUnaryOperationCore(ref ptr, ref length, UnaryOperatorType.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                if (method == UnaryOperatorType.Identity)
                    return;
                if (Limits.CheckTypeCanBeVectorized<T>() && length > Limits.GetLimitForVectorizing<T>())
                {
                    switch (method)
                    {
                        case UnaryOperatorType.Not:
                            VectorizedNot(ptr, length);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(method));
                    }
                    return;
                }
                ScalarizedUnaryOperationCore(ref ptr, ref length, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method);

            [Inline(InlineBehavior.Remove)]
            private static void ScalarizedUnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType method)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = ScalarizedUnaryOperation(ptr[0], method);
                    ptr[1] = ScalarizedUnaryOperation(ptr[1], method);
                    ptr[2] = ScalarizedUnaryOperation(ptr[2], method);
                    ptr[3] = ScalarizedUnaryOperation(ptr[3], method);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedUnaryOperation(*ptr, method);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedUnaryOperation(*ptr, method);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedUnaryOperation(*ptr, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static T ScalarizedUnaryOperation(T item, [InlineParameter] UnaryOperatorType method)
                => method switch
                {
                    UnaryOperatorType.Not => UnsafeHelper.Not(item),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        unsafe partial class SlowCore<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Not(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, ref length, UnaryOperatorType.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperatorType operatorType)
                => UnaryOperationCore(ptr, length, operatorType switch
                {
                    UnaryOperatorType.Not => UnaryOperator<T>.Not,
                    _ => throw new ArgumentOutOfRangeException(nameof(operatorType))
                });

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnaryOperationCore(T* ptr, nuint length, IUnaryOperator<T> @operator)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = @operator.Operate(ptr[0]);
                    ptr[1] = @operator.Operate(ptr[1]);
                    ptr[2] = @operator.Operate(ptr[2]);
                    ptr[3] = @operator.Operate(ptr[3]);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void UnaryOperationCore(T* ptr, nuint length, UnaryOperation<T> operation)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = operation.Invoke(ptr[0]);
                    ptr[1] = operation.Invoke(ptr[1]);
                    ptr[2] = operation.Invoke(ptr[2]);
                    ptr[3] = operation.Invoke(ptr[3]);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr);
            }
        }
    }
}
