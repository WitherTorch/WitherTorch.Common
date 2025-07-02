using InlineMethod;

using System;
using System.Runtime.CompilerServices;
using System.Reflection;

using WitherTorch.Common.Threading;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

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
            public static void Not(T* ptr, nuint length)
                => UnaryOperationCore(ref ptr, length, UnaryOperationMethod.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, nuint length, [InlineParameter] UnaryOperationMethod method)
            {
                if (CheckTypeCanBeVectorized())
                {
                    nuint* vectorOperationCounts = stackalloc nuint[InternalShared.VectorClassCount + 1];
                    InternalShared.CalculateOperationCount<T>(length, vectorOperationCounts);
                    VectorizedUnaryOperationCore(ref ptr, vectorOperationCounts, method);
                    return;
                }
                LegacyUnaryOperationCore(ref ptr, length, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore(ref T* ptr, nuint* vectorOperationCounts, [InlineParameter] UnaryOperationMethod method)
            {
                nuint operationCount;
#if NET6_0_OR_GREATER
                if (Limits.UseVector512() && (operationCount = vectorOperationCounts[0]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector512<T> valueVector = Vector512.Load(ptr);
                        VectorizedUnaryOperationCore_512(valueVector, method).Store(ptr);
                        ptr += Vector512<T>.Count;
                    } while (++i < operationCount);
                }
                if (Limits.UseVector256() && (operationCount = vectorOperationCounts[1]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector256<T> valueVector = Vector256.Load(ptr);
                        VectorizedUnaryOperationCore_256(valueVector, method).Store(ptr);
                        ptr += Vector256<T>.Count;
                    } while (++i < operationCount);
                }
                if (Limits.UseVector128() && (operationCount = vectorOperationCounts[2]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector128<T> valueVector = Vector128.Load(ptr);
                        VectorizedUnaryOperationCore_128(valueVector, method).Store(ptr);
                        ptr += Vector128<T>.Count;
                    } while (++i < operationCount) ;
                }
                if (Limits.UseVector64() && (operationCount = vectorOperationCounts[3]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector64<T> valueVector = Vector64.Load(ptr);
                        VectorizedUnaryOperationCore_64(valueVector, method).Store(ptr);
                        ptr += Vector64<T>.Count;
                    } while (++i < operationCount);
                }
#else
                if (Limits.UseVector() && (operationCount = vectorOperationCounts[0]) > 0)
                {
                    nuint i = 0;
                    do
                    {
                        Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                        UnsafeHelper.Write(ptr, VectorizedUnaryOperationCore(valueVector, method));
                        ptr += Vector<T>.Count;
                    } while (++i < operationCount);
                }
#endif
                operationCount = vectorOperationCounts[InternalShared.VectorClassCount];
                for (nuint i = 0; i < operationCount; i++, ptr++)
                    *ptr = LegacyUnaryOperationCore(*ptr, method);
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedUnaryOperationCore_512(in Vector512<T> valueVector,
            [InlineParameter] UnaryOperationMethod method)
            => method switch
            {
                UnaryOperationMethod.Not => ~valueVector,
                _ => throw new InvalidOperationException(),
            };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedUnaryOperationCore_256(in Vector256<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedUnaryOperationCore_128(in Vector128<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedUnaryOperationCore_64(in Vector64<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };
#else

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedUnaryOperationCore(in Vector<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };
#endif

            [Inline(InlineBehavior.Remove)]
            private static void LegacyUnaryOperationCore(ref T* ptr, nuint length, [InlineParameter] UnaryOperationMethod method)
            {
                for (nuint i = 0; i < length; i++, ptr++)
                    *ptr = LegacyUnaryOperationCore(*ptr, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static T LegacyUnaryOperationCore(T item, [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => UnsafeHelper.Not(item),
                    _ => throw new InvalidOperationException(),
                };
        }

        unsafe partial class SlowCore
        {
            internal static readonly string[] UnaryOperatorNames = new string[(int)UnaryOperationMethod._Last]
            {
                "op_OnesComplement",
            };
        }

        unsafe partial class SlowCore<T>
        {
            private static readonly LazyTinyStruct<nint[]> _unaryOperatorsLazy = new LazyTinyStruct<nint[]>(InitializeUnaryOperators);

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
