using System.Runtime.CompilerServices;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOf<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOf(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfExclude<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfExclude(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfGreaterThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfGreaterThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfGreaterThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfGreaterThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfLessThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfLessThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static nuint CountOfLessThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? 0 : CountOfLessThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOf<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOfExclude<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOfGreaterThan<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOfLessThan<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial nuint CountOfLessThanOrEquals<T>(T* ptr, nuint length, T value);
    }
}
