﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Extensions
{
    public static class StringExtensions
    {
        private const int UpperLowerDiff = 'a' - 'A';

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static char First(this string obj)
        {
            return string.IsNullOrEmpty(obj) ? '\0' : obj[0];
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static char Last(this string obj)
        {
            return string.IsNullOrEmpty(obj) ? '\0' : obj[obj.Length - 1];
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool HasAnyItem(this string obj)
        {
            return obj.Length > 0;
        }

#if NET472_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool Contains(this string obj, char value)
        {
            return StringHelper.Contains(obj, value);
        }
#endif

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool Contains(this string obj, params char[] values)
        {
            return StringHelper.Contains(obj, values);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool Contains(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            return array.IndexOf(value, 0, array.Length, comparison) < 0;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool Contains(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
        {
            return array.IndexOf(value, startIndex, length, comparison) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static int IndexOf(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            return array.IndexOf(value, 0, array.Length, comparison);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static int IndexOf(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
        {
            for (int i = startIndex; i < length; i++)
            {
                if (value.Equals(array[i], comparison))
                    return i;
            }
            return -1;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool EndsWith(this string str, char c)
        {
            int count = str.Length;
            if (count > 0)
            {
                return str[count - 1] == c;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool EndsWith(this string str, params char[] chars)
        {
            int count = str.Length;
            if (count > 0)
            {
                char c = str[count - 1];
                for (int i = 0, length = chars.Length; i < length; i++)
                {
                    if (c == chars[i])
                        return true;
                }
            }
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool StartsWith(this string str, char c)
        {
            fixed (char* ptr = str)
                return *ptr == c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool StartsWith(this string str, params char[] chars)
        {
            fixed (char* ptr = str)
            {
                char c = *ptr;
                for (int i = 0, length = chars.Length; i < length; i++)
                {
                    if (c == chars[i])
                        return true;
                }
            }
            return false;
        }

        public static string[] ToUpperAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToUpperAscii(array[i]);
            return array;
        }

        public static string[] ToLowerAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToLowerAscii(array[i]);
            return array;
        }

        public static unsafe string ToUpperAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                string result = StringHelper.AllocateRawString(value.Length);
                fixed (char* ptr = value, ptr2 = result)
                    UnsafeHelper.CopyBlock(ptr2, ptr, unchecked((uint)(value.Length * sizeof(char))));
                return result;
            });
            fixed (char* ptr = value)
            {
                char* iterator = ptr;
                char* ptrEnd = ptr + length;
                if (Vector.IsHardwareAccelerated)
                {
                    char* ptrLimit = ptrEnd - Vector<ushort>.Count;
                    if (ptr < ptrLimit)
                    {
                        Vector<ushort> maskVector1 = default, maskVector2 = default;
                        {
                            ushort* pMask = (ushort*)UnsafeHelper.AsPointerRef(ref maskVector1); // 將要比對的項目擴充成向量
                            for (int i = 0; i < Vector<ushort>.Count; i++)
                                pMask[i] = 'a';
                        }
                        {
                            ushort* pMask = (ushort*)UnsafeHelper.AsPointerRef(ref maskVector2); // 將要比對的項目擴充成向量
                            for (int i = 0; i < Vector<ushort>.Count; i++)
                                pMask[i] = 'z';
                        }
                        do
                        {
                            Vector<ushort> valueVector = UnsafeHelper.Read<Vector<ushort>>(ptr);
                            Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVector1) & Vector.LessThanOrEqual(valueVector, maskVector2);
                            if (resultVector.Equals(default))
                                continue;
                            fixed (char* ptr2 = resultLazy.Value)
                            {
                                char* iterator2 = ptr2 + (iterator - ptr);
                                for (int i = 0; i < Vector<ushort>.Count; i++)
                                {
                                    if (resultVector[i] != default)
                                        iterator2[i] -= (char)UpperLowerDiff;
                                }
                            }
                        } while ((iterator += Vector<ushort>.Count) < ptrLimit);
                        if (iterator == ptrEnd)
                            return resultLazy.GetValueDirectly() ?? value;
                    }
                }

                for (; iterator < ptrEnd; iterator++)
                {
                    char c = *iterator;
                    if (c < 'a' || c > 'z')
                        continue;
                    fixed (char* ptr2 = resultLazy.Value)
                        ptr2[iterator - ptr] = unchecked((char)(c - UpperLowerDiff));
                }

                return resultLazy.GetValueDirectly() ?? value;
            }
        }

        public static unsafe string ToLowerAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                string result = StringHelper.AllocateRawString(value.Length);
                fixed (char* ptr = value, ptr2 = result)
                    UnsafeHelper.CopyBlock(ptr2, ptr, unchecked((uint)(value.Length * sizeof(char))));
                return result;
            });
            fixed (char* ptr = value)
            {
                char* iterator = ptr;
                char* ptrEnd = ptr + length;
                if (Vector.IsHardwareAccelerated)
                {
                    char* ptrLimit = ptrEnd - Vector<ushort>.Count;
                    if (ptr < ptrLimit)
                    {
                        Vector<ushort> maskVector1 = default, maskVector2 = default;
                        {
                            ushort* pMask = (ushort*)UnsafeHelper.AsPointerRef(ref maskVector1); // 將要比對的項目擴充成向量
                            for (int i = 0; i < Vector<ushort>.Count; i++)
                                pMask[i] = 'A';
                        }
                        {
                            ushort* pMask = (ushort*)UnsafeHelper.AsPointerRef(ref maskVector2); // 將要比對的項目擴充成向量
                            for (int i = 0; i < Vector<ushort>.Count; i++)
                                pMask[i] = 'Z';
                        }
                        do
                        {
                            Vector<ushort> valueVector = UnsafeHelper.Read<Vector<ushort>>(ptr);
                            Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVector1) & Vector.LessThanOrEqual(valueVector, maskVector2);
                            if (resultVector.Equals(default))
                                continue;
                            fixed (char* ptr2 = resultLazy.Value)
                            {
                                char* iterator2 = ptr2 + (iterator - ptr);
                                for (int i = 0; i < Vector<ushort>.Count; i++)
                                {
                                    if (resultVector[i] != default)
                                        iterator2[i] += (char)UpperLowerDiff;
                                }
                            }
                        } while ((iterator += Vector<ushort>.Count) < ptrLimit);
                        if (iterator == ptrEnd)
                            return resultLazy.GetValueDirectly() ?? value;
                    }
                }

                for (; iterator < ptrEnd; iterator++)
                {
                    char c = *iterator;
                    if (c < 'A' || c > 'Z')
                        continue;
                    fixed (char* ptr2 = resultLazy.Value)
                        ptr2[iterator - ptr] = unchecked((char)(c + UpperLowerDiff));
                }

                return resultLazy.GetValueDirectly() ?? value;
            }
        }

        public static string[] WithPrefix(this string[] array, string prefix)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = array[i].WithPrefix(prefix);
            return array;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static string WithPrefix(this string value, string prefix)
            => prefix + value;
    }
}
