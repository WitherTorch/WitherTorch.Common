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

        // https://unicode.org/reports/tr29/#Grapheme_Cluster_Boundaries
        // https://unicode.org/Public/UCD/latest/ucd/PropertyValueAliases.txt

        // ccs-base := [\p{L}\p{N}\p{P}\p{S}\p{Zs}]
        private static bool IsCcsBase(UnicodeCategory category)
            => category switch
            {
                // p{L}
                UnicodeCategory.LowercaseLetter => true, // p{Ll}
                UnicodeCategory.ModifierLetter => true, // p{Lm}
                UnicodeCategory.OtherLetter => true, // p{Lo}
                UnicodeCategory.TitlecaseLetter => true, // p{Lt}
                UnicodeCategory.UppercaseLetter => true, // p{Lu}
                // p{N}
                UnicodeCategory.DecimalDigitNumber => true, // p{Nd}
                UnicodeCategory.LetterNumber => true, // p{Nl}
                UnicodeCategory.OtherNumber => true, // p{No}
                // p{P}
                UnicodeCategory.ConnectorPunctuation => true, // p{Pc}
                UnicodeCategory.DashPunctuation => true, // p{Pd}
                UnicodeCategory.ClosePunctuation => true, // p{Pe}
                UnicodeCategory.FinalQuotePunctuation => true, // p{Pf}
                UnicodeCategory.InitialQuotePunctuation => true, // p{Pi}
                UnicodeCategory.OtherPunctuation => true, // p{Po}
                UnicodeCategory.OpenPunctuation => true, // p{Ps}
                // p{S}
                UnicodeCategory.CurrencySymbol => true, // p{Sc}
                UnicodeCategory.ModifierSymbol => true, // p{Sk}
                UnicodeCategory.MathSymbol => true, // p{Sm}
                UnicodeCategory.OtherSymbol => true, // p{So}
                // p{Zs}
                UnicodeCategory.SpaceSeparator => true,
                _ => false
            };

        // ccs-extended := [\p{M}\p{Join_Control}]
        private static bool IsCcsExtended(char c, UnicodeCategory category)
            => category switch
            {
                // p{M}
                UnicodeCategory.SpacingCombiningMark => true, // p{Mc}
                UnicodeCategory.EnclosingMark => true, // p{Me}
                UnicodeCategory.NonSpacingMark => true, // p{Mn}
                _ => IsJoinControl(c)
            };

        // p{Join_Control} := [\u200C \u200D]
        private static bool IsJoinControl(char c)
            => c switch
            {
                '\u200C' => true,
                '\u200D' => true,
                _ => false
            };
    }
}
#endif