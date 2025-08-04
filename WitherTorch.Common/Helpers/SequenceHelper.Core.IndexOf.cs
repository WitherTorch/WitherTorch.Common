using System;
using System.Collections.Generic;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum IndexOfMethod
        {
            Include,
            Exclude,
            GreaterThan,
            GreaterOrEqualsThan,
            LessThan,
            LessOrEqualsThan,
            _Last
        }

#pragma warning disable CS0162
#pragma warning disable CS8500
        unsafe partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static bool Contains(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.Contains((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.Contains((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.Contains((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.Contains((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsExclude((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsExclude((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsExclude((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsExclude((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsGreaterThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsGreaterThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsGreaterThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsGreaterThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsGreaterOrEqualsThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsGreaterOrEqualsThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsGreaterOrEqualsThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsGreaterOrEqualsThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsLessThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsLessThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsLessThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsLessThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsLessOrEqualsThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsLessOrEqualsThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsLessOrEqualsThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsLessOrEqualsThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool Contains(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.Contains((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.Contains((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.Contains((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.Contains((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsExclude((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsExclude((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsLessThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsLessThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsLessOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsLessOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOf(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOf((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOf((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOf((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOf((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfExclude(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterOrEqualsThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessOrEqualsThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOf(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfExclude(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterOrEqualsThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessOrEqualsThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, length, *(ulong*)&value);
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
            public static bool Contains(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include, accurateResult: false), usePreciseBooleanDefination: true);

            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude, accurateResult: false), usePreciseBooleanDefination: true);

            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan, accurateResult: false), usePreciseBooleanDefination: true);

            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan, accurateResult: false), usePreciseBooleanDefination: true);

            public static bool ContainsGreaterOrEqualsThan(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: false), usePreciseBooleanDefination: true);

            public static bool ContainsLessOrEqualsThan(T* ptr, nuint length, T value)
                => MathHelper.ToBoolean(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessOrEqualsThan, accurateResult: false), usePreciseBooleanDefination: true);

            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include, accurateResult: true);

            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude, accurateResult: true);

            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan, accurateResult: true);

            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan, accurateResult: true);

            public static T* PointerIndexOfGreaterOrEqualsThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: true);

            public static T* PointerIndexOfLessOrEqualsThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessOrEqualsThan, accurateResult: true);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                T* ptrEnd = ptr + length;
                if (CheckTypeCanBeVectorized())
                    return VectorizedPointerIndexOfCore(ref ptr, ptrEnd, value, method, accurateResult);

                return LegacyPointerIndexOfCore(ref ptr, ptrEnd, value, method, accurateResult);
            }

            [Inline(InlineBehavior.Remove)]
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult);

            [Inline(InlineBehavior.Remove)]
            private static T* LegacyPointerIndexOfCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                for (; ptr < ptrEnd; ptr++)
                {
                    if (LegacyIndexOfCore(*ptr, value, method))
                        return accurateResult ? ptr : (T*)Booleans.TrueNative;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCore(T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => UnsafeHelper.Equals(item, value),
                    IndexOfMethod.Exclude => UnsafeHelper.NotEquals(item, value),
                    IndexOfMethod.GreaterThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanUnsigned(item, value) : UnsafeHelper.IsGreaterThan(item, value),
                    IndexOfMethod.GreaterOrEqualsThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterOrEqualsThanUnsigned(item, value) : UnsafeHelper.IsGreaterOrEqualsThan(item, value),
                    IndexOfMethod.LessThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanUnsigned(item, value) : UnsafeHelper.IsLessThan(item, value),
                    IndexOfMethod.LessOrEqualsThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessOrEqualsThanUnsigned(item, value) : UnsafeHelper.IsLessOrEqualsThan(item, value),
                    _ => throw new InvalidOperationException(),
                };
        }

        unsafe partial class SlowCore<T>
        {
            public static bool Contains(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.Include) != null;

            public static bool ContainsExclude(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.Exclude) != null;

            public static bool ContainsGreaterThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.GreaterThan) != null;

            public static bool ContainsLessThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.LessThan) != null;

            public static bool ContainsGreaterOrEqualsThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.GreaterOrEqualsThan) != null;

            public static bool ContainsLessOrEqualsThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.LessOrEqualsThan) != null;

            public static T* PointerIndexOf(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.Include);

            public static T* PointerIndexOfExclude(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.Exclude);

            public static T* PointerIndexOfGreaterThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.GreaterThan);

            public static T* PointerIndexOfLessThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.LessThan);

            public static T* PointerIndexOfGreaterOrEqualsThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.GreaterOrEqualsThan);

            public static T* PointerIndexOfLessOrEqualsThan(T* ptr, T value, nuint length)
                => PointerIndexOfCore(ref ptr, value, length, IndexOfMethod.LessOrEqualsThan);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, T value, nuint length, [InlineParameter] IndexOfMethod method)
            {
                if (method == IndexOfMethod.Include || method == IndexOfMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(EqualityComparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => comparer.Equals(item, value),
                    IndexOfMethod.Exclude => !comparer.Equals(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(Comparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
            {
                int result = comparer.Compare(item, value);
                return method switch
                {
                    IndexOfMethod.GreaterThan => result > 0,
                    IndexOfMethod.GreaterOrEqualsThan => result >= 0,
                    IndexOfMethod.LessThan => result < 0,
                    IndexOfMethod.LessOrEqualsThan => result <= 0,
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
