using System.Runtime.InteropServices;
#if !DEBUG
using InlineIL;
#endif

namespace WitherTorch.Common.Windows.Structures
{

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 260 * sizeof(char))]
    public unsafe readonly struct FixedChar260
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;
        readonly ulong _10, _11, _12, _13, _14, _15, _16, _17, _18, _19;
        readonly ulong _20, _21, _22, _23, _24, _25, _26, _27, _28, _29;
        readonly ulong _30, _31, _32, _33, _34, _35, _36, _37, _38, _39;
        readonly ulong _40, _41, _42, _43, _44, _45, _46, _47, _48, _49;
        readonly ulong _50, _51, _52, _53, _54, _55, _56, _57, _58, _59;
        readonly ulong _60, _61, _62, _63;
        readonly uint _64;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar260>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar260 char260)
        {
            return new string((char*)&char260._0);
        }
#else
        public static implicit operator string(in FixedChar260 char260)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar260>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar260>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 256 * sizeof(char))]
    public unsafe readonly struct FixedChar256
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;
        readonly ulong _10, _11, _12, _13, _14, _15, _16, _17, _18, _19;
        readonly ulong _20, _21, _22, _23, _24, _25, _26, _27, _28, _29;
        readonly ulong _30, _31, _32, _33, _34, _35, _36, _37, _38, _39;
        readonly ulong _40, _41, _42, _43, _44, _45, _46, _47, _48, _49;
        readonly ulong _50, _51, _52, _53, _54, _55, _56, _57, _58, _59;
        readonly ulong _60, _61, _62, _63;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar256>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar256 char256)
        {
            return new string((char*)&char256._0);
        }
#else
        public static implicit operator string(in FixedChar256 char256)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar256>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar256>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 128 * sizeof(char))]
    public readonly unsafe struct FixedChar128
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;
        readonly ulong _10, _11, _12, _13, _14, _15, _16, _17, _18, _19;
        readonly ulong _20, _21, _22, _23, _24, _25, _26, _27, _28, _29;
        readonly ulong _30, _31;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar128>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar128 char128)
        {
            return new string((char*)&char128._0);
        }
#else
        public static implicit operator string(in FixedChar128 char128)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar128>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar128>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 80 * sizeof(char))]
    public unsafe readonly struct FixedChar80
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;
        readonly ulong _10, _11, _12, _13, _14, _15, _16, _17, _18, _19;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar80>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar80 char80)
        {
            return new string((char*)&char80._0);
        }
#else
        public static implicit operator string(in FixedChar80 char80)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar80>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar80>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 64 * sizeof(char))]
    public unsafe readonly struct FixedChar64
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;
        readonly ulong _10, _11, _12, _13, _14, _15;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar64>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar64 char64)
        {
            return new string((char*)&char64._0);
        }
#else
        public static implicit operator string(in FixedChar64 char64)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar64>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar64>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 32 * sizeof(char))]
    public unsafe readonly struct FixedChar32
    {
#if DEBUG
        readonly ulong _0, _1, _2, _3, _4, _5, _6, _7;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar32>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar32 char32)
        {
            return new string((char*)&char32._0);
        }
#else
        public static implicit operator string(in FixedChar32 char32)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar32>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar32>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 14 * sizeof(char))]
    public unsafe readonly struct FixedChar14
    {
#if DEBUG
        readonly ulong _0, _1, _2;
        readonly uint _4;
#else
        readonly ulong _0;
#endif

        public string Value
        {
            get
            {
#if DEBUG
                return this;
#else
                IL.Emit.Ldarg_0();
                IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar14>(), nameof(_0)));
                IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
                return IL.Return<string>();
#endif
            }
        }

#if DEBUG
        public static implicit operator string(FixedChar14 char14)
        {
            return new string((char*)&char14._0);
        }
#else
        public static implicit operator string(in FixedChar14 char32)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar14>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
        }
#endif

        public override string ToString()
        {
#if DEBUG
            return this;
#else
            IL.Emit.Ldarg_0();
            IL.Emit.Ldflda(FieldRef.Field(TypeRef.Type<FixedChar14>(), nameof(_0)));
            IL.Emit.Newobj(MethodRef.Constructor(TypeRef.Type<string>(), TypeRef.Type(typeof(char*))));
            return IL.Return<string>();
#endif
        }
    }
}
