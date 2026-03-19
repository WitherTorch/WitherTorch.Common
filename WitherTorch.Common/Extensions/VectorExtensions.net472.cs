#if NET472_OR_GREATER
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

#pragma warning disable CS8500

namespace WitherTorch.Common.Extensions
{
    unsafe partial class VectorExtensions
    {
        public static partial ulong ExtractMostSignificantBits<T>(this in Vector<T> _this) where T : struct
            => ExtractMostSignificantBitsCore<T>.Extract(in _this);

        private static class ExtractMostSignificantBitsCore<T> where T : struct
        {
            private static readonly Vector<ulong> _maskVector, _multiplyVector;
            private static readonly int _rightShift;

            static ExtractMostSignificantBitsCore()
            {
                switch (UnsafeHelper.SizeOf<T>())
                {
                    case sizeof(byte):
                        {
                            _maskVector = new Vector<ulong>(0x80_80_80_80_80_80_80_80u);
                            _multiplyVector = new Vector<ulong>(0x00_02_04_08_10_20_40_81u);
                            _rightShift = 56;
                      }
                        break;
                    case sizeof(ushort):
                        {
                            _maskVector = new Vector<ulong>(0x8000_8000_8000_8000u);
                            _multiplyVector = new Vector<ulong>(0x0000_2000_4000_8001u);
                            _rightShift = 60;
                        }
                        break;
                    case sizeof(uint):
                        {
                            _maskVector = new Vector<ulong>(0x80000000_80000000u);
                            _multiplyVector = new Vector<ulong>(0x00000000_80000001u);
                            _rightShift = 62;
                        }
                        break;
                    case sizeof(ulong):
                        {
                            _maskVector = new Vector<ulong>(0x8000000000000000u);
                            _multiplyVector = new Vector<ulong>(1);
                            _rightShift = 63;
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid element size!");
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ulong Extract(in Vector<T> source)
            {
                Vector<ulong> resultVector = (UnsafeHelper.As<Vector<T>, Vector<ulong>>(ref UnsafeHelper.AsRefIn(in source)) & _maskVector) * _multiplyVector;
                ulong* pResult = (ulong*)&resultVector;
                return UnsafeHelper.SizeOf<Vector<T>>() switch
                {
                    sizeof(ulong) => *pResult,
                    sizeof(ulong) * 2 => Extract_128(pResult, UnsafeHelper.SizeOfSigned<ulong>() / UnsafeHelper.SizeOfSigned<T>()),
                    sizeof(ulong) * 4 => Extract_256(pResult, UnsafeHelper.SizeOfSigned<ulong>() / UnsafeHelper.SizeOfSigned<T>()),
                    sizeof(ulong) * 8 => Extract_512(pResult, UnsafeHelper.SizeOfSigned<ulong>() / UnsafeHelper.SizeOfSigned<T>()),
                    _ => throw new InvalidOperationException("Invalid vector size!")
                };
            }

            [Inline(InlineBehavior.Remove)]
            private static ulong Extract_128(ulong* vector, int bitCountPer64)
                => Extract(vector[0]) | (Extract(vector[1]) << bitCountPer64);

            [Inline(InlineBehavior.Remove)]
            private static ulong Extract_256(ulong* vector, int bitCountPer64)
                => (Extract(vector[0]) | (Extract(vector[1]) << bitCountPer64)) |
                ((Extract(vector[2]) << (bitCountPer64 * 2)) | (Extract(vector[3]) << (bitCountPer64 * 3)));

            [Inline(InlineBehavior.Remove)]
            private static ulong Extract_512(ulong* vector, int bitCountPer64)
                => ((Extract(vector[0]) | (Extract(vector[1]) << bitCountPer64)) |
                ((Extract(vector[2]) << (bitCountPer64 * 2)) | (Extract(vector[3]) << (bitCountPer64 * 3)))) |
                (((Extract(vector[4]) << (bitCountPer64 * 4)) | (Extract(vector[5]) << (bitCountPer64 * 5))) |
                ((Extract(vector[6]) << (bitCountPer64 * 6)) | (Extract(vector[7]) << (bitCountPer64 * 7))));

            [Inline(InlineBehavior.Remove)]
            private static ulong Extract(ulong source) => source >>> _rightShift;
        }
    }
}
#endif