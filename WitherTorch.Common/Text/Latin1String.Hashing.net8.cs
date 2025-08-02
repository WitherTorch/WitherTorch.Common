#if NET8_0_OR_GREATER
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Latin1String
    {
        private static class HashingHelper
        {
            private static readonly delegate* managed<ReadOnlySpan<byte>, ulong, int> _computeHash32Func;
            private static readonly delegate* managed<Latin1String, int> _hashingFunc;
            private static readonly ulong _hashingSeed;

            static HashingHelper()
            {
                Type? marvinType = Type.GetType("System.Marvin");
                if (marvinType is null)
                {
                    Debug.WriteLine("System.Marvin type not found, using slower hashing route! (=> string.GetHashCode() )");
                    goto Failed;
                }

                nint seedGetterFunc = ReflectionHelper.GetPropertyGetterPointer(marvinType, "DefaultSeed", typeof(ulong), BindingFlags.Static | BindingFlags.Public);
                if (seedGetterFunc == 0)
                {
                    Debug.WriteLine("Cannot getting hashing seed in System.Marvin type, using slower hashing route! (=> string.GetHashCode() )");
                    goto Failed;
                }
                _hashingSeed = ((delegate* managed<ulong>)seedGetterFunc)();

                nint hashingFunc = ReflectionHelper.GetMethodPointer(marvinType, "ComputeHash32", [typeof(ReadOnlySpan<byte>), typeof(ulong)], typeof(int));
                if (hashingFunc == 0)
                {
                    Debug.WriteLine("Cannot getting hashing function in System.Marvin type, using slower hashing route! (=> string.GetHashCode() )");
                    goto Failed;
                }

                _computeHash32Func = (delegate* managed<ReadOnlySpan<byte>, ulong, int>)hashingFunc;
                _hashingFunc = &MarvinHashingFunction;
                return;

            Failed:
                _hashingSeed = 0UL;
                _computeHash32Func = null;
                _hashingFunc = &FallbackHashingFunction;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int GetHashCode(Latin1String str) => _hashingFunc(str);

            private static int FallbackHashingFunction(Latin1String str) => str.ToString().GetHashCode();

            private static int MarvinHashingFunction(Latin1String str)
            {
                byte[] value = str._value;
                int length = str._length;
                if (length <= 0)
                    return string.Empty.GetHashCode();
                ArrayPool<char> pool = ArrayPool<char>.Shared;
                char[] buffer = pool.Rent(length);
                try
                {
                    fixed (byte* ptrSource = value)
                    fixed (char* ptrBuffer = buffer)
                    {
                        Latin1EncodingHelper.WriteToUtf16BufferCore(ptrSource, ptrBuffer, unchecked((nuint)length));
                        return _computeHash32Func(new ReadOnlySpan<byte>(ptrBuffer, length * sizeof(char)), _hashingSeed);
                    }
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }
    }
}
#endif
