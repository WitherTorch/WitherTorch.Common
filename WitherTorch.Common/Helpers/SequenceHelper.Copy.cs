using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Native;

namespace WitherTorch.Common.Helpers
{
#pragma warning disable CS8500

    unsafe partial class SequenceHelper
    {
        public static void Copy<T>(T[] sourceArray, T[] destinationArray)
        {
            int length = sourceArray.Length;
            if (length <= 0 || ReferenceEquals(sourceArray, destinationArray))
                return;
            if (destinationArray.Length < length)
                throw new ArgumentException($"Cannot copy {sourceArray}'s items into ${destinationArray}!", nameof(destinationArray));
            fixed (T* source = sourceArray, destination = destinationArray)
                CopyCore(source, destination, unchecked((nuint)length));
        }

        public static void Copy<T>(T[] sourceArray, nuint sourceIndex, T[] destinationArray, nuint destinationIndex, nuint length)
        {
            if (length == 0)
                return;
            ThrowIfArgumentGroupGreaterThanLimit(sourceIndex, length, sourceArray.Length, nameof(sourceIndex), nameof(length));
            ThrowIfArgumentGroupGreaterThanLimit(destinationIndex, length, destinationArray.Length, nameof(destinationIndex), nameof(length));
            if (ReferenceEquals(sourceArray, destinationArray) && CheckOverlapped(sourceIndex, destinationIndex, length))
            {
                fixed (T* source = sourceArray, destination = destinationArray)
                    NativeMethods.MoveMemory(source + sourceIndex, destination + destinationIndex, length * UnsafeHelper.SizeOf<T>());
                return;
            }
            fixed (T* source = sourceArray, destination = destinationArray)
                CopyCore(source + sourceIndex, destination + destinationIndex, length);
        }

        public static void Copy<T>(T* source, T* destination, nuint count)
        {
            if (count == 0)
                return;

            if (CheckOverlapped(source, destination, count))
                NativeMethods.MoveMemory(destination, source, count * UnsafeHelper.SizeOf<T>());
            else
                CopyCore(source, destination, count);
        }

        [Inline(InlineBehavior.Remove)]
        private static void CopyCore<T>(T* source, T* destination, nuint count)
            => UnsafeHelper.CopyBlockUnaligned(destination, source, count * UnsafeHelper.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckOverlapped<T>(T* source, T* destination, nuint count)
        {
            T* sourceEnd = source + count;
            T* destinationEnd = destination + count;
            return (source >= destination || sourceEnd >= destinationEnd) && (source <= destination || sourceEnd <= destinationEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckOverlapped(nuint source, nuint destination, nuint count)
        {
            nuint sourceEnd = source + count;
            nuint destinationEnd = destination + count;
            return (source >= destination || sourceEnd >= destinationEnd) && (source <= destination || sourceEnd <= destinationEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfArgumentGroupGreaterThanLimit(nuint startIndex, nuint length, int limit, string argumentName1, string argumentName2)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException(argumentName1);
            nuint castedLimit = (nuint)limit;
            if (castedLimit <= startIndex)
                throw new ArgumentOutOfRangeException(argumentName1);
            castedLimit -= startIndex;
            if (castedLimit < length)
                throw new ArgumentOutOfRangeException(argumentName2);
        }
    }
}
