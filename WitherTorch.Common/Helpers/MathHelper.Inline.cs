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
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != Booleans.FalseInt;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(uint value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != Booleans.FalseInt;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(long value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != Booleans.FalseLong;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(ulong value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != Booleans.FalseLong;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(nint value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != Booleans.FalseNative;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool ToBoolean(nuint value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != (nuint)Booleans.FalseNative;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool ToBoolean(void* value, [InlineParameter] bool usePreciseBooleanDefination)
        {
            if (usePreciseBooleanDefination)
            {
                IL.Push(value);
                IL.Emit.Conv_U1();
                return IL.Return<bool>();
            }
            return value != null;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int BooleanToInt32(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I4();
            return IL.Return<int>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static uint BooleanToUInt32(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_U4();
            return IL.Return<uint>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static long BooleanToInt64(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I8();
            return IL.Return<long>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ulong BooleanToUInt64(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_U8();
            return IL.Return<ulong>();
        }
    }
}
