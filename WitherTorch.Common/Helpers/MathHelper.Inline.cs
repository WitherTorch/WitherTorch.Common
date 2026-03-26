using System;
using System.Runtime.CompilerServices;

using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    partial class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<T>(T value) where T : unmanaged
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Cgt_Un();
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ToBoolean(void* value)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Cgt_Un();
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly bool ToBooleanUnsafe<T>(in T value) where T : unmanaged
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref readonly bool ToBooleanUnsafe(in void* value)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ret();
            throw IL.Unreachable();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static sbyte BooleanToInt8(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I4();
            return IL.Return<sbyte>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static byte BooleanToUInt8(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_U4();
            return IL.Return<byte>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static short BooleanToInt16(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I2();
            return IL.Return<short>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ushort BooleanToUInt16(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_U2();
            return IL.Return<ushort>();
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static nint BooleanToNative(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_I();
            return IL.Return<nint>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint BooleanToNativeUnsigned(bool value)
        {
            IL.Push(value);
            IL.Emit.Conv_U();
            return IL.Return<nuint>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static float BooleanToFloat32(bool value)
        {
            int result = -BooleanToInt32(value) & 0x3F800000;
            return UnsafeHelper.As<int, float>(ref result);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static double BooleanToFloat64(bool value)
        {
            long result = -BooleanToInt64(value) & 0x3FF0000000000000L;
            return UnsafeHelper.As<long, double>(ref result);
        }
    }
}
