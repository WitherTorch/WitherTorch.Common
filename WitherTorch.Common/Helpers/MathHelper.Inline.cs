using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(int value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                return IL.Return<bool>();
            }
            return value != Booleans.FalseInt;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int BooleanToInt32(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I4();
            return IL.Return<int>();
        }
    }
}
