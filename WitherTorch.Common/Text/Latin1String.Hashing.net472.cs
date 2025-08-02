#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Latin1String
    {
        private static class HashingHelper
        {
            private static readonly delegate* managed<Latin1String, int> _hashingFunc;

            static HashingHelper()
            {
                nint useMarvinHashingFunc = ReflectionHelper.GetMethodPointer(typeof(string), "InternalUseRandomizedHashing", null, typeof(bool),
                    BindingFlags.Static | BindingFlags.NonPublic);
                if (useMarvinHashingFunc == 0 || !((delegate* managed<bool>)useMarvinHashingFunc)())
                {
                    _hashingFunc = &NormalHashingFunction;
                    return;
                }

                _hashingFunc = &FallbackHashingFunction;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetHashCode(Latin1String str) => _hashingFunc(str);

            private static int FallbackHashingFunction(Latin1String str) => str.ToString().GetHashCode();

            private static int NormalHashingFunction(Latin1String str)
            {
                byte[] value = str._value;
                int length = str._length;
                if (length <= 0)
                    return string.Empty.GetHashCode();
                fixed (byte* ptr = value)
                {
                    DebugHelper.ThrowIf(ptr[length] != '\0', "ptr[length] should be equals zero!");
                    DebugHelper.ThrowIf((((nint)ptr) % 4) != 0, "Managed string should start at 4 bytes boundary!");

                    return UnsafeHelper.PointerSizeConstant switch
                    {
                        sizeof(int) => NormalHashingFunctionCore_32bit((uint*)ptr, length),
                        sizeof(long) => NormalHashingFunctionCore_64bit((byte*)ptr, length),
                        _ => UnsafeHelper.PointerSize switch
                        {
                            sizeof(int) => NormalHashingFunctionCore_32bit((uint*)ptr, length),
                            sizeof(long) => NormalHashingFunctionCore_64bit((byte*)ptr, length),
                            _ => FallbackHashingFunction(str),
                        },
                    };
                }
            }

            private static int NormalHashingFunctionCore_32bit(uint* ptr, int length)
            {
                const uint HashSeed = (5381 << 16) + 5381;
                uint hash1 = HashSeed, hash2 = HashSeed;

                while (length > 2)
                {
                    Widen(*ptr, out uint lower, out uint upper);
                    lower = WidenByteAndCompose(lower);
                    upper = WidenByteAndCompose(upper);

                    hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ WidenByteAndCompose(lower);
                    hash2 = ((hash2 << 5) + hash2 + (hash2 >> 27)) ^ WidenByteAndCompose(upper);
                    ptr++;
                    length -= 4;
                }

                if (length > 0)
                {
                    Widen(*ptr, out uint lower, out _);
                    hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ WidenByteAndCompose(lower);
                }

                return unchecked((int)(hash1 + (hash2 * 1566083941)));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void Widen(uint source, out uint lower, out uint upper)
            {
                lower = source & 0xFFFF;
                upper = (source >> 16) & 0xFFFF;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint WidenByteAndCompose(uint source)
            {
                uint lower = source & 0xFF;
                uint upper = (source >> 8) & 0xFF;
                return upper << 16 | lower;
            }

            private static unsafe int NormalHashingFunctionCore_64bit(byte* ptr, int length)
            {
                const int HashSeed = 5381;
                int hash1 = HashSeed, hash2 = HashSeed;

                int character;
                while ((character = ptr[0]) != 0)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ character;
                    character = ptr[1];
                    if (character == 0)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ character;
                    ptr += 2;
                }
                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
#endif
