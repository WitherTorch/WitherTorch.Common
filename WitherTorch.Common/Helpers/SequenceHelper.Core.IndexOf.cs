using System;
using System.Collections.Generic;

using InlineMethod;
using System.Runtime.CompilerServices;


#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

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
        unsafe partial class Core
        {
            [Inline(InlineBehavior.Remove)]
            public static bool Contains(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.Contains((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.Contains((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.Contains((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.Contains((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool Contains(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.Contains((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.Contains((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.Contains((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.Contains((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOf(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOf((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOf((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOf((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOf((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfExclude(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOfExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOfExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOfExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOfExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOfGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOfGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOfGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOfGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterOrEqualsThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOfLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOfLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOfLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOfLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessOrEqualsThan(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)Core<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (nint*)Core<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)Core<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (nint*)Core<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOf(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOf((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOf((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOf((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOf((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfExclude(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOfExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOfExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOfExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOfExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOfGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOfGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterOrEqualsThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOfLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOfLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOfLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOfLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessOrEqualsThan(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)Core<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)Core<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)Core<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)Core<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
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

        unsafe partial class Core<T>
        {
            public static bool Contains(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.Include, accurateResult: false) != null;

            public static bool ContainsExclude(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.Exclude, accurateResult: false) != null;

            public static bool ContainsGreaterThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.GreaterThan, accurateResult: false) != null;

            public static bool ContainsLessThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.LessThan, accurateResult: false) != null;

            public static bool ContainsGreaterOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: false) != null;

            public static bool ContainsLessOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.LessOrEqualsThan, accurateResult: false) != null;

            public static T* PointerIndexOf(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.Include, accurateResult: true);

            public static T* PointerIndexOfExclude(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.Exclude, accurateResult: true);

            public static T* PointerIndexOfGreaterThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.GreaterThan, accurateResult: true);

            public static T* PointerIndexOfLessThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.LessThan, accurateResult: true);

            public static T* PointerIndexOfGreaterOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: true);

            public static T* PointerIndexOfLessOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ref ptr, ptrEnd, value, IndexOfMethod.LessOrEqualsThan, accurateResult: true);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                // Vector.IsHardwareAccelerated 與 Vector<T>.Count 會在執行時期被優化成常數，故不需要變數快取 (反而會妨礙 JIT 進行迴圈及條件展開)
                if (CheckTypeCanBeVectorized())
                    return VectorizedPointerIndexOfCore(ref ptr, ptrEnd, value, method, accurateResult);
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    for (; ptr < ptrEnd; ptr++)
                    {
                        if (LegacyIndexOfCoreFast(*ptr, value, method))
                            return ptr;
                    }
                    return null;
                }
                if (method == IndexOfMethod.Include || method == IndexOfMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (; ptr < ptrEnd; ptr++)
                    {
                        if (LegacyIndexOfCoreSlow(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (; ptr < ptrEnd; ptr++)
                    {
                        if (LegacyIndexOfCoreSlow(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static T* VectorizedPointerIndexOfCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated)
                {
                    if (ptr + Vector512<T>.Count < ptrEnd)
                    {
                        Vector512<T> maskVector = Vector512.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector512<T> valueVector = Vector512.Load(ptr);
                            Vector512<T> resultVector = VectorizedIndexOfCore_512(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                            {
                                ptr += Vector512<T>.Count;
                                continue;
                            }
                            return accurateResult ? ptr + FindIndexForResultVector_512(resultVector) : ptr;
                        }
                        while (ptr + Vector512<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Vector256.IsHardwareAccelerated)
                {
                    if (ptr + Vector256<T>.Count < ptrEnd)
                    {
                        Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            Vector256<T> resultVector = VectorizedIndexOfCore_256(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                            {
                                ptr += Vector256<T>.Count;
                                continue;
                            }
                            return accurateResult ? ptr + FindIndexForResultVector_256(resultVector) : ptr;
                        }
                        while (ptr + Vector256<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Vector128.IsHardwareAccelerated)
                {
                    if (ptr + Vector128<T>.Count < ptrEnd)
                    {
                        Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            Vector128<T> resultVector = VectorizedIndexOfCore_128(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                            {
                                ptr += Vector128<T>.Count;
                                continue;
                            }
                            return accurateResult ? ptr + FindIndexForResultVector_128(resultVector) : ptr;
                        }
                        while (ptr + Vector128<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                }
                if (Vector64.IsHardwareAccelerated)
                {
                    if (ptr + Vector64<T>.Count < ptrEnd)
                    {
                        Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            Vector64<T> resultVector = VectorizedIndexOfCore_64(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                            {
                                ptr += Vector64<T>.Count;
                                continue;
                            }
                            return accurateResult ? ptr + FindIndexForResultVector_64(resultVector) : ptr;
                        }
                        while (ptr + Vector64<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                        Vector64<T> valueVector = default, resultVector = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.InitBlockUnaligned(&resultVector, 0xFF, byteCount);
                        resultVector &= VectorizedIndexOfCore_64(valueVector, maskVector, method);
                        if (resultVector.Equals(default))
                            return null;
                        return accurateResult ? ptr + FindIndexForResultVector_64(resultVector) : ptr;
                    }
                    for (int i = 0; i < 2; i++) // CLR 編譯時會展開
                    {
                        if (LegacyIndexOfCoreFast(*ptr, value, method))
                            return ptr;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return null;
                }
#else
                if (Vector.IsHardwareAccelerated)
                {
                    if (ptr + Vector<T>.Count < ptrEnd)
                    {
                        Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                            Vector<T> resultVector = VectorizedIndexOfCore(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                            {
                                ptr += Vector<T>.Count;
                                continue;
                            }
                            return accurateResult ? ptr + FindIndexForResultVector(resultVector) : ptr;
                        }
                        while (ptr + Vector<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return null;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                        Vector<T> valueVector = default, resultVector = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.InitBlockUnaligned(&resultVector, 0xFF, byteCount);
                        resultVector &= VectorizedIndexOfCore(valueVector, maskVector, method);
                        if (resultVector.Equals(default))
                            return null;
                        return accurateResult ? ptr + FindIndexForResultVector(resultVector) : ptr;
                    }
                    for (int i = 0; i < 2; i++) // CLR 編譯時會展開
                    {
                        if (LegacyIndexOfCoreFast(*ptr, value, method))
                            return ptr;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return null;
                }
#endif
                for (; ptr < ptrEnd; ptr++)
                {
                    if (LegacyIndexOfCoreFast(*ptr, value, method))
                        return ptr;
                }
                return null;
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedIndexOfCore_512(in Vector512<T> valueVector, in Vector512<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector512.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector512.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector512.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector512.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector512.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector512.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedIndexOfCore_256(in Vector256<T> valueVector, in Vector256<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector256.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector256.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector256.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector256.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector256.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector256.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedIndexOfCore_128(in Vector128<T> valueVector, in Vector128<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector128.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector128.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector128.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector128.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector128.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector128.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedIndexOfCore_64(in Vector64<T> valueVector, in Vector64<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector64.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector64.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector64.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector64.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector64.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector64.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector_512(in Vector512<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector_256(in Vector256<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector_128(in Vector128<T> vector)
                => MathHelper.TrailingZeroCount(vector.ExtractMostSignificantBits());

            [Inline(InlineBehavior.Remove)]
            private static int FindIndexForResultVector_64(in Vector64<T> vector)
                => MathHelper.TrailingZeroCount(*(ulong*)UnsafeHelper.AsPointerIn(in vector)) / sizeof(T) / 8;
#else
            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedIndexOfCore(in Vector<T> valueVector, in Vector<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int FindIndexForResultVector(in Vector<T> vector)
            {
                ulong* ptrVector = (ulong*)UnsafeHelper.AsPointerIn(in vector);
                for (int i = 0; i < Vector<ulong>.Count; i++)
                {
                    int result = MathHelper.TrailingZeroCount(ptrVector[i]);
                    if (result == sizeof(ulong) * 8)
                        continue;
                    return i * (sizeof(ulong) / sizeof(T)) + result / sizeof(T) / 8;
                }
                return Vector<T>.Count;
            }
#endif

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCoreFast(T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => UnsafeHelper.Equals(item, value),
                    IndexOfMethod.Exclude => UnsafeHelper.NotEquals(item, value),
                    IndexOfMethod.GreaterThan => IsUnsigned() ? UnsafeHelper.IsGreaterThanUnsigned(item, value) : UnsafeHelper.IsGreaterThan(item, value),
                    IndexOfMethod.GreaterOrEqualsThan => IsUnsigned() ? UnsafeHelper.IsGreaterOrEqualsThanUnsigned(item, value) : UnsafeHelper.IsGreaterOrEqualsThan(item, value),
                    IndexOfMethod.LessThan => IsUnsigned() ? UnsafeHelper.IsLessThanUnsigned(item, value) : UnsafeHelper.IsLessThan(item, value),
                    IndexOfMethod.LessOrEqualsThan => IsUnsigned() ? UnsafeHelper.IsLessOrEqualsThanUnsigned(item, value) : UnsafeHelper.IsLessOrEqualsThan(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCoreSlow(EqualityComparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => comparer.Equals(item, value),
                    IndexOfMethod.Exclude => !comparer.Equals(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCoreSlow(Comparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
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
