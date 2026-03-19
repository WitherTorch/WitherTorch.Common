using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum CompareMethod
        {
            Include,
            Exclude,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
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
            public static bool ContainsGreaterThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
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
            public static bool ContainsLessThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, *(long*)&value);
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
            public static bool ContainsGreaterThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
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
            public static bool ContainsLessThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
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
            public static nint* PointerIndexOfGreaterThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
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
            public static nint* PointerIndexOfLessThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
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
            public static nuint* PointerIndexOfGreaterThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
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
            public static nuint* PointerIndexOfLessThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static void Replace(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Replace((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.Replace((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Replace((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.Replace((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceExclude(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceExclude((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceExclude((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceExclude((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceExclude((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceGreaterThan(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceGreaterThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceGreaterThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceGreaterThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceGreaterThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceGreaterThanOrEquals(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceGreaterThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceGreaterThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceGreaterThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceGreaterThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceLessThan(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceLessThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceLessThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceLessThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceLessThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceLessThanOrEquals(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceLessThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceLessThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceLessThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceLessThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void Replace(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Replace((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Replace((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Replace((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Replace((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceExclude(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceExclude((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceExclude((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceGreaterThan(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceGreaterThanOrEquals(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceGreaterThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceGreaterThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceLessThan(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceLessThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceLessThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceLessThanOrEquals(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceLessThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceLessThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceLessThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceLessThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static bool Contains(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Replace(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.Include);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.Exclude);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.GreaterThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.LessThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.GreaterThanOrEquals);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.LessThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContains(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOf(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplace(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.Include);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.Exclude);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.GreaterThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.LessThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.GreaterThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => VectorizedReplaceCore(ref ptr, ref length, filter, replacement, CompareMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                if (CheckTypeCanBeVectorized() && length >= GetMinimumVectorCount())
                    return method switch
                    {
                        CompareMethod.Include => accurateResult ? VectorizedPointerIndexOf(ptr, length, value) : VectorizedContains(ptr, length, value),
                        CompareMethod.Exclude => accurateResult ? VectorizedPointerIndexOfExclude(ptr, length, value) : VectorizedContainsExclude(ptr, length, value),
                        CompareMethod.GreaterThan => accurateResult ? VectorizedPointerIndexOfGreaterThan(ptr, length, value) : VectorizedContainsGreaterThan(ptr, length, value),
                        CompareMethod.GreaterThanOrEquals => accurateResult ? VectorizedPointerIndexOfGreaterThanOrEquals(ptr, length, value) : VectorizedContainsGreaterThanOrEquals(ptr, length, value),
                        CompareMethod.LessThan => accurateResult ? VectorizedPointerIndexOfLessThan(ptr, length, value) : VectorizedContainsLessThan(ptr, length, value),
                        CompareMethod.LessThanOrEquals => accurateResult ? VectorizedPointerIndexOfLessThanOrEquals(ptr, length, value) : VectorizedContainsLessThanOrEquals(ptr, length, value),
                        _ => throw new ArgumentOutOfRangeException(nameof(method))
                    };

                return ScalarizedPointerIndexOfCore(ref ptr, ref length, value, method, accurateResult);
            }

            [Inline(InlineBehavior.Remove)]
            private static void ReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                if (CheckTypeCanBeVectorized() && length >= GetMinimumVectorCount())
                {
                    switch (method)
                    {
                        case CompareMethod.Include:
                            VectorizedReplace(ptr, length, filter, replacement);
                            break;
                        case CompareMethod.Exclude:
                            VectorizedReplaceExclude(ptr, length, filter, replacement);
                            break;
                        case CompareMethod.GreaterThan:
                            VectorizedReplaceGreaterThan(ptr, length, filter, replacement);
                            break;
                        case CompareMethod.GreaterThanOrEquals:
                            VectorizedReplaceGreaterThanOrEquals(ptr, length, filter, replacement);
                            break;
                        case CompareMethod.LessThan:
                            VectorizedReplaceLessThan(ptr, length, filter, replacement);
                            break;
                        case CompareMethod.LessThanOrEquals:
                            VectorizedReplaceLessThanOrEquals(ptr, length, filter, replacement);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(method));
                    }
                }

                ScalarizedReplaceCore(ref ptr, ref length, filter, replacement, method);
            }

            [LocalsInit(false)]
            [Inline(InlineBehavior.Remove)]
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult);

            [Inline(InlineBehavior.Remove)]
            private static T* ScalarizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    if (ScalarizedCompare(ptr[0], value, method))
                        return accurateResult ? ptr : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[1], value, method))
                        return accurateResult ? ptr + 1 : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[2], value, method))
                        return accurateResult ? ptr + 2 : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[3], value, method))
                        return accurateResult ? ptr + 3 : (T*)Booleans.TrueNative;
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;

            NotFound:
                return null;
            }


            [Inline(InlineBehavior.Remove)]
            private static partial void VectorizedReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method);

            [Inline(InlineBehavior.Remove)]
            private static void ScalarizedReplaceCore(ref T* ptr, ref nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    if (ScalarizedCompare(ptr[0], filter, method))
                        ptr[0] = replacement;
                    if (ScalarizedCompare(ptr[1], filter, method))
                        ptr[1] = replacement;
                    if (ScalarizedCompare(ptr[2], filter, method))
                        ptr[2] = replacement;
                    if (ScalarizedCompare(ptr[3], filter, method))
                        ptr[3] = replacement;
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                if (ScalarizedCompare(*ptr, filter, method))
                    *ptr = replacement;
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                if (ScalarizedCompare(*ptr, filter, method))
                    *ptr = replacement;
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                if (ScalarizedCompare(*ptr, filter, method))
                    *ptr = replacement;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool ScalarizedCompare(T item, T value, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => UnsafeHelper.Equals(item, value),
                    CompareMethod.Exclude => UnsafeHelper.NotEquals(item, value),
                    CompareMethod.GreaterThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanUnsigned(item, value) : UnsafeHelper.IsGreaterThan(item, value),
                    CompareMethod.GreaterThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsGreaterThanOrEquals(item, value),
                    CompareMethod.LessThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanUnsigned(item, value) : UnsafeHelper.IsLessThan(item, value),
                    CompareMethod.LessThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsLessThanOrEquals(item, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        unsafe partial class SlowCore<T>
        {
            public static bool Contains(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Include) != null;

            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Exclude) != null;

            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThan) != null;

            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThan) != null;

            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThanOrEquals) != null;

            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThanOrEquals) != null;

            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Include);

            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Exclude);

            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThan);

            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThan);

            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThanOrEquals);

            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThanOrEquals);

            public static void Replace(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.Include);

            public static void ReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.Exclude);

            public static void ReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.GreaterThan);

            public static void ReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.LessThan);

            public static void ReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.GreaterThanOrEquals);

            public static void ReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, CompareMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] CompareMethod method)
            {
                if (method == CompareMethod.Include || method == CompareMethod.Exclude)
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
            private static void ReplaceCore(ref T* ptr, nuint length, T filter, T replacement, [InlineParameter] CompareMethod method)
            {
                if (method == CompareMethod.Include || method == CompareMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, filter, method))
                            *ptr = replacement;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, filter, method))
                            *ptr = replacement;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(EqualityComparer<T> comparer, T item, T value, [InlineParameter] CompareMethod method)
                => method switch
                {
                    CompareMethod.Include => comparer.Equals(item, value),
                    CompareMethod.Exclude => !comparer.Equals(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(Comparer<T> comparer, T item, T value, [InlineParameter] CompareMethod method)
            {
                int result = comparer.Compare(item, value);
                return method switch
                {
                    CompareMethod.GreaterThan => result > 0,
                    CompareMethod.GreaterThanOrEquals => result >= 0,
                    CompareMethod.LessThan => result < 0,
                    CompareMethod.LessThanOrEquals => result <= 0,
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
