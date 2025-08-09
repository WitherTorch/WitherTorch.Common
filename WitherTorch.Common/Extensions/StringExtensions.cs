using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;
using WitherTorch.Common.Buffers;

using LocalsInit;
using WitherTorch.Common.Text;


#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase ToStringBase(this string _this)
            => ToStringBase(_this, WTCommon.StringCreateOptions);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase ToStringBase(this string _this, StringCreateOptions options)
            => StringBase.Create(_this, options);

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
            fixed (char* ptr = value)
                return ToUpperAsciiCore(ptr, ptr + length) ?? value;
        }

        public static unsafe string ToLowerAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            fixed (char* ptr = value)
                return ToLowerAsciiCore(ptr, ptr + length) ?? value;
        }

        private static unsafe string? ToUpperAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: true);

        private static unsafe string? ToLowerAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: false);

        public static string[] WithPrefix(this string[] array, string prefix)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = array[i].WithPrefix(prefix);
            return array;
        }

#if NET472_OR_GREATER
        private const int SplitPathLength = 256;

        /// <inheritdoc cref="string.Split(char[])"/>
        [Inline(InlineBehavior.Remove)]
        public static string[] Split(this string _this, char separator) => Split(_this, separator, StringSplitOptions.None);

        /// <inheritdoc cref="string.Split(char[], StringSplitOptions)"/>>
        public static unsafe string[] Split(this string _this, char separator, StringSplitOptions options)
        {
            fixed (char* ptr = _this)
                return SplitCore(_this, ptr, _this.Length, separator, options);
        }

        /// <inheritdoc cref="string.Split(string[])"/>
        [Inline(InlineBehavior.Remove)]
        public static string[] Split(this string _this, string separator) => Split(_this, separator, StringSplitOptions.None);

        /// <inheritdoc cref="string.Split(char[], StringSplitOptions)"/>>
        public static unsafe string[] Split(this string _this, string separator, StringSplitOptions options)
        {
            fixed (char* ptr = _this, ptrSeparator = separator)
                return SplitCore(_this, ptr, _this.Length, ptrSeparator, separator.Length, options);
        }

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe string[] SplitCore(string _this, char* ptr, int length, char separator, StringSplitOptions options)
        {
            char** paths = stackalloc char*[SplitPathLength];

            if (length <= 0)
                return NeedRemoveEmptyEntries(options) ? Array.Empty<string>() : [_this];

            char* ptrEnd = ptr + length;
            nuint count = GetSplitCount(ptr, ptrEnd, paths, separator);
            if (count == 0)
                return [_this];
            if ((options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries)
            {
                ArrayPool<string?> pool = ArrayPool<string?>.Shared;
                string?[] buffer = pool.Rent(count + 1);
                if (count > SplitPathLength)
                    CopySplitStringIntoBuffer(ptr, ptrEnd, paths, separator, buffer);
                else
                    CopySplitStringIntoBuffer(ptr, ptrEnd, paths, count, separatorLength: 1u, buffer);
                return CollectAndRestoreBuffer(pool, buffer, count);
            }
            string[] result = new string[count + 1];
            if (count > SplitPathLength)
                CopySplitStringIntoBuffer(ptr, ptrEnd, paths, separator, result);
            else
                CopySplitStringIntoBuffer(ptr, ptrEnd, paths, count, separatorLength: 1u, result);
            return result;
        }

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe string[] SplitCore(string _this, char* ptr, int length, char* separator, int separatorLength, StringSplitOptions options)
        {
            char** paths = stackalloc char*[SplitPathLength];

            if (length <= 0)
                return NeedRemoveEmptyEntries(options) ? Array.Empty<string>() : [_this];
            if (separatorLength < 0)
                return [_this];

            char* ptrEnd = ptr + length;
            nuint castedSeparatorLength = unchecked((nuint)separatorLength);
            nuint count = GetSplitCount(ptr, ptrEnd, paths, separator, castedSeparatorLength);
            if (count == 0)
                return [_this];
            if ((options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries)
            {
                ArrayPool<string?> pool = ArrayPool<string?>.Shared;
                string?[] buffer = pool.Rent(count + 1);
                if (count > SplitPathLength)
                    CopySplitStringIntoBuffer(ptr, ptrEnd, paths, separator, castedSeparatorLength, buffer);
                else
                    CopySplitStringIntoBuffer(ptr, ptrEnd, paths, count, castedSeparatorLength, buffer);
                return CollectAndRestoreBuffer(pool, buffer, count);
            }
            string[] result = new string[count + 1];
            if (count > SplitPathLength)
                CopySplitStringIntoBuffer(ptr, ptrEnd, paths, separator, castedSeparatorLength, result);
            else
                CopySplitStringIntoBuffer(ptr, ptrEnd, paths, count, castedSeparatorLength, result);
            return result;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe nuint GetSplitCount(char* ptr, char* ptrEnd, char** paths, char separator)
        {
            nuint i = 0;
            while ((ptr = SequenceHelper.PointerIndexOf(ptr, ptrEnd, separator)) != null)
            {
                paths[i++] = ptr;
                ptr++;
                if (i >= SplitPathLength)
                {
                    CollectSplitCount(ref i, ref ptr, ptrEnd, separator);
                    break;
                }
            }
            return i;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe nuint GetSplitCount(char* ptr, char* ptrEnd, char** paths, char* separator, nuint separatorLength)
        {
            nuint i = 0;
            while ((ptr = PointerIndexOfCore(ptr, ptrEnd, separator, separatorLength)) != null)
            {
                paths[i++] = ptr;
                ptr += separatorLength;
                if (i >= SplitPathLength)
                {
                    CollectSplitCount(ref i, ref ptr, ptrEnd, separator, separatorLength);
                    break;
                }
            }
            return i;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe void CollectSplitCount(ref nuint i, ref char* ptr, char* ptrEnd, char separator)
        {
            while ((ptr = SequenceHelper.PointerIndexOf(ptr, ptrEnd, separator)) != null)
            {
                i++;
                ptr++;
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe void CollectSplitCount(ref nuint i, ref char* ptr, char* ptrEnd, char* separator, nuint separatorLength)
        {
            while ((ptr = PointerIndexOfCore(ptr, ptrEnd, separator, separatorLength)) != null)
            {
                i++;
                ptr += separatorLength;
            }
        }

        private static unsafe void CopySplitStringIntoBuffer(char* ptr, char* ptrEnd, char** paths, nuint pathCount, nuint separatorLength, string?[] buffer)
        {
            for (nuint i = 0; i < pathCount; i++)
            {
                char* path = paths[i];
                buffer[i] = new string(ptr, 0, unchecked((int)(path - ptr)));
                ptr = path + separatorLength;
            }
            buffer[pathCount] = new string(ptr, 0, unchecked((int)(ptrEnd - ptr)));
        }

        private static unsafe void CopySplitStringIntoBuffer(char* ptr, char* ptrEnd, char** paths, char separator, string?[] buffer)
        {
            nuint i;
            for (i = 0; i < SplitPathLength; i++)
            {
                char* path = paths[i];
                buffer[i] = new string(ptr, 0, unchecked((int)(path - ptr)));
                ptr = path + 1;
            }
            char* splitEnd;
            while ((splitEnd = SequenceHelper.PointerIndexOf(ptr, ptrEnd, separator)) != null)
            {
                buffer[i++] = new string(ptr, 0, unchecked((int)(splitEnd - ptr)));
                ptr = splitEnd + 1;
            }
            buffer[i] = new string(ptr, 0, unchecked((int)(ptrEnd - ptr)));
        }

        private static unsafe void CopySplitStringIntoBuffer(char* ptr, char* ptrEnd, char** paths, char* separator, nuint separatorLength, string?[] buffer)
        {
            nuint i;
            for (i = 0; i < SplitPathLength; i++)
            {
                char* path = paths[i];
                buffer[i] = new string(ptr, 0, unchecked((int)(path - ptr)));
                ptr = path + separatorLength;
            }
            char* splitEnd;
            while ((splitEnd = PointerIndexOfCore(ptr, ptrEnd, separator, separatorLength)) != null)
            {
                buffer[i++] = new string(ptr, 0, unchecked((int)(splitEnd - ptr)));
                ptr = splitEnd + separatorLength;
            }
            buffer[i] = new string(ptr, 0, unchecked((int)(ptrEnd - ptr)));
        }

        private static string[] CollectAndRestoreBuffer(ArrayPool<string?> pool, string?[] buffer, nuint count)
        {
            nuint newCount = count;
            for (nuint i = 0; i < count; i++)
            {
                if (string.IsNullOrEmpty(buffer[i]))
                {
                    buffer[i] = null;
                    newCount--;
                }
            }
            if (newCount <= 0)
            {
                pool.Return(buffer);
                return Array.Empty<string>();
            }
            string[] result = new string[newCount];
            for (nuint i = 0, j = 0; i < count; i++)
            {
                string? item = buffer[i];
                if (item is null)
                    continue;
                result[j++] = item;
            }
            pool.Return(buffer);
            return result;
        }

        [Inline(InlineBehavior.Remove)]
        private static bool NeedRemoveEmptyEntries(StringSplitOptions options)
            => (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe char* PointerIndexOfCore(char* ptrStart, char* ptrEnd, char* value, nuint valueLength)
        {
            switch (valueLength)
            {
                case 0:
                    return null;
                case 1:
                    return SequenceHelper.PointerIndexOf(ptrStart, ptrEnd, *value);
                default:
                    valueLength--;
                    break;
            }
            char firstChar = *value;
            ptrEnd -= valueLength;
            while ((ptrStart = SequenceHelper.PointerIndexOf(ptrStart, ptrEnd, firstChar)) != null)
            {
                if (SequenceHelper.Equals(ptrStart + 1, value + 1, valueLength))
                    return ptrStart;
                ptrStart++;
            }
            return null;
        }
#endif

        [Inline(InlineBehavior.Keep, export: true)]
        public static string WithPrefix(this string value, string prefix)
            => prefix + value;

        [Inline(InlineBehavior.Remove)]
        private static unsafe string? ToLowerOrUpperAsciiCore(char* ptr, char* ptrEnd, [InlineParameter] bool isUpper)
        {
            char* ptrStart = ptr;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                int resultLength = unchecked((int)(ptrEnd - ptrStart));
                string result = StringHelper.AllocateRawString(resultLength);
                fixed (char* ptrResult = result)
                    UnsafeHelper.CopyBlock(ptrResult, ptrStart, unchecked((uint)(resultLength * sizeof(char))));
                return result;
            });

#if NET6_0_OR_GREATER
            if (Limits.UseVector512())
            {
                Vector512<ushort>* ptrLimit = ((Vector512<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector512<ushort> maskVectorLow = Vector512.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector512<ushort> maskVectorHigh = Vector512.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector512<ushort> operationVector = Vector512.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector512<ushort> valueVector = Vector512.Load((ushort*)ptr);
                        Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector512.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore_512(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* ptrLimit = ((Vector256<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector256<ushort> maskVectorLow = Vector256.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector256<ushort> maskVectorHigh = Vector256.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector256<ushort> operationVector = Vector256.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector256<ushort> valueVector = Vector256.Load((ushort*)ptr);
                        Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector256.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore_256(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* ptrLimit = ((Vector128<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector128<ushort> maskVectorLow = Vector128.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector128<ushort> maskVectorHigh = Vector128.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector128<ushort> operationVector = Vector128.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector128<ushort> valueVector = Vector128.Load((ushort*)ptr);
                        Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector128.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore_128(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* ptrLimit = ((Vector64<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector64<ushort> maskVectorLow = Vector64.Create<ushort>(isUpper ? 'a' : 'A');
                    Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>(isUpper ? 'z' : 'Z');
                    Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector64<ushort> valueVector = Vector64.Load((ushort*)ptr);
                        Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore_64(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
#else
            if (Limits.UseVector())
            {
                Vector<ushort>* ptrLimit = ((Vector<ushort>*)ptr) + 1;
                if (ptrLimit < ptrEnd)
                {
                    Vector<ushort> maskVectorLow = new Vector<ushort>(isUpper ? 'a' : 'A');
                    Vector<ushort> maskVectorHigh = new Vector<ushort>(isUpper ? 'z' : 'Z');
                    Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                    do
                    {
                        Vector<ushort> valueVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(ptr);
                        Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                        if (!resultVector.Equals(default))
                        {
                            valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                            fixed (char* ptrResult = resultLazy.Value)
                                UnsafeHelper.WriteUnaligned(ptrResult + (ptr - ptrStart), valueVector);
                        }
                        ptr = (char*)ptrLimit;
                    } while (++ptrLimit < ptrEnd);
                    if (ptr >= ptrEnd)
                        return resultLazy.GetValueDirectly();
                }
            }
#endif
            for (; ptr < ptrEnd; ptr++)
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
            return resultLazy.GetValueDirectly();
        }

#if NET6_0_OR_GREATER
        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector512<ushort> VectorizedToLowerOrUpperAsciiCore_512(in Vector512<ushort> valueVector, in Vector512<ushort> operationVector,
            in Vector512<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector256<ushort> VectorizedToLowerOrUpperAsciiCore_256(in Vector256<ushort> valueVector, in Vector256<ushort> operationVector,
            in Vector256<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector128<ushort> VectorizedToLowerOrUpperAsciiCore_128(in Vector128<ushort> valueVector, in Vector128<ushort> operationVector,
            in Vector128<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector64<ushort> VectorizedToLowerOrUpperAsciiCore_64(in Vector64<ushort> valueVector, in Vector64<ushort> operationVector,
            in Vector64<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
#else
        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector<ushort> valueVector, in Vector<ushort> operationVector,
            in Vector<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
#endif

        [Inline(InlineBehavior.Remove)]
        private static unsafe void LegacyToLowerOrUpperAsciiCore(char* ptr, char* ptrStart, ref LazyTinyRefStruct<string> resultLazy, [InlineParameter] bool isUpper)
        {
            char c = *ptr;
            if (isUpper)
            {
                if (c < 'a' || c > 'z')
                    return;
            }
            else
            {
                if (c < 'A' || c > 'Z')
                    return;
            }
            fixed (char* ptrResult = resultLazy.Value)
                *(ptrResult + (ptr - ptrStart)) =
                    isUpper ? unchecked((char)(c - UpperLowerDiff)) : unchecked((char)(c + UpperLowerDiff));
        }
    }
}
