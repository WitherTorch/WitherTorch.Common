using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace WitherTorch.Common.Extensions
{
    public static class StringExtensions
    {
        private const int UpperLowerDiff = 'a' - 'A';

        [Inline(InlineBehavior.Keep, export: true)]
        public static char FirstOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[0];

        [Inline(InlineBehavior.Keep, export: true)]
        public static char LastOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[obj.Length - 1];

#if NET472_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool Contains(this string obj, char value) => StringHelper.Contains(obj, value);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Contains(this string obj, params char[] values) => StringHelper.Contains(obj, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, startIndex, length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
        {
            for (int i = startIndex; i < length; i++)
            {
                if (SequenceHelper.Equals(array[i], value, comparison))
                    return i;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith(this string str, char c)
        {
            int count = str.Length;
            if (count > 0)
                return str[count - 1] == c;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static bool StartsWith(this string str, char c)
            => !StringHelper.IsNullOrEmpty(str) && str[0] == c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool StartsWith(this string str, params char[] chars)
        {
            if (StringHelper.IsNullOrEmpty(str))
                return false;
            return chars.Contains(str[0]);
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
            fixed (char* ptr = value)
                return ToLowerAsciiCore(ptr, ptr + length) ?? value;
        }

        private static unsafe string? ToLowerAsciiCore(char* ptr, char* ptrEnd)
        {
            char* ptrStart = ptr;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                int length = unchecked((int)(ptrEnd - ptrStart));
                string result = StringHelper.AllocateRawString(length);
                fixed (char* ptrResult = result)
                    UnsafeHelper.CopyBlock(ptrResult, ptrStart, unchecked((uint)(length * sizeof(char))));
                return result;
            });
#if NET6_0_OR_GREATER
            if (Vector512.IsHardwareAccelerated)
            {
                if (ptr + Vector512<ushort>.Count < ptrEnd)
                {
                    Vector512<ushort> maskVectorLow = Vector512.Create<ushort>('A');
                    Vector512<ushort> maskVectorHigh = Vector512.Create<ushort>('Z');
                    Vector512<ushort> operationVector = Vector512.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector512<ushort> valueVector = Vector512.Load((ushort*)ptr);
                        Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector512.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            fixed (char* ptrResult = resultLazy.Value)
                                Vector512.Store(valueVector + (operationVector & resultVector), (ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr += Vector512<ushort>.Count;
                    } while (ptr + Vector512<ushort>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Vector256.IsHardwareAccelerated)
            {
                if (ptr + Vector256<ushort>.Count < ptrEnd)
                {
                    Vector256<ushort> maskVectorLow = Vector256.Create<ushort>('A');
                    Vector256<ushort> maskVectorHigh = Vector256.Create<ushort>('Z');
                    Vector256<ushort> operationVector = Vector256.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector256<ushort> valueVector = Vector256.Load((ushort*)ptr);
                        Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector256.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            fixed (char* ptrResult = resultLazy.Value)
                                Vector256.Store(valueVector + (operationVector & resultVector), (ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr += Vector256<ushort>.Count;
                    } while (ptr + Vector256<ushort>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Vector128.IsHardwareAccelerated)
            {
                if (ptr + Vector128<ushort>.Count < ptrEnd)
                {
                    Vector128<ushort> maskVectorLow = Vector128.Create<ushort>('A');
                    Vector128<ushort> maskVectorHigh = Vector128.Create<ushort>('Z');
                    Vector128<ushort> operationVector = Vector128.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector128<ushort> valueVector = Vector128.Load((ushort*)ptr);
                        Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector128.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            fixed (char* ptrResult = resultLazy.Value)
                                Vector128.Store(valueVector + (operationVector & resultVector), (ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr += Vector128<ushort>.Count;
                    } while (ptr + Vector128<ushort>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Vector64.IsHardwareAccelerated)
            {
                if (ptr + Vector64<ushort>.Count < ptrEnd)
                {
                    Vector64<ushort> maskVectorLow = Vector64.Create<ushort>('A');
                    Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>('Z');
                    Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector64<ushort> valueVector = Vector64.Load((ushort*)ptr);
                        Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            fixed (char* ptrResult = resultLazy.Value)
                                Vector64.Store(valueVector + (operationVector & resultVector), (ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr += Vector64<ushort>.Count;
                    } while (ptr + Vector64<ushort>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
                if (ptr + 2 < ptrEnd)
                {
                    Vector64<ushort> maskVectorLow = Vector64.Create<ushort>('A');
                    Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>('Z');
                    Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                    Vector64<ushort> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector += operationVector & resultVector;
                        fixed (char* ptrResult = resultLazy.Value)
                            UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                    }
                    return resultLazy.GetValueDirectly();
                }
                for (int i = 0; i < 2; i++) // CLR 會自動展開這個迴圈
                {
                    char c = *ptr;
                    if (c < 'A' || c > 'Z')
                        continue;
                    if (++ptr >= ptrEnd)
                        break;
                    fixed (char* ptrResult = resultLazy.Value)
                        *(ptrResult + (ptr - ptrStart)) = unchecked((char)(c + UpperLowerDiff));
                }
            }
#else
            if (Vector.IsHardwareAccelerated)
            {
                if (ptr + Vector<ushort>.Count < ptrEnd)
                {
                    Vector<ushort> maskVectorLow = new Vector<ushort>('A');
                    Vector<ushort> maskVectorHigh = new Vector<ushort>('Z');
                    Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector<ushort> valueVector = UnsafeHelper.Read<Vector<ushort>>(ptr);
                        Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            fixed (char* ptrResult = resultLazy.Value)
                                UnsafeHelper.Write(ptrResult + (ptr - ptrStart), valueVector + (operationVector & resultVector));
                        }
                        ptr += Vector<ushort>.Count;
                    } while (ptr + Vector<ushort>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
                if (ptr + 2 < ptrEnd)
                {
                    Vector<ushort> maskVectorLow = new Vector<ushort>('A');
                    Vector<ushort> maskVectorHigh = new Vector<ushort>('Z');
                    Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                    Vector<ushort> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector += operationVector & resultVector;
                        fixed (char* ptrResult = resultLazy.Value)
                            UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                    }
                    return resultLazy.GetValueDirectly();
                }
                for (int i = 0; i < 2; i++) // CLR 會自動展開這個迴圈
                {
                    char c = *ptr;
                    if (c < 'A' || c > 'Z')
                        continue;
                    if (++ptr >= ptrEnd)
                        break;
                    fixed (char* ptrResult = resultLazy.Value)
                        *(ptrResult + (ptr - ptrStart)) = unchecked((char)(c + UpperLowerDiff));
                }
                return resultLazy.GetValueDirectly();
            }
#endif
            for (; ptr < ptrEnd; ptr++)
            {
                char c = *ptr;
                if (c < 'A' || c > 'Z')
                    continue;
                fixed (char* ptrResult = resultLazy.Value)
                    *(ptrResult + (ptr - ptrStart)) = unchecked((char)(c + UpperLowerDiff));
            }
            return resultLazy.GetValueDirectly();
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
