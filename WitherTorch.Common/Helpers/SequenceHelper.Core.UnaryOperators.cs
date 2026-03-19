using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum UnaryOperationMethod
        {
            Not,
            _Last
        }

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
            public static void Not(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, ref length, UnaryOperationMethod.Not);

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedNot(T* ptr, nuint length)
                => VectorizedUnaryOperationCore(ref ptr, ref length, UnaryOperationMethod.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperationMethod method)
            {
                if (CheckTypeCanBeVectorized() && length >= GetMinimumVectorCount())
                {
                    switch (method)
                    {
                        case UnaryOperationMethod.Not:
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
            private static partial void VectorizedUnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperationMethod method);

            [Inline(InlineBehavior.Remove)]
            private static void ScalarizedUnaryOperationCore(ref T* ptr, ref nuint length, [InlineParameter] UnaryOperationMethod method)
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
            private static T ScalarizedUnaryOperation(T item, [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => UnsafeHelper.Not(item),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        partial class SlowCore
        {
            internal static readonly string[] UnaryOperatorNames = new string[(int)UnaryOperationMethod._Last]
            {
                "op_OnesComplement",
            };
        }

        unsafe partial class SlowCore<T>
        {
            private static readonly LazyTiny<nint[]> _unaryOperatorsLazy = new LazyTiny<nint[]>(InitializeUnaryOperators);

            private static nint[] InitializeUnaryOperators()
            {
                nint[] result = new nint[(int)UnaryOperationMethod._Last];
                for (int i = 0; i < (int)UnaryOperationMethod._Last; i++)
                    result[i] = ReflectionHelper.GetMethodPointer(typeof(T), SlowCore.UnaryOperatorNames[i], [typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static);
                return result;
            }

            public static void Not(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, length, UnaryOperationMethod.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, nuint length, [InlineParameter] UnaryOperationMethod method)
            {
                nint functionPointer = _unaryOperatorsLazy.Value[(int)method];
                if (functionPointer == default)
                    throw new InvalidOperationException($"Cannot find the {method} operator for {typeof(T).Name}!");
                for (nuint i = 0; i < length; i++, ptr++)
                    *ptr = LegacyUnaryOperationCoreSlow(*ptr, functionPointer);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T LegacyUnaryOperationCoreSlow(T item, nint functionPointer)
                => ((delegate* managed<T, T>)functionPointer)(item);
        }
    }
}
