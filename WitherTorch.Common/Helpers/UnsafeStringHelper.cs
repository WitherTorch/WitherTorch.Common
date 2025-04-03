using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using InlineIL;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static unsafe class UnsafeStringHelper
    {
        private static readonly void* _fastAllocateStringFuncPointer;

        static UnsafeStringHelper()
        {
            MethodInfo? method = typeof(string).GetMethod("FastAllocateString", BindingFlags.Static | BindingFlags.NonPublic,
                Type.DefaultBinder, CallingConventions.Standard, [typeof(int)], null);
            if (method is null)
            {
                _fastAllocateStringFuncPointer = (delegate*<int, string>)&LegacyAllocateRawString;
                Debug.WriteLine("Cannot find string.FastAllocateString method!, fallback to new string()");
            }
            else
            {
                _fastAllocateStringFuncPointer = (void*)method.MethodHandle.GetFunctionPointer();
            }
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static int GetStringLengthByHeadPointer(char* headPointer)
        {
            return *((int*)headPointer - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AllocateRawString(int length)
        {
            IL.Push(length);
            IL.Push(_fastAllocateStringFuncPointer);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConventions.Standard, typeof(string), typeof(int)));
            return IL.Return<string>();
        }

        private static string LegacyAllocateRawString(int length) => new string('\0', length);
    }
}
