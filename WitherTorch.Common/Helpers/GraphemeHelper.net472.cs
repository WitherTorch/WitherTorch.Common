#if NET472_OR_GREATER
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class GraphemeHelper
    {
        private static readonly delegate* managed<uint, UnicodeCategory> _internalGetUnicodeCategoryFunc = GetUnicodeCategoryFunc();

        private static delegate* managed<uint, UnicodeCategory> GetUnicodeCategoryFunc()
        {
            nint func = ReflectionHelper.GetMethodPointer(typeof(CharUnicodeInfo), "InternalGetUnicodeCategory",
                [typeof(int)], typeof(UnicodeCategory), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            if (func == 0)
                return &GetUnicodeCategoryFallback;
            return (delegate* managed<uint, UnicodeCategory>)func;
        }

        private static unsafe partial int[] GetGraphemeIndicesCore(char* ptr, int length)
        {
            using PooledList<int> list = new PooledList<int>(length);

            list.Add(0);

            char* iterator = ptr;
            char* ptrEnd = ptr + length;
            UnicodeCategory category = default;
            while ((iterator = GetNextGraphemePointer(iterator, ptrEnd, ref category)) != null)
                list.Add((int)(iterator - ptr));

            return list.ToArray();
        }

        private static char* GetNextGraphemePointer(char* ptr, char* ptrEnd, ref UnicodeCategory categoryCurrent)
        {
            if (ptr == null)
                return null;

            ptr = Utf8EncodingHelper.TryReadUtf16Character(ptr, ptrEnd, out uint unicodeValue);
            UnicodeCategory categoryNext = GetUnicodeCategory(unicodeValue);
            if (IsCombiningCategory(categoryNext) && !IsCombiningCategory(categoryCurrent) &&
                categoryCurrent != UnicodeCategory.Format && categoryCurrent != UnicodeCategory.Control &&
                categoryCurrent != UnicodeCategory.OtherNotAssigned && categoryCurrent != UnicodeCategory.Surrogate)
            {
                while ((ptr = Utf8EncodingHelper.TryReadUtf16Character(ptr, ptrEnd, out unicodeValue)) != null)
                {
                    categoryNext = GetUnicodeCategory(unicodeValue);
                    if (!IsCombiningCategory(categoryNext))
                    {
                        categoryCurrent = categoryNext;
                        goto Result;
                    }
                }
                goto Result;
            }
            categoryCurrent = categoryNext;
            goto Result;

        Result:
            return ptr >= ptrEnd ? null : ptr;
        }

        [Inline(InlineBehavior.Remove)]
        private static bool IsCombiningCategory(UnicodeCategory category)
            => category is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark or UnicodeCategory.EnclosingMark;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UnicodeCategory GetUnicodeCategory(uint unicodeValue) => _internalGetUnicodeCategoryFunc(unicodeValue);

        private static UnicodeCategory GetUnicodeCategoryFallback(uint codePoint)
        {
            string result = StringHelper.AllocateRawString(1 + MathHelper.BooleanToInt32(codePoint >= 0x10000));
            fixed (char* ptr = result)
                StringHelper.WriteUtf32CharacterToUtf16Buffer(ptr, codePoint);
            return CharUnicodeInfo.GetUnicodeCategory(result, 0);
        }
    }
}
#endif