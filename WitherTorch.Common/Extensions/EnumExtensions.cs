using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasFlagOptimized<T>(this T _this, T flag) where T : Enum
        {
            switch (UnsafeHelper.SizeOf<T>())
            {
                case 1:
                    goto Size_1;
                case 2:
                    goto Size_2;
                case 4:
                    goto Size_4;
                case 8:
                    goto Size_8;
                default:
                    goto Fallback;
            }

        Size_1:
            return HasFlagCore<T, byte>(_this, flag);
        Size_2:
            return HasFlagCore<T, ushort>(_this, flag);
        Size_4:
            return HasFlagCore<T, uint>(_this, flag);
        Size_8:
            return HasFlagCore<T, ulong>(_this, flag);
        Fallback:
            return _this.HasFlag(flag);
        }

        [Inline(InlineBehavior.Remove)]
        private static bool HasFlagCore<T, TRaw>(T source, T flag)
            => UnsafeHelper.Equals(UnsafeHelper.And(UnsafeHelper.As<T, TRaw>(source), UnsafeHelper.As<T, TRaw>(flag)), UnsafeHelper.As<T, TRaw>(flag));
    }
}
