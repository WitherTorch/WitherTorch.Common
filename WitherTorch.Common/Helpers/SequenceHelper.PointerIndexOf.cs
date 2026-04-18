using System.Runtime.CompilerServices;

using InlineMethod;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOf<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOf(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfExclude<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfExclude(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfGreaterThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfGreaterThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfGreaterThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfGreaterThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfLessThan<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfLessThan(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T* PointerIndexOfLessThanOrEquals<T>(T* ptrStart, T* ptrEnd, T value)
            => ptrStart >= ptrEnd ? null : PointerIndexOfLessThanOrEquals(ptrStart, unchecked((nuint)(ptrEnd - ptrStart)), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOf<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOfExclude<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOfGreaterThan<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOfGreaterThanOrEquals<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOfLessThan<T>(T* ptr, nuint length, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial T* PointerIndexOfLessThanOrEquals<T>(T* ptr, nuint length, T value);
    }
}
