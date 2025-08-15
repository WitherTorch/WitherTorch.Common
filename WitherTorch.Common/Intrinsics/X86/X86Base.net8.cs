#if NET8_0_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using InlineMethod;

using WitherTorch.Common.Helpers;

using RuntimeX86Base = System.Runtime.Intrinsics.X86.X86Base;

namespace WitherTorch.Common.Intrinsics.X86
{
	unsafe partial class X86Base
	{
		private static readonly delegate* managed<uint, uint> _bsfFunc, _bsrFunc;

		static X86Base()
		{
			if (!IsSupported)
				return;
			Type type = typeof(RuntimeX86Base);
			_bsfFunc = (delegate* managed<uint, uint>)ReflectionHelper.GetMethodPointer(type, "BitScanForward",
				[typeof(uint)], typeof(uint), BindingFlags.NonPublic | BindingFlags.Static);
			_bsrFunc = (delegate* managed<uint, uint>)ReflectionHelper.GetMethodPointer(type, "BitScanReverse",
				[typeof(uint)], typeof(uint), BindingFlags.NonPublic | BindingFlags.Static);
		}

		public static partial bool IsSupported
		{
			[Inline(InlineBehavior.Keep, export: true)]
			get => RuntimeX86Base.IsSupported;
		}

		[Inline(InlineBehavior.Keep, export: true)]
		public static partial (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId)
			=> RuntimeX86Base.CpuId(functionId, subFunctionId);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static partial uint BitScanForward(uint value) => _bsfFunc(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static partial uint BitScanReverse(uint value) => _bsrFunc(value);

		[RequiresPreviewFeatures]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static partial (int Quotient, int Remainder) DivRem(long dividend, int divisor)
		{
			(uint lower, int upper) = UnsafeHelper.As<long, PackedInt64>(dividend);
			return RuntimeX86Base.DivRem(lower, upper, divisor);
		}

		[RequiresPreviewFeatures]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static partial (uint Quotient, uint Remainder) DivRem(ulong dividend, uint divisor)
		{
			(uint lower, uint upper) = UnsafeHelper.As<ulong, PackedUInt64>(dividend);
			return RuntimeX86Base.DivRem(lower, upper, divisor);
		}

		[RequiresPreviewFeatures]
		[Inline(InlineBehavior.Keep, export: true)]
		public static partial (int Quotient, int Remainder) DivRem(uint lower, int upper, int divisor)
			=> RuntimeX86Base.DivRem(lower, upper, divisor);

		[RequiresPreviewFeatures]
		[Inline(InlineBehavior.Keep, export: true)]
		public static partial (uint Quotient, uint Remainder) DivRem(uint lower, uint upper, uint divisor)
			=> RuntimeX86Base.DivRem(lower, upper, divisor);

		[RequiresPreviewFeatures]
		[Inline(InlineBehavior.Keep, export: true)]
		public static partial (nint Quotient, nint Remainder) DivRem(nuint lower, nint upper, nint divisor)
			=> RuntimeX86Base.DivRem(lower, upper, divisor);

		[RequiresPreviewFeatures]
		[Inline(InlineBehavior.Keep, export: true)]
		public static partial (nuint Quotient, nuint Remainder) DivRem(nuint lower, nuint upper, nuint divisor)
			=> RuntimeX86Base.DivRem(lower, upper, divisor);

		[StructLayout(LayoutKind.Sequential, Pack = 4, Size = sizeof(long))]
		private readonly struct PackedInt64
		{
			private readonly uint _lower;
			private readonly int _upper;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Deconstruct(out uint lower, out int upper)
			{
				lower = _lower;
				upper = _upper;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4, Size = sizeof(ulong))]
		private readonly struct PackedUInt64
		{
			private readonly uint _lower;
			private readonly uint _upper;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Deconstruct(out uint lower, out uint upper)
			{
				lower = _lower;
				upper = _upper;
			}
		}
	}
}
#endif